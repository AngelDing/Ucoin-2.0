using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ucoin.Framework.ObjectMapper
{
    /// <summary>
    ///     Represents a collection of member objects that inherit from <see cref="MappingMember" />.
    /// </summary>
    public class MappingMemberCollection : IEnumerable<MappingMember>
    {
        #region Fields

        private readonly Hashtable _members;

        #endregion

        #region Constructors

        internal MappingMemberCollection()
            : this(StringComparer.Ordinal)
        {
        }

        internal MappingMemberCollection(IEnumerable<MappingMember> collection, StringComparer comparer)
        {
            _members = new Hashtable(comparer);
            if (collection != null)
            {
                foreach (MappingMember item in collection)
                {
                    Add(item);
                }
            }
        }

        internal MappingMemberCollection(IEnumerable<MappingMember> collection)
            : this(collection, StringComparer.Ordinal)
        {
        }

        internal MappingMemberCollection(StringComparer comparer)
            : this(null, comparer)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the member with the specified name.
        /// </summary>
        /// <param name="name">The name by which the member is identified.</param>
        /// <returns>The member with the specified name.</returns>
        public MappingMember this[string name]
        {
            get { return (MappingMember) _members[name]; }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="IEnumerator{MappingMember}" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<MappingMember> GetEnumerator()
        {
            return _members.Values.Cast<MappingMember>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Add(MappingMember item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (string.IsNullOrEmpty(item.MemberName))
            {
                throw new ArgumentException("Member name cannot be null or empty");
            }
            if (this[item.MemberName] != null)
            {
                throw new ArgumentException("Member name cannot be duplicated.");
            }
            _members.Add(item.MemberName, item);
        }

        #endregion
    }
}