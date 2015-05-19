using System.Transactions;

namespace Ucoin.Framework
{
    public class ReadUncommittedScope : DisposableObject
    {
        private TransactionScope readUncommittedScope;

        public ReadUncommittedScope()
        {
            var options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
            readUncommittedScope = new TransactionScope(TransactionScopeOption.Required, options);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                readUncommittedScope.Complete();
            }
        }
    }
}
