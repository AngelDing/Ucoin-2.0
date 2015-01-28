using System;
using System.Linq;

namespace Ucoin.Framework.ObjectMapper
{
    /// <summary>
    ///     The convention to match mapping member names with options.
    /// </summary>
    public class MatchNameConvention : IConvention
    {
        #region Fields

        private MemberMapOptions _options;
        private bool _readonly;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the options for member matching algorithm.
        /// </summary>
        /// <value>
        ///     The options for member matching algorithm.
        /// </value>
        public MemberMapOptions Options
        {
            get { return _options; }
            set
            {
                CheckReadOnly();
                _options = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Applies the convention to type member mappings.
        /// </summary>
        /// <param name="context">The context to apply convensions.</param>
        public void Apply(ConventionContext context)
        {
            MemberMapOptions options = context.Options == MemberMapOptions.Default ? Options : context.Options;
            bool includeNonPublic = (options & MemberMapOptions.NonPublic) == MemberMapOptions.NonPublic;
            MappingMember[] targetMembers =
                context.TargetMembers.Where(member => member.CanWrite(includeNonPublic)).ToArray();
            MappingMember[] sourceMembers =
                context.SourceMembers.Where(member => member.CanRead(includeNonPublic)).ToArray();
            StringComparison comparison = (options & MemberMapOptions.IgnoreCase) == MemberMapOptions.IgnoreCase
                ? StringComparison.CurrentCultureIgnoreCase
                : StringComparison.CurrentCulture;
            bool hierarchy = (options & MemberMapOptions.Hierarchy) == MemberMapOptions.Hierarchy;
            foreach (MappingMember targetMember in targetMembers)
            {
                foreach (MappingMember sourceMember in sourceMembers)
                {
                    if (string.Equals(sourceMember.MemberName, targetMember.MemberName, comparison) &&
                        (hierarchy || targetMember.MemberType.IsAssignableFrom(sourceMember.MemberType) ||
                         context.Converters.Get(sourceMember.MemberType, targetMember.MemberType) != null))
                    {
                        context.Mappings.Set(sourceMember, targetMember);
                        break;
                    }
                }
            }
        }

        void IConvention.SetReadOnly()
        {
            _readonly = true;
        }

        private void CheckReadOnly()
        {
            if (_readonly)
            {
                throw new NotSupportedException("The conversation is read-only");
            }
        }

        #endregion
    }
}