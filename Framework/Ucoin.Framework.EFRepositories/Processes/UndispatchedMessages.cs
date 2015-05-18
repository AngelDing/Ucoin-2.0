
namespace Ucoin.Framework.SqlDb.Processes
{
    using System;

    public class UndispatchedMessages
    {
        protected UndispatchedMessages() { }

        public UndispatchedMessages(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }

        public string Commands { get; set; }
    }
}
