﻿using System;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class LambdaCreator<TSource, TTarget> : IInstanceCreator<TTarget>
    {
        private readonly Func<TSource, TTarget> _expression;
        private FuncInvokerBuilder<TSource, TTarget> _invokerBuilder;

        public LambdaCreator(Func<TSource, TTarget> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _expression = expression;
        }

        public void Compile(ModuleBuilder builder)
        {
            if (_invokerBuilder == null)
            {
                _invokerBuilder = new FuncInvokerBuilder<TSource, TTarget>(_expression);
                _invokerBuilder.Compile(builder);
            }
        }

        public void Emit(CompilationContext context)
        {
            context.LoadSource();
            _invokerBuilder.Emit(context);
            context.CurrentType = typeof (TTarget);
        }
    }
}