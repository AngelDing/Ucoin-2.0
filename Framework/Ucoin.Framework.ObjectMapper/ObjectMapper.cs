using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Compilation;
using System.Web.Hosting;

namespace Ucoin.Framework.ObjectMapper
{
    /// <summary>
    ///     Main entry point for the object mapper component.
    /// </summary>
    public class ObjectMapper
    {
        #region Fields

        private static readonly Lazy<ObjectMapper> _instance = new Lazy<ObjectMapper>(() => new ObjectMapper());
        private readonly ConventionCollection _conventions = new ConventionCollection();
        private readonly ConverterCollection _converters;
        private readonly object _lockObj = new object();
        private readonly ModuleBuilder _moduleBuilder;
        private bool _compiled;

        #endregion

        #region Constructor

        /// <summary>
        ///     Create new instance of <see cref="ObjectMapper" />.
        /// </summary>
        public ObjectMapper()
        {
            var assemblyName = new AssemblyName("Dextrys_ILEmit_TypeMaps");
            _moduleBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                    AssemblyBuilderAccess.RunAndSave)
                    .DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll", false);
            _converters = new ConverterCollection(this);
            _converters.Add(new PrimitiveConverter { Intrinsic = true });
            _converters.Add(new ObjectToStringConverter { Intrinsic = true });
            _converters.Add(new FromStringConverter { Intrinsic = true });

            #region bool Converters

            // sbyte -> bool
            Converters.AddIntrinsic((sbyte source) => source != 0);
            // byte -> bool
            Converters.AddIntrinsic((byte source) => source != 0);
            // char -> bool
            Converters.AddIntrinsic((char source) => source != 0);
            // int -> bool
            Converters.AddIntrinsic((int source) => source != 0);
            // uint -> bool
            Converters.AddIntrinsic((uint source) => source != 0);
            // long -> bool
            Converters.AddIntrinsic((long source) => source != 0);
            // ulong -> bool
            Converters.AddIntrinsic((ulong source) => source != 0);
            // short -> bool
            Converters.AddIntrinsic((short source) => source != 0);
            // ushort -> bool
            Converters.AddIntrinsic((ushort source) => source != 0);
            // decimal -> bool
            Converters.AddIntrinsic((decimal source) => source != 0);
            // float -> bool
            Converters.AddIntrinsic((float source) => source != 0);
            // double -> bool
            Converters.AddIntrinsic((double source) => source != 0);

            // bool -> int
            Converters.AddIntrinsic((bool source) => (source ? 1 : 0));
            // bool -> char
            Converters.AddIntrinsic((bool source) => source ? 'T' : 'F');
            // bool -> byte
            Converters.AddIntrinsic((bool source) => (byte)(source ? 1 : 0));
            // bool -> short
            Converters.AddIntrinsic((bool source) => (short)(source ? 1 : 0));
            // bool -> ushort
            Converters.AddIntrinsic((bool source) => (ushort)(source ? 1 : 0));
            // bool -> uint
            Converters.AddIntrinsic((bool source) => (uint)(source ? 1 : 0));
            // bool -> long
            Converters.AddIntrinsic((bool source) => (long)(source ? 1 : 0));
            // bool -> ulong
            Converters.AddIntrinsic((bool source) => (ulong)(source ? 1 : 0));
            // bool -> decimal
            Converters.AddIntrinsic((bool source) => (decimal)(source ? 1 : 0));
            // bool -> float
            Converters.AddIntrinsic((bool source) => (float)(source ? 1 : 0));
            // bool -> double
            Converters.AddIntrinsic((bool source) => (double)(source ? 1 : 0));
            // bool -> sbyte
            Converters.AddIntrinsic((bool source) => (sbyte)(source ? 1 : 0));

            #endregion

            #region DateTime Converters

            // DateTime -> long
            Converters.AddIntrinsic((DateTime source) => source.Ticks);
            // DateTime -> ulong
            Converters.AddIntrinsic((DateTime source) => (ulong)source.Ticks);

            // sbyte -> DateTime
            Converters.AddIntrinsic((sbyte source) => new DateTime(source));
            // byte -> DateTime
            Converters.AddIntrinsic((byte source) => new DateTime(source));
            // char -> DateTime
            Converters.AddIntrinsic((char source) => new DateTime(source));
            // int -> DateTime
            Converters.AddIntrinsic((int source) => new DateTime(source));
            // uint -> DateTime
            Converters.AddIntrinsic((uint source) => new DateTime(source));
            // long -> DateTime
            Converters.AddIntrinsic((long source) => new DateTime(source));
            // ulong -> DateTime
            Converters.AddIntrinsic((ulong source) => new DateTime((long)source));
            // short -> DateTime
            Converters.AddIntrinsic((short source) => new DateTime(source));
            // ushort -> DateTime
            Converters.AddIntrinsic((ushort source) => new DateTime(source));
            // decimal -> DateTime
            Converters.AddIntrinsic((decimal source) => new DateTime((long)source));
            // float -> DateTime
            Converters.AddIntrinsic((float source) => new DateTime((long)source));
            // double -> DateTime
            Converters.AddIntrinsic((double source) => new DateTime((long)source));

            #endregion

            #region TimeSpan Converters

            // TimeSpan -> long
            Converters.AddIntrinsic((TimeSpan source) => source.Ticks);
            // TimeSpan -> ulong
            Converters.AddIntrinsic((TimeSpan source) => (ulong)source.Ticks);

            // sbyte -> TimeSpan
            Converters.AddIntrinsic((sbyte source) => new TimeSpan(source));
            // byte -> TimeSpan
            Converters.AddIntrinsic((byte source) => new TimeSpan(source));
            // char -> TimeSpan
            Converters.AddIntrinsic((char source) => new TimeSpan(source));
            // int -> TimeSpan
            Converters.AddIntrinsic((int source) => new TimeSpan(source));
            // uint -> TimeSpan
            Converters.AddIntrinsic((uint source) => new TimeSpan(source));
            // long -> TimeSpan
            Converters.AddIntrinsic((long source) => new TimeSpan(source));
            // ulong -> TimeSpan
            Converters.AddIntrinsic((ulong source) => new TimeSpan((long)source));
            // short -> TimeSpan
            Converters.AddIntrinsic((short source) => new TimeSpan(source));
            // ushort -> TimeSpan
            Converters.AddIntrinsic((ushort source) => new TimeSpan(source));
            // decimal -> TimeSpan
            Converters.AddIntrinsic((decimal source) => new TimeSpan((long)source));
            // float -> TimeSpan
            Converters.AddIntrinsic((float source) => new TimeSpan((long)source));
            // double -> TimeSpan
            Converters.AddIntrinsic((double source) => new TimeSpan((long)source));

            #endregion

            #region Byte Array Converters

            // bool -> byte[]
            Converters.AddIntrinsic((bool source) => BitConverter.GetBytes(source));
            // byte[] -> bool
            Converters.AddIntrinsic((byte[] source) => source != null && BitConverter.ToBoolean(source, 0));

            // char -> byte[]
            Converters.AddIntrinsic((char source) => BitConverter.GetBytes(source));
            // byte[] -> char
            Converters.AddIntrinsic((byte[] source) => source == null ? '\0' : BitConverter.ToChar(source, 0));

            // double -> byte[]
            Converters.AddIntrinsic((double source) => BitConverter.GetBytes(source));
            // byte[] -> double
            Converters.AddIntrinsic((byte[] source) => source == null ? (double)0 : BitConverter.ToDouble(source, 0));

            // float -> byte[]
            Converters.AddIntrinsic((float source) => BitConverter.GetBytes(source));
            // byte[] -> float
            Converters.AddIntrinsic((byte[] source) => source == null ? (float)0 : BitConverter.ToSingle(source, 0));

            // short -> byte[]
            Converters.AddIntrinsic((short source) => BitConverter.GetBytes(source));
            // byte[] -> short
            Converters.AddIntrinsic((byte[] source) => source == null ? (short)0 : BitConverter.ToInt16(source, 0));

            // ushort -> byte[]
            Converters.AddIntrinsic((ushort source) => BitConverter.GetBytes(source));
            // byte[] -> ushort
            Converters.AddIntrinsic((byte[] source) => source == null ? (ushort)0 : BitConverter.ToUInt16(source, 0));

            // int -> byte[]
            Converters.AddIntrinsic((int source) => BitConverter.GetBytes(source));
            // byte[] -> int
            Converters.AddIntrinsic((byte[] source) => source == null ? 0 : BitConverter.ToInt32(source, 0));

            // uint -> byte[]
            Converters.AddIntrinsic((uint source) => BitConverter.GetBytes(source));
            // byte[] -> uint
            Converters.AddIntrinsic((byte[] source) => source == null ? (uint)0 : BitConverter.ToUInt32(source, 0));

            // long -> byte[]
            Converters.AddIntrinsic((long source) => BitConverter.GetBytes(source));
            // byte[] -> long
            Converters.AddIntrinsic((byte[] source) => source == null ? (long)0 : BitConverter.ToInt64(source, 0));

            // ulong -> byte[]
            Converters.AddIntrinsic((ulong source) => BitConverter.GetBytes(source));
            // byte[] -> ulong
            Converters.AddIntrinsic((byte[] source) => source == null ? (ulong)0 : BitConverter.ToUInt64(source, 0));

            #endregion

            #region Spartial

            Converters.AddIntrinsic((DbGeography source) => source);
            Converters.AddIntrinsic((DbGeography source) => source == null ? null : source.AsText());
            Converters.AddIntrinsic((DbGeography source) => source == null ? null : source.AsBinary());
            Converters.AddIntrinsic((string source) => string.IsNullOrWhiteSpace(source) ? null : DbGeography.FromText(source));
            Converters.AddIntrinsic((DbGeography source) => source.AsBinary());
            Converters.AddIntrinsic((byte[] source) => source == null ? null : DbGeography.FromBinary(source));

            Converters.AddIntrinsic((DbGeometry source) => source);
            Converters.AddIntrinsic((DbGeometry source) => source.AsText());
            Converters.AddIntrinsic((string source) => string.IsNullOrWhiteSpace(source) ? null : DbGeometry.FromText(source));
            Converters.AddIntrinsic((DbGeometry source) => source.AsBinary());
            Converters.AddIntrinsic((byte[] source) => source == null ? null : DbGeometry.FromBinary(source));

            #endregion

            #region Misc Converters

            // DateTime -> DateTimeOffset
            Converters.AddIntrinsic((DateTime source) => new DateTimeOffset(source));
            // Guid -> byte[]
            Converters.AddIntrinsic((Guid source) => source.ToByteArray());
            // byte[] -> Guid
            Converters.AddIntrinsic((byte[] source) => source == null ? Guid.Empty : new Guid(source));
            // byte[] -> IPAddress
            Converters.AddIntrinsic((byte[] source) => source != null ? new IPAddress(source) : null);
            // IPAddress -> byte[]
            Converters.AddIntrinsic((IPAddress source) => source == null ? (byte[])null : source.GetAddressBytes());
            // Type -> string
            Converters.AddIntrinsic((Type source) => source == null ? null : source.AssemblyQualifiedName);
            // TimeZoneInfo -> string
            Converters.AddIntrinsic((TimeZoneInfo source) => source == null ? null : source.ToSerializedString());

            // string -> Uri
            Converters.AddIntrinsic((string source) => string.IsNullOrWhiteSpace(source) ? null : new Uri(source.Trim()));
            // string -> Type
            Converters.AddIntrinsic(
                (string source) =>
                    string.IsNullOrWhiteSpace(source)
                        ? null
                        : HostingEnvironment.IsHosted ? BuildManager.GetType(source, true) : Type.GetType(source.Trim(), true));
            // string -> TimeZoneInfo
            Converters.AddIntrinsic(
                (string source) => string.IsNullOrWhiteSpace(source) ? null : TimeZoneInfo.FromSerializedString(source.Trim()));

            #endregion

            Conventions.Add<MatchNameConvention>();
        }

        #endregion

        #region Extension Points

        /// <summary>
        ///     Gets a <see cref="ConventionCollection" /> object that used to manage the convensions.
        /// </summary>
        /// <value>A <see cref="ConventionCollection" /> object that used to manage the convensions.</value>
        public ConventionCollection Conventions
        {
            get { return _conventions; }
        }

        /// <summary>
        ///     Registers a custom converter to the <see cref="ObjectMapper" /> instance.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="expression">Callback to convert from source type to the target type.</param>
        public void RegisterConverter<TSource, TTarget>(Func<TSource, TTarget> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            Converters.Add(expression);
        }

        #endregion

        #region Entry Points

        /// <summary>
        ///     Gets the default object mapper container.
        /// </summary>
        public static ObjectMapper Default
        {
            get
            {
                return _instance.Value;
            }
        }

        /// <summary>
        ///     Execute a mapping from the source object to a new target object.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source object to map from.</param>
        /// <returns>Mapped target object.</returns>
        public TTarget Map<TSource, TTarget>(TSource source)
        {
            return ExecutorFactory<TSource, TTarget>.GetConverter(this)(source);
        }

        /// <summary>
        ///     Execute a mapping from the source object to a new target object.
        /// </summary>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source object to map from.</param>
        /// <returns>Mapped target object.</returns>
        public TTarget Map<TTarget>(object source)
        {
            if (source == null) return default(TTarget);
            return (TTarget)this.FastInvoke(new[] { source.GetType(), typeof(TTarget) }, "Map", source);
        }

        /// <summary>
        ///     Execute a mapping from the source <see cref="IEnumerable{TSource}" /> to a new destination
        ///     <see cref="IEnumerable{TTarget}" />.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source.</typeparam>
        /// <typeparam name="TTarget">The element type of the target.</typeparam>
        /// <param name="sources">The source <see cref="IEnumerable{TSource}" /> to map from.</param>
        /// <returns>The mapped target <see cref="IEnumerable{TSource}" />.</returns>
        public IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> sources)
        {
            if (sources == null) return null;
            IInstanceMapper<TSource, TTarget> mapper = GetMapper<TSource, TTarget>();
            return sources.Select(new Func<TSource, TTarget>(mapper.Map));
        }

        /// <summary>
        ///     Execute a mapping from the source array of <typeparamref name="TSource" /> to a new destination array of
        ///     <typeparamref name="TTarget" />.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source array.</typeparam>
        /// <typeparam name="TTarget">The element type of the target array.</typeparam>
        /// <param name="sources">The source array to map from.</param>
        /// <returns>The mapped target array.</returns>
        public TTarget[] Map<TSource, TTarget>(TSource[] sources)
        {
            return (TTarget[])Map<TSource, TTarget>((ICollection<TSource>)sources);
        }

        /// <summary>
        ///     Execute a mapping from the source collection of <typeparamref name="TSource" /> to a new destination collection of
        ///     <typeparamref name="TTarget" />.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection.</typeparam>
        /// <typeparam name="TTarget">The element type of the target collection.</typeparam>
        /// <param name="sources">The source collection to map from.</param>
        /// <returns>The mapped target collection.</returns>
        public ICollection<TTarget> Map<TSource, TTarget>(ICollection<TSource> sources)
        {
            if (sources == null) return null;
            if (sources.Count == 0) return new TTarget[0];
            return Map<TSource, TTarget>((IEnumerable<TSource>)sources).ToArray();
        }

        /// <summary>
        ///     Execute a mapping from the source list of <typeparamref name="TSource" /> to a new destination list of
        ///     <typeparamref name="TTarget" />.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source list.</typeparam>
        /// <typeparam name="TTarget">The element type of the target list.</typeparam>
        /// <param name="sources">The source collection to map from.</param>
        /// <returns>The mapped target list.</returns>
        public IList<TTarget> Map<TSource, TTarget>(IList<TSource> sources)
        {
            return (IList<TTarget>)Map<TSource, TTarget>((ICollection<TSource>)sources);
        }

        /// <summary>
        ///     Execute a mapping from the source list of <typeparamref name="TSource" /> to a new destination list of
        ///     <typeparamref name="TTarget" />.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source list.</typeparam>
        /// <typeparam name="TTarget">The element type of the target list.</typeparam>
        /// <param name="sources">The source collection to map from.</param>
        /// <returns>The mapped target list.</returns>
        public List<TTarget> Map<TSource, TTarget>(List<TSource> sources)
        {
            if (sources == null) return null;
            if (sources.Count == 0) return new List<TTarget>();
            return new List<TTarget>(Map<TSource, TTarget>((IEnumerable<TSource>)sources));
        }

        /// <summary>
        ///     Execute a mapping from the source object to the existing target object.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source object to map from.</param>
        /// <param name="target">Target object to map into.</param>
        public void Map<TSource, TTarget>(TSource source, TTarget target)
        {
            ExecutorFactory<TSource, TTarget>.GetMapper(this)(source, target);
        }

        /// <summary>
        ///     Execute a mapping from the source <see cref="IEnumerable{TSource}" /> to the existing destination
        ///     <see cref="IEnumerable{TTarget}" />.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source array.</typeparam>
        /// <typeparam name="TTarget">The element type of the target array.</typeparam>
        /// <param name="sources">The source <see cref="IEnumerable{TSource}" /> to map from.</param>
        /// <param name="targets">The target <see cref="IEnumerable{TSource}" /> to map to.</param>
        public void Map<TSource, TTarget>(IEnumerable<TSource> sources, IEnumerable<TTarget> targets)
        {
            if (sources == null || targets == null) return;
            IInstanceMapper<TSource, TTarget> mapper = GetMapper<TSource, TTarget>();
            IEnumerator<TSource> sourceEnumerator = sources.GetEnumerator();
            IEnumerator<TTarget> targetEnumerator = targets.GetEnumerator();
            while (sourceEnumerator.MoveNext() && targetEnumerator.MoveNext())
            {
                mapper.Map(sourceEnumerator.Current, targetEnumerator.Current);
            }
        }

        /// <summary>
        ///     Returns a mapper instance for specified types.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <returns>A mapper instance for specified types.</returns>
        public IInstanceMapper<TSource, TTarget> GetMapper<TSource, TTarget>()
        {
            return InstanceMapper<TSource, TTarget>.GetInstance(this);
        }

        /// <summary>
        ///     Returns a type mapping instance of specified types for the configuration purpose.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <returns>A type mapping instance of specified types for the configuration purpose.</returns>
        public ITypeMapper<TSource, TTarget> Configure<TSource, TTarget>()
        {
            return TypeMapper<TSource, TTarget>.GetInstance(this);
        }

        #endregion

        #region Native Points

        /// <summary>
        ///     Gets a <see cref="ConverterCollection" /> object that used to register converters.
        /// </summary>
        /// <value>A <see cref="ConverterCollection" /> object that used to register converters.</value>
        internal ConverterCollection Converters
        {
            get { return _converters; }
        }

        internal Func<TSource, TTarget> GetMapFunc<TSource, TTarget>()
        {
            Compile();
            Converter converter = Converters.Get<TSource, TTarget>();
            if (converter != null)
            {
                return
                    (Func<TSource, TTarget>)
                        converter.CreateDelegate(typeof(TSource), typeof(TTarget), _moduleBuilder);
            }
            TypeMapper<TSource, TTarget> typeMapper = TypeMapper<TSource, TTarget>.GetInstance(this);
            typeMapper.SetReadOnly();
            typeMapper.Compile(_moduleBuilder);
            return typeMapper.CreateConverter(_moduleBuilder);
        }

        internal Action<TSource, TTarget> GetMapAction<TSource, TTarget>()
        {
            Compile();
            TypeMapper<TSource, TTarget> typeMapper = TypeMapper<TSource, TTarget>.GetInstance(this);
            typeMapper.SetReadOnly();
            typeMapper.Compile(_moduleBuilder);
            return typeMapper.CreateMapper(_moduleBuilder);
        }

        internal void Compile()
        {
            if (!_compiled)
            {
                lock (_lockObj)
                {
                    if (!_compiled)
                    {
                        Conventions.SetReadOnly();
                        Converters.SetReadOnly();
                        Converters.Compile(_moduleBuilder);
                        _compiled = true;
                    }
                }
            }
        }

        #endregion
    }
}