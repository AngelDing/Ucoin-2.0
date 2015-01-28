using System;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal abstract class FastActivatorBase
    {
        protected readonly ConstructorInfo[] Constructors;

        protected FastActivatorBase(Type type)
        {
            ObjectType = type;
            Constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        protected Type ObjectType { get; set; }
    }
}