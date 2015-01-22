using System;

namespace Ucoin.Framework
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface)]
    public sealed class CompareIgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CompareKeyAttribute : Attribute
    { 
    }
}
