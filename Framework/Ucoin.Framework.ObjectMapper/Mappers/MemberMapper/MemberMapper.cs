using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal abstract class MemberMapper
    {
        private static readonly ConcurrentDictionary<TypeMapperKey, Type> _genericMapperTypes
            = new ConcurrentDictionary<TypeMapperKey, Type>();

        private readonly ObjectMapper _container;
        private readonly MemberMapOptions _options;
        private readonly MappingMember _targetMember;
        private Converter _converter;

        protected MemberMapper(ObjectMapper container, MemberMapOptions options, MappingMember targetMember,
            Converter converter)
        {
            _container = container;
            _options = options;
            _targetMember = targetMember;
            _converter = converter;
        }

        public MappingMember TargetMember
        {
            get { return _targetMember; }
        }

        public abstract Type SourceType { get; }

        public virtual void Compile(ModuleBuilder builder)
        {
            if (_converter != null)
            {
                _converter.Compile(builder);
            }
            else
            {
                Type sourceType = SourceType;
                Type targetType = TargetMember.MemberType;
                _converter = _container.Converters.Get(sourceType, targetType);
                if (_converter == null && (_options & MemberMapOptions.Hierarchy) == MemberMapOptions.Hierarchy &&
                    !(TargetMember.MemberType.IsValueType && targetType == sourceType))
                {
                    var key = new TypeMapperKey(_container, sourceType, targetType);
                    _genericMapperTypes.GetOrAdd(key, k => CreateMapper(builder, k.SourceType, k.TargetType));
                }
            }
        }

        private Type CreateMapper(ModuleBuilder builder, Type sourceType, Type targetType)
        {
            TypeBuilder typeBuilder = builder.DefineStaticType();
            // public static ObjectMapper Container;
            FieldBuilder field = typeBuilder.DefineField("Container", typeof(ObjectMapper),
                FieldAttributes.Public | FieldAttributes.Static);
            // Declare Convert method.
            {
                MethodBuilder methodBuilder = typeBuilder.DefineStaticMethod("Convert");
                methodBuilder.SetReturnType(targetType);
                methodBuilder.SetParameters(sourceType);

                MethodInfo convertMethod = typeof(ObjectMapper).GetMethods().Where(method =>
                {
                    if (method.Name != "Map" || !method.IsGenericMethodDefinition) return false;
                    method = method.MakeGenericMethod(sourceType, targetType);
                    if (method.ReturnType != targetType) return false;
                    ParameterInfo[] parameters = method.GetParameters();
                    return parameters.Length == 1 && parameters[0].ParameterType == sourceType;
                }).First();
                ILGenerator il = methodBuilder.GetILGenerator();
                il.Emit(OpCodes.Ldsfld, field);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, convertMethod.MakeGenericMethod(sourceType, targetType));
                il.Emit(OpCodes.Ret);
            }
            // Declare Map method.
            {
                MethodBuilder methodBuilder = typeBuilder.DefineStaticMethod("Map");
                methodBuilder.SetParameters(sourceType, targetType);

                MethodInfo mapMethod = typeof(ObjectMapper).GetMethods().Where(method =>
                {
                    if (method.Name != "Map" || method.ReturnType != typeof(void) || !method.IsGenericMethodDefinition)
                        return false;
                    method = method.MakeGenericMethod(sourceType, targetType);
                    ParameterInfo[] parameters = method.GetParameters();
                    return parameters.Length == 2 && parameters[0].ParameterType == sourceType &&
                           parameters[1].ParameterType == targetType;
                }).First();
                ILGenerator il = methodBuilder.GetILGenerator();
                il.Emit(OpCodes.Ldsfld, field);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Callvirt, mapMethod.MakeGenericMethod(sourceType, targetType));
                il.Emit(OpCodes.Ret);
            }

            Type type = typeBuilder.CreateType();
            type.FastSetField("Container", _container);
            return type;
        }

        private Action<CompilationContext> GetConvertEmitter(Type sourceType, Type targetType)
        {
            if (sourceType == targetType)
            {
                return context => { };
            }
            if (targetType.IsAssignableFrom(sourceType))
            {
                return context => context.EmitCast(targetType);
            }
            MethodInfo convertMethod = Helper.GetConvertMethod(sourceType, targetType);
            if (convertMethod != null)
            {
                return context =>
                {
                    context.EmitCall(convertMethod);
                    context.CurrentType = targetType;
                };
            }
            return null;
        }

        public virtual void Emit(CompilationContext context)
        {
            Type sourceType = SourceType;
            Type targetType = TargetMember.MemberType;
            if (_converter != null)
            {
                EmitSource(context);
                _converter.Emit(sourceType, targetType, context);
                EmitSetTarget(context);
                return;
            }
            if ((_options & MemberMapOptions.Hierarchy) != MemberMapOptions.Hierarchy)
            {
                Action<CompilationContext> converter = GetConvertEmitter(sourceType, targetType);
                if (converter != null)
                {
                    EmitSource(context);
                    converter(context);
                    EmitSetTarget(context);
                }
            }
            else
            {
                var key = new TypeMapperKey(_container, sourceType, targetType);
                Type mapperType = _genericMapperTypes[key];

                if (targetType.IsClass && !targetType.IsNullable())
                {
                    LocalBuilder sourceValue = context.DeclareLocal(sourceType);
                    EmitSource(context);
                    context.Emit(OpCodes.Stloc, sourceValue);

                    LocalBuilder targetValue = context.DeclareLocal(targetType);
                    ((IMemberBuilder)TargetMember).EmitGetter(context);
                    context.Emit(OpCodes.Stloc, targetValue);

                    Label label = context.DefineLabel();
                    context.Emit(OpCodes.Ldloc, targetValue);
                    context.Emit(OpCodes.Brtrue, label);

                    context.Emit(OpCodes.Ldloc, sourceValue);
                    context.CurrentType = sourceType;
                    context.EmitCall(mapperType.GetMethod("Convert"));
                    EmitSetTarget(context);

                    Label finalLabel = context.DefineLabel();
                    context.Emit(OpCodes.Br, finalLabel);

                    context.MakeLabel(label);

                    context.Emit(OpCodes.Ldloc, sourceValue);
                    context.Emit(OpCodes.Ldloc, targetValue);
                    context.EmitCall(mapperType.GetMethod("Map"));

                    context.MakeLabel(finalLabel);
                }
                else
                {
                    EmitSource(context);
                    context.CurrentType = sourceType;
                    context.EmitCall(mapperType.GetMethod("Convert"));
                    EmitSetTarget(context);
                }
            }
        }

        protected abstract void EmitSource(CompilationContext context);

        private void EmitSetTarget(CompilationContext context)
        {
            ((IMemberBuilder)TargetMember).EmitSetter(context);
        }
    }
}