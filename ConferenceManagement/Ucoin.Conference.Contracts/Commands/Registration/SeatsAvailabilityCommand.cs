
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using Ucoin.Framework.Messaging;

    public abstract class SeatsAvailabilityCommand : ICommand, IMessageSessionProvider
    {
        public SeatsAvailabilityCommand()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ConferenceId { get; set; }

        string IMessageSessionProvider.SessionId
        {
            get { return this.SessionId; }
        }

        protected string SessionId
        {
            get { return "SeatsAvailability_" + this.ConferenceId.ToString(); }
        }
    }
}
