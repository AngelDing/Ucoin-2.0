using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucoin.Framework.Reflection;
using Xunit;

namespace Ucoin.Framework.Test
{
    public class ReflectionHelperTest
    {
        [Fact]
        public virtual void WhenExtractingNameFromAValidPropertyExpression_ThenPropertyNameReturned()
        {
            var propertyName = ReflectionHelper.ExtractPropertyName(() => this.InstanceProperty);
            Assert.Equal("InstanceProperty", propertyName);
        }

        [Fact]
        public void WhenExpressionRepresentsAStaticProperty_ThenExceptionThrown()
        {
            ExceptionAssert.Throws<ArgumentException>(() => ReflectionHelper.ExtractPropertyName(() => StaticProperty));
        }

        [Fact]
        public void WhenExpressionIsNull_ThenAnExceptionIsThrown()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => ReflectionHelper.ExtractPropertyName<int>(null));
        }

        [Fact]
        public void WhenExpressionRepresentsANonMemberAccessExpression_ThenAnExceptionIsThrown()
        {
            ExceptionAssert.Throws<ArgumentException>(
                () => ReflectionHelper.ExtractPropertyName(() => this.GetHashCode())
                );

        }

        [Fact]
        public void WhenExpressionRepresentsANonPropertyMemberAccessExpression_ThenAnExceptionIsThrown()
        {
            ExceptionAssert.Throws<ArgumentException>(() => ReflectionHelper.ExtractPropertyName(() => this.InstanceField));
        }

        public static int StaticProperty { get; set; }
        public int InstanceProperty { get; set; }
        public int InstanceField;
        public static int SetOnlyStaticProperty { set { } }
    }

    public static class ExceptionAssert
    {
        public static void Throws<TException>(Action action)
            where TException : Exception
        {
            ExceptionAssert.Throws(typeof(TException), action);
        }

        public static void Throws(Type expectedExceptionType, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.IsType(expectedExceptionType, ex);
                return;
            }

            Assert.True(false, string.Format("No exception thrown.  Expected exception type of {0}.", expectedExceptionType.Name));
        }
    }
}
