using System;
using System.Collections.Generic;
using System.Linq;

namespace Ucoin.Framework.ObjectMapper
{
    /// <summary>
    ///     Represents a collection of convention objects that inherit from <see cref="IConvention" />.
    /// </summary>
    public sealed class ConventionCollection
    {
        private readonly List<IConvention> _convensions = new List<IConvention>();
        private bool _readonly;

        private void CheckReadOnly()
        {
            if (_readonly)
            {
                throw new NotSupportedException("Collection is read-only");
            }
        }

        internal void SetReadOnly()
        {
            if (!_readonly)
            {
                _readonly = true;
                _convensions.ForEach(conversation => conversation.SetReadOnly());
            }
        }

        /// <summary>
        ///     Removes all convensions from the collection.
        /// </summary>
        public void Clear()
        {
            CheckReadOnly();
            _convensions.Clear();
        }

        /// <summary>
        ///     Adds a convention to the collection.
        /// </summary>
        /// <param name="convention"></param>
        public void Add(IConvention convention)
        {
            CheckReadOnly();
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }
            _convensions.Add(convention);
        }

        /// <summary>
        ///     Adds a convention to the collection through a callback expression.
        /// </summary>
        /// <param name="action">The callback expression to apply the convention.</param>
        public void Add(Action<ConventionContext> action)
        {
            CheckReadOnly();
            Add(new LambdaConvention(action));
        }

        /// <summary>
        ///     Adds a convention to the collection through the type of the convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool Add<T>()
            where T : IConvention, new()
        {
            CheckReadOnly();
            if (!Contains<T>())
            {
                Add(new T());
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Remove all the convensions with the specified type.
        /// </summary>
        /// <param name="convensionType">The specified type of the convensions to be removed.</param>
        /// <returns>The number of convensions removed from the <see cref="ConventionCollection" />.</returns>
        public int Remove(Type convensionType)
        {
            CheckReadOnly();
            if (convensionType == null)
            {
                throw new ArgumentNullException("convensionType");
            }
            return _convensions.RemoveAll(convensionType.IsInstanceOfType);
        }

        /// <summary>
        ///     Remove all the convensions with the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type of the convensions to be removed.</typeparam>
        /// <returns>The number of convensions removed from the <see cref="ConventionCollection" />.</returns>
        public int Remove<T>()
            where T : IConvention
        {
            CheckReadOnly();
            return Remove(typeof (T));
        }

        /// <summary>
        ///     Removes the first occurrence of a specific convention from the <see cref="ConventionCollection" />.
        /// </summary>
        /// <param name="convention">The convention to remove from the <see cref="ConventionCollection" />.</param>
        /// <returns><c>true</c> if <paramref name="convention" /> is successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(IConvention convention)
        {
            CheckReadOnly();
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }
            return _convensions.Remove(convention);
        }

        /// <summary>
        ///     Retrieves the first convention occurrence of the specified type for the configuration purpose.
        /// </summary>
        /// <param name="convensionType">The specified type to retrieve the convention.</param>
        /// <returns>
        ///     The first convention occurrence of the specified type if there is any convention is the type of
        ///     <paramref name="convensionType" />, otherwise <see langword="null" />.
        /// </returns>
        public object Configure(Type convensionType)
        {
            CheckReadOnly();
            if (convensionType == null)
            {
                throw new ArgumentNullException("convensionType");
            }
            return _convensions.FirstOrDefault(convensionType.IsInstanceOfType);
        }

        /// <summary>
        ///     Retrieves the first convention occurrence of the specified type for the configuration purpose.
        /// </summary>
        /// <typeparam name="T">The specified type to retrieve the convention.</typeparam>
        /// <returns>
        ///     The first convention occurrence of the specified type if there is any convention is the type of
        ///     <typeparamref name="T" />, otherwise the default value of <typeparamref name="T" />.
        /// </returns>
        public T Configure<T>()
            where T : IConvention
        {
            return (T) Configure(typeof (T));
        }

        /// <summary>
        ///     Determines whether any convention is the specified type.
        /// </summary>
        /// <param name="convensionType">The specified type to examine the convensions.</param>
        /// <returns><c>true</c> if any convention type is <paramref name="convensionType" />; otherwise, <c>false</c>.</returns>
        public bool Contains(Type convensionType)
        {
            if (convensionType == null)
            {
                throw new ArgumentNullException("convensionType");
            }
            return _convensions.Where(convensionType.IsInstanceOfType).Any();
        }

        /// <summary>
        ///     Determines whether any convention is the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type to examine the convensions.</typeparam>
        /// <returns><c>true</c> if any convention type is <typeparamref name="T" />; otherwise, <c>false</c>.</returns>
        public bool Contains<T>()
            where T : IConvention
        {
            return Contains(typeof (T));
        }

        internal void Apply(ConventionContext context)
        {
            foreach (IConvention convension in _convensions)
            {
                convension.Apply(context);
            }
        }
    }
}