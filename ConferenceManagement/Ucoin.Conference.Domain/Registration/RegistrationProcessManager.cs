
namespace Ucoin.Conference.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ucoin.Framework.Processes;
    using Ucoin.Framework.Messaging;
    using Ucoin.Conference.Entities.Registration;
    using Ucoin.Conference.Contracts.Events.Registration;
    using Ucoin.Conference.Contracts.Events.Payments;
    using Ucoin.Conference.Contracts.Commands.Registration;
    using Ucoin.Framework.Entities;

    /// <summary>
    /// Represents a Process Manager that is in charge of communicating between the different distributed components
    /// when registering to a conference, reserving the seats, expiring the reservation in case the order is not
    /// completed in time, etc.
    /// </summary>
    public class RegistrationProcessManager : IProcessManager
    {
        private static readonly TimeSpan BufferTimeBeforeReleasingSeatsAfterExpiration = TimeSpan.FromMinutes(14);

        private readonly List<Envelope<ICommand>> commands = new List<Envelope<ICommand>>();

        public RegistrationProcess ProcessInfo { get; private set; }
        public RegistrationProcessManager(RegistrationProcess pInfo)
        {
            if (pInfo == null)
            {
                ProcessInfo = new RegistrationProcess();
            }
            else
            {
                ProcessInfo = pInfo;
            }
        }

        public RegistrationProcessManager()
            : this(new RegistrationProcess())
        { 
        }
      
        public ProcessStateType State
        {
            get { return (ProcessStateType)this.ProcessInfo.StateValue; }
            internal set { this.ProcessInfo.StateValue = (int)value; }
        }

        public IEnumerable<Envelope<ICommand>> Commands
        {
            get { return this.commands; }
        }

        public void Handle(OrderPlaced message)
        {
            if (this.State == ProcessStateType.NotStarted)
            {
                this.ProcessInfo.ConferenceId = message.ConferenceId;
                this.ProcessInfo.OrderId = message.SourceId;
                // Use the order id as an opaque reservation id for the seat reservation. 
                // It could be anything else, as long as it is deterministic from the OrderPlaced event.
                this.ProcessInfo.ReservationId = message.SourceId;
                this.ProcessInfo.ReservationAutoExpiration = message.ReservationAutoExpiration;
                var expirationWindow = message.ReservationAutoExpiration.Subtract(DateTime.UtcNow);

                if (expirationWindow > TimeSpan.Zero)
                {
                    this.State = ProcessStateType.AwaitingReservationConfirmation;
                    var seatReservationCommand =
                        new MakeSeatReservation
                        {
                            ConferenceId = this.ProcessInfo.ConferenceId,
                            ReservationId = this.ProcessInfo.ReservationId,
                            Seats = message.Seats.ToList()
                        };
                    this.ProcessInfo.SeatReservationCommandId = seatReservationCommand.Id;

                    this.AddCommand(new Envelope<ICommand>(seatReservationCommand)
                    {
                        TimeToLive = expirationWindow.Add(TimeSpan.FromMinutes(1)),
                    });

                    var expirationCommand = new ExpireRegistrationProcess { ProcessId = this.Id };
                    this.ProcessInfo.ExpirationCommandId = expirationCommand.Id;
                    this.AddCommand(new Envelope<ICommand>(expirationCommand)
                    {
                        Delay = expirationWindow.Add(BufferTimeBeforeReleasingSeatsAfterExpiration),
                    });
                }
                else
                {
                    this.AddCommand(new RejectOrder { OrderId = this.ProcessInfo.OrderId });
                    this.ProcessInfo.Completed = true;
                }
            }
            else
            {
                if (message.ConferenceId != this.ProcessInfo.ConferenceId)
                {
                    // throw only if not reprocessing
                    throw new InvalidOperationException();
                }
            }
        }

        public void Handle(OrderUpdated message)
        {
            if (this.State == ProcessStateType.AwaitingReservationConfirmation
                || this.State == ProcessStateType.ReservationConfirmationReceived)
            {
                this.State = ProcessStateType.AwaitingReservationConfirmation;

                var seatReservationCommand =
                    new MakeSeatReservation
                    {
                        ConferenceId = this.ProcessInfo.ConferenceId,
                        ReservationId = this.ProcessInfo.ReservationId,
                        Seats = message.Seats.ToList()
                    };
                this.ProcessInfo.SeatReservationCommandId = seatReservationCommand.Id;
                this.AddCommand(seatReservationCommand);
            }
            else
            {
                throw new InvalidOperationException("The order cannot be updated at this stage.");
            }
        }

        public void Handle(Envelope<SeatsReserved> envelope)
        {
            if (this.State == ProcessStateType.AwaitingReservationConfirmation)
            {
                if (envelope.CorrelationId != null)
                {
                    if (string.CompareOrdinal(this.ProcessInfo.SeatReservationCommandId.ToString(), envelope.CorrelationId) != 0)
                    {
                        // skip this event
                        Trace.TraceWarning("Seat reservation response for reservation id {0} does not match the expected correlation id.", envelope.Body.ReservationId);
                        return;
                    }
                }

                this.State = ProcessStateType.ReservationConfirmationReceived;

                this.AddCommand(new MarkSeatsAsReserved
                {
                    OrderId = this.ProcessInfo.OrderId,
                    Seats = envelope.Body.ReservationDetails.ToList(),
                    Expiration = this.ProcessInfo.ReservationAutoExpiration.Value,
                });
            }
            else if (string.CompareOrdinal(this.ProcessInfo.SeatReservationCommandId.ToString(), envelope.CorrelationId) == 0)
            {
                Trace.TraceInformation("Seat reservation response for request {1} for reservation id {0} was already handled. Skipping event.", envelope.Body.ReservationId, envelope.CorrelationId);
            }
            else
            {
                throw new InvalidOperationException("Cannot handle seat reservation at this stage.");
            }
        }

        public void Handle(PaymentCompleted @event)
        {
            if (this.State == ProcessStateType.ReservationConfirmationReceived)
            {
                this.State = ProcessStateType.PaymentConfirmationReceived;
                this.AddCommand(new ConfirmOrder { OrderId = this.ProcessInfo.OrderId });
            }
            else
            {
                throw new InvalidOperationException("Cannot handle payment confirmation at this stage.");
            }
        }

        public void Handle(OrderConfirmed @event)
        {
            if (this.State == ProcessStateType.ReservationConfirmationReceived || this.State == ProcessStateType.PaymentConfirmationReceived)
            {
                this.ProcessInfo.ExpirationCommandId = Guid.Empty;
                this.ProcessInfo.Completed = true;

                this.AddCommand(new CommitSeatReservation
                {
                    ReservationId = this.ProcessInfo.ReservationId,
                    ConferenceId = this.ProcessInfo.ConferenceId
                });
            }
            else
            {
                throw new InvalidOperationException("Cannot handle order confirmation at this stage.");
            }
        }

        public void Handle(ExpireRegistrationProcess command)
        {
            if (this.ProcessInfo.ExpirationCommandId == command.Id)
            {
                this.ProcessInfo.Completed = true;

                this.AddCommand(new RejectOrder { OrderId = this.ProcessInfo.OrderId });
                this.AddCommand(new CancelSeatReservation
                {
                    ConferenceId = this.ProcessInfo.ConferenceId,
                    ReservationId = this.ProcessInfo.ReservationId,
                });

                // TODO cancel payment if any
            }

            // else ignore the message as it is no longer relevant (but not invalid)
        }

        private void AddCommand<T>(T command)
            where T : ICommand
        {
            this.commands.Add(Envelope.Create<ICommand>(command));
        }

        private void AddCommand(Envelope<ICommand> envelope)
        {
            this.commands.Add(envelope);
        }

        public Guid Id
        {
            get { return this.ProcessInfo.Id; }
        }

        public bool Completed
        {
            get { return this.ProcessInfo.Completed; }
        }
    }
}
