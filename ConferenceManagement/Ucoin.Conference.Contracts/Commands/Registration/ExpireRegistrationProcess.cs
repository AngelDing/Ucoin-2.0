
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using Ucoin.Framework.Messaging;

    public class ExpireRegistrationProcess : ICommand
    {
        public ExpireRegistrationProcess()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProcessId { get; set; }
    }
}
