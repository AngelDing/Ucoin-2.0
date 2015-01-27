using System;

namespace Ucoin.Framework.ObjectMapper
{
    internal sealed class LambdaConvention : IConvention
    {
        private readonly Action<ConventionContext> _action;

        public LambdaConvention(Action<ConventionContext> action)
        {
            _action = action;
        }

        public void Apply(ConventionContext context)
        {
            if (_action != null)
            {
                _action(context);
            }
        }

        void IConvention.SetReadOnly()
        {
        }
    }
}