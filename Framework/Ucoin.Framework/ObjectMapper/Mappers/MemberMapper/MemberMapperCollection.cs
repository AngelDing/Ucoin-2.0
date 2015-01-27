using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ucoin.Framework.ObjectMapper
{
    internal class MemberMapperCollection : IEnumerable<MemberMapper>
    {
        private readonly ObjectMapper _container;
        private readonly List<MemberMapper> _mappers = new List<MemberMapper>();
        private readonly MemberMapOptions _options;
        private bool _readonly;

        public MemberMapperCollection(ObjectMapper container, MemberMapOptions options)
        {
            _container = container;
            _options = options;
        }

        public MemberMapper this[MappingMember targetMember]
        {
            get { return Get(targetMember); }
        }

        public IEnumerator<MemberMapper> GetEnumerator()
        {
            return _mappers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void CheckReadOnly()
        {
            if (_readonly)
            {
                throw new NotSupportedException("Collection is read-only");
            }
        }

        public void SetReadOnly()
        {
            if (!_readonly)
            {
                _readonly = true;
            }
        }

        public void Clear()
        {
            CheckReadOnly();
            _mappers.Clear();
        }

        public void Set(MemberMapper mapper)
        {
            CheckReadOnly();
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            Remove(mapper.TargetMember);
            _mappers.Add(mapper);
        }

        public void Set(MappingMember targetMember, MappingMember sourceMember, Converter converter)
        {
            CheckReadOnly();
            if (targetMember == null)
            {
                throw new ArgumentNullException("targetMember");
            }
            if (sourceMember == null)
            {
                throw new ArgumentNullException("sourceMember");
            }
            Remove(targetMember);
            _mappers.Add(new DefaultMemberMapper(_container, _options, targetMember, sourceMember, converter));
        }

        public void Set(MappingMember targetMember, MappingMember sourceMember)
        {
            Set(targetMember, sourceMember, null);
        }

        public void Set<TSource, TMember>(MappingMember targetMember, Func<TSource, TMember> expression)
        {
            CheckReadOnly();
            if (targetMember == null)
            {
                throw new ArgumentNullException("targetMember");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            Remove(targetMember);
            _mappers.Add(new LambdaMemberMapper<TSource, TMember>(_container, _options, targetMember, expression));
        }

        public bool Remove(MappingMember targetMember)
        {
            CheckReadOnly();
            if (targetMember == null)
            {
                throw new ArgumentNullException("targetMember");
            }
            return _mappers.RemoveAll(m => m.TargetMember == targetMember) > 0;
        }

        public MemberMapper Get(MappingMember targetMember)
        {
            if (targetMember == null)
            {
                throw new ArgumentNullException("targetMember");
            }
            return _mappers.FirstOrDefault(mapper => mapper.TargetMember == targetMember);
        }
    }
}