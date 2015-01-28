using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading;

namespace Ucoin.Framework.ObjectMapper
{
    internal class TypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
    {
        #region Fields

        private readonly ObjectMapper _container;
        private readonly object _lockObj = new object();
        private bool _compiled;
        private IInstanceCreator<TTarget> _creator = new DefaultCreator<TTarget>();
        private ActionInvokerBuilder<TSource, TTarget> _customInvokerBuilder;
        private Action<TSource, TTarget> _customMapper;
        private bool _initialized;
        private MemberMapperCollection _memberMappers;
        private MemberMapOptions _options;
        private bool _readonly;
        private MappingMemberCollection _targetMembers;

        private static readonly ConcurrentDictionary<ObjectMapper, TypeMapper<TSource, TTarget>> _instances =
            new ConcurrentDictionary<ObjectMapper, TypeMapper<TSource, TTarget>>();

        #endregion

        public static TypeMapper<TSource, TTarget> GetInstance(ObjectMapper container)
        {
             return _instances.GetOrAdd(container, CreateMapper);
        }

        private static TypeMapper<TSource, TTarget> CreateMapper(ObjectMapper container)
        {
            return new TypeMapper<TSource, TTarget>(container);
        }

        private TypeMapper(ObjectMapper container)
        {
            _container = container;
        }

        private void Initialize()
        {
            if (!_initialized)
            {
                Thread.MemoryBarrier();
                lock (_lockObj)
                {
                    if (!_initialized)
                    {
                        var context = new ConventionContext(_container, typeof(TSource), typeof(TTarget), _options);
                        _container.Conventions.Apply(context);
                        _targetMembers = context.TargetMembers;
                        _memberMappers = new MemberMapperCollection(_container, _options);
                        foreach (MemberMapping mapping in context.Mappings)
                        {
                            _memberMappers.Set(mapping.TargetMember, mapping.SourceMember, mapping.Converter);
                        }
                        _initialized = true;
                    }
                }
            }
        }

        private void CheckReadOnly()
        {
            if (_readonly)
            {
                throw new NotSupportedException("The type mapper is read-only");
            }
        }

        public void SetReadOnly()
        {
            if (!_readonly)
            {
                _readonly = true;
            }
        }

        public void Compile(ModuleBuilder builder)
        {
            if (!_compiled)
            {
                Initialize();
                _creator.Compile(builder);
                if (_customMapper != null)
                {
                    _customInvokerBuilder = new ActionInvokerBuilder<TSource, TTarget>(_customMapper);
                    _customInvokerBuilder.Compile(builder);
                }
                else
                {
                    foreach (MemberMapper mapper in _memberMappers)
                    {
                        mapper.Compile(builder);
                    }
                }
                _compiled = true;
            }
        }

        public Action<TSource, TTarget> CreateMapper(ModuleBuilder builder)
        {
            Initialize();
            if (_customMapper != null) return _customMapper;
            TypeBuilder typeBuilder = builder.DefineStaticType();
            MethodBuilder methodBuilder = typeBuilder.DefineStaticMethod("Map");
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(TSource), typeof(TTarget));

            ILGenerator il = methodBuilder.GetILGenerator();
            var context = new CompilationContext(il);
            context.SetSource(() => il.Emit(OpCodes.Ldarg_0));
            context.SetTarget(() => il.Emit(OpCodes.Ldarg_1));
            foreach (MemberMapper mapper in _memberMappers)
            {
                mapper.Emit(context);
            }
            context.Emit(OpCodes.Ret);
            return
                (Action<TSource, TTarget>)
                    Delegate.CreateDelegate(typeof(Action<TSource, TTarget>), typeBuilder.CreateType(), "Map");
        }

        public Func<TSource, TTarget> CreateConverter(ModuleBuilder builder)
        {
            Initialize();
            TypeBuilder typeBuilder = builder.DefineStaticType();
            MethodBuilder methodBuilder = typeBuilder.DefineStaticMethod("Map");
            methodBuilder.SetReturnType(typeof(TTarget));
            methodBuilder.SetParameters(typeof(TSource));

            ILGenerator il = methodBuilder.GetILGenerator();
            var context = new CompilationContext(il);
            context.SetSource(() => il.Emit(OpCodes.Ldarg_0));
            LocalBuilder targetLocal = il.DeclareLocal(typeof(TTarget));
            _creator.Emit(context);
            il.Emit(OpCodes.Stloc, targetLocal);
            context.SetTarget(() => il.Emit(OpCodes.Ldloc, targetLocal));
            if (_customInvokerBuilder != null)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc, targetLocal);
                _customInvokerBuilder.Emit(context);
            }
            else
            {
                foreach (MemberMapper mapper in _memberMappers)
                {
                    mapper.Emit(context);
                }
            }
            context.LoadTarget();
            context.Emit(OpCodes.Ret);
            return
                (Func<TSource, TTarget>)
                    Delegate.CreateDelegate(typeof(Func<TSource, TTarget>), typeBuilder.CreateType(), "Map");
        }

        #region Configuration

        public ITypeMapper<TSource, TTarget> WithOptions(MemberMapOptions options)
        {
            if (_initialized)
            {
                throw new NotSupportedException(
                    "The type mapper has been initialized. Please configure options before the other configurations.");
            }
            _options = options;
            return this;
        }

        public ITypeMapper<TSource, TTarget> MapWith(Action<TSource, TTarget> expression)
        {
            CheckReadOnly();
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _customMapper = expression;
            return this;
        }

        public ITypeMapper<TSource, TTarget> CreateWith(Func<TSource, TTarget> expression)
        {
            CheckReadOnly();
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _creator = new LambdaCreator<TSource, TTarget>(expression);
            return this;
        }

        public ITypeMapper<TSource, TTarget> MapMember<TMember>(string targetName, Func<TSource, TMember> expression)
        {
            CheckReadOnly();
            Initialize();
            if (string.IsNullOrEmpty(targetName))
            {
                throw new ArgumentException("The name of the target member cannot be null or empty.", "targetName");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            MappingMember targetMember = _targetMembers[targetName];
            if (targetMember != null)
            {
                _memberMappers.Set(targetMember, expression);
            }
            return this;
        }

        public ITypeMapper<TSource, TTarget> Ignore(IEnumerable<string> members)
        {
            CheckReadOnly();
            Initialize();
            if (members == null)
            {
                throw new ArgumentNullException("members");
            }
            foreach (string member in members)
            {
                if (string.IsNullOrEmpty(member))
                {
                    throw new ArgumentException("The name of the target member to be ignored cannot be null or empty.");
                }
                MappingMember targetMember = _targetMembers[member];
                if (targetMember != null)
                {
                    _memberMappers.Remove(targetMember);
                }
            }
            return this;
        }

        #endregion
    }
}