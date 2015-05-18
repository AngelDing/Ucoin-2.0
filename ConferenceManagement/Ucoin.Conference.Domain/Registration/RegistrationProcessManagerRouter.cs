using System;
using System.Linq;
using System.Diagnostics;
using Ucoin.Conference.Contracts.Commands.Registration;
using Ucoin.Conference.Contracts.Events.Payments;
using Ucoin.Conference.Contracts.Events.Registration;
using Ucoin.Conference.Repositories;
using Ucoin.Framework.Messaging;
using Ucoin.Framework.Messaging.Handling;
using Ucoin.Framework.Processes;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.Entities;
using Ucoin.Framework.EventSourcing;

namespace Ucoin.Conference.Domain
{
    public class RegistrationProcessManagerRouter :
        IEventHandler<OrderPlaced>,
        IEventHandler<OrderUpdated>,
        IEnvelopedEventHandler<SeatsReserved>,
        IEventHandler<PaymentCompleted>,
        IEventHandler<OrderConfirmed>,
        ICommandHandler<ExpireRegistrationProcess>
    {
        private IRegistrationProcessRepository repository;
        private IRepositoryContext context;

        public RegistrationProcessManagerRouter(IRepositoryContext context, IRegistrationProcessRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public void Handle(OrderPlaced @event)
        {
            var manager = new RegistrationProcessManager();
            manager.Handle(@event);
            context.RegisterNew(manager.ProcessInfo);
            context.Commit();
        }

        public void Handle(OrderUpdated @event)
        {
            var pInfo = repository.GetBy(x => x.OrderId == @event.SourceId).FirstOrDefault();
            if (pInfo != null)
            {
                var manager = new RegistrationProcessManager(pInfo);
                manager.Handle(@event);
                context.RegisterModified(manager.ProcessInfo);
                context.Commit();
            }
            else
            {
                Trace.TraceError("Failed to locate the registration process manager handling the order with id {0}.", @event.SourceId);
            }
        }

        public void Handle(Envelope<SeatsReserved> envelope)
        {
            var pInfo = repository.GetBy(x => x.ReservationId == envelope.Body.ReservationId).FirstOrDefault();
            if (pInfo != null)
            {
                var manager = new RegistrationProcessManager(pInfo);
                manager.Handle(envelope);
                context.RegisterModified(manager.ProcessInfo);
                context.Commit();
            }
            else
            {
                Trace.TraceError("Failed to locate the registration process manager handling the seat reservation with id {0}. TODO: should Cancel seat reservation!", envelope.Body.ReservationId);
            }
        }

        public void Handle(OrderConfirmed @event)
        {
            var pInfo = repository.GetBy(x => x.OrderId == @event.SourceId).FirstOrDefault();
            if (pInfo != null)
            {
                var manager = new RegistrationProcessManager(pInfo);
                manager.Handle(@event);
                context.RegisterModified(manager.ProcessInfo);
                context.Commit();
            }
            else
            {
                Trace.TraceError("Failed to locate the registration process manager handling the order with id {0}.", @event.SourceId);
            }
        }


        public void Handle(PaymentCompleted @event)
        {
            // TODO: should not skip the completed processes and try to re-acquire the reservation,
            // and if not possible due to not enough seats, move them to a "manual intervention" state.
            // This was not implemented but would be very important.
            var pInfo = repository.GetBy(x => x.OrderId == @event.PaymentSourceId).FirstOrDefault();
            if (pInfo != null)
            {
                var manager = new RegistrationProcessManager(pInfo);
                manager.Handle(@event);
                context.RegisterModified(manager.ProcessInfo);
                context.Commit();
            }
            else
            {
                Trace.TraceError("Failed to locate the registration process manager handling the paid order with id {0}.", @event.PaymentSourceId);
            }
        }

        public void Handle(ExpireRegistrationProcess command)
        {
            var pInfo = repository.GetBy(x => x.Id == command.ProcessId).FirstOrDefault();
            if (pInfo != null)
            {
                var manager = new RegistrationProcessManager(pInfo);
                manager.Handle(command);
                context.RegisterModified(manager.ProcessInfo);
                context.Commit();
            }
        }
    }
}
