using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    /// <summary>
    ///     Describes the source and target of the mapping strategy for convensions.
    ///     This class cannot be inherited.
    /// </summary>
    public sealed class ConventionContext
    {
        private readonly ObjectMapper _container;
        private readonly MemberMappingCollection _mappings = new MemberMappingCollection();
        private readonly MemberMapOptions _options;
        private readonly MappingMemberCollection _sourceMembers;
        private readonly Type _sourceType;
        private readonly MappingMemberCollection _targetMembers;
        private readonly Type _targetType;

        internal ConventionContext(ObjectMapper container, Type sourceType, Type targetType, MemberMapOptions options)
        {
            _container = container;
            _sourceType = sourceType;
            _targetType = targetType;
            _options = options;
            _sourceMembers = new MappingMemberCollection(GetMembers(sourceType, true, false));
            _targetMembers = new MappingMemberCollection(GetMembers(targetType, false, true));
        }

        /// <summary>
        ///     Gets the source type to map from.
        /// </summary>
        /// <value>The type to map from.</value>
        public Type SourceType
        {
            get { return _sourceType; }
        }

        /// <summary>
        ///     Gets the target type to map to.
        /// </summary>
        /// <value>The target type to map to.</value>
        public Type TargetType
        {
            get { return _targetType; }
        }

        /// <summary>
        ///     Gets a <see cref="MappingMemberCollection" /> represents the members(properties and fields) of the
        ///     <see cref="SourceType" />.
        /// </summary>
        /// <value>The members of the <see cref="SourceType" />.</value>
        public MappingMemberCollection SourceMembers
        {
            get { return _sourceMembers; }
        }

        /// <summary>
        ///     Gets a <see cref="MappingMemberCollection" /> represents the members(properties and fields) of the
        ///     <see cref="TargetType" />.
        /// </summary>
        /// <value>The members of the <see cref="TargetType" />.</value>
        public MappingMemberCollection TargetMembers
        {
            get { return _targetMembers; }
        }

        /// <summary>
        ///     Gets a <see cref="MemberMappingCollection" /> represents the mappings between source members and target members.
        /// </summary>
        /// <value>The mappings between source members and target members.</value>
        public MemberMappingCollection Mappings
        {
            get { return _mappings; }
        }

        internal ConverterCollection Converters
        {
            get { return _container.Converters; }
        }

        /// <summary>
        ///     Gets the options that control the member matching algorithm.
        /// </summary>
        /// <value>The options that control the member matching algorithm.</value>
        public MemberMapOptions Options
        {
            get { return _options; }
        }

        private IEnumerable<MappingMember> GetMembers(Type type, bool includeReadOnly, bool includeWriteOnly)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetFields(bindingFlags).Select(field => (MappingMember) new MappingField(field))
                .Concat(type.GetProperties(bindingFlags).Select(property => (MappingMember)new MappingProperty(property)))
                .Where(
                    member => (member.CanRead(true) && includeReadOnly) || (member.CanWrite(true) && includeWriteOnly));
        }
    }
}