using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Reflection;

namespace General.Core.Reflection
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IncludeInHashAttribute : Attribute
    { }

    public static class ExpressionHasher
    {
        private const int StartHashValue = unchecked((int)2166136261);

        private static Dictionary<Type, Func<object, int>> HashCodeFunctions { get; } = new Dictionary<Type, Func<object, int>>();

        public static int GetHashCode(object target)
        {
            var type = target.GetType();
            if (!HashCodeFunctions.TryGetValue(type, out var func))
            {
                func = GetHashCodeFunction(type);
                HashCodeFunctions.Add(type, func);
            }

            return func.Invoke(target);
        }

        private static Func<object, int> GetHashCodeFunction(Type type)
        {
            var includedMembers = type.GetMembers()
                                      .Where(m => m.GetCustomAttribute(typeof(IncludeInHashAttribute)) != null);

            if (!includedMembers.Any())
                throw new InvalidOperationException();

            // Represents the paraemter to the hash code function - the target object, boxed as an object
            var parameterExpression = Expression.Parameter(typeof(object));

            // This is the expression that will give us the hash code
            // We start with a large prime and then mutate it with each member value
            var hashCodeExpression = (Expression)Expression.Constant(StartHashValue, typeof(int));

            // Represents the target object, as unboxed from the parameter to its actual type
            var targetExpression = Expression.Convert(parameterExpression, type);

            // A expression for EqualityComparer<object>.Default, which we'll use when hashing the member values
            // We use the default comparer instead of calling each object's GetHashCode so nulls are appropriately handled
            var defaultComparerExpression = Expression.Constant(EqualityComparer<object>.Default,
                                                                typeof(EqualityComparer<object>));

            // MethodInfo for EqualityComparer<object>.Default.GetHashCode(object), which we'll need to invoke it over and over for each member
            var defaultComparerGetHashCodeMethod = typeof(EqualityComparer<object>).GetMethod(nameof(EqualityComparer<object>.GetHashCode),
                                                                                              new[] { typeof(object) });

            // A constant expression for the prime that we multiply with for each member value
            var hashMultiplyFactorExpression = Expression.Constant(16777619);

            foreach (var member in includedMembers)
            {
                // Get the value of each member and box
                var memberAccessExpression = Expression.PropertyOrField(targetExpression, member.Name);
                var memberBoxExpression = Expression.Convert(memberAccessExpression, typeof(object));

                // Get the hash for the current member from the default comparer
                var memberHashExpression = Expression.Call(defaultComparerExpression,
                                                           defaultComparerGetHashCodeMethod,
                                                           memberBoxExpression);

                // Mutate the hash code based off the hash code for the current member
                // (currentHash * multiplyFactor) ^ memberHash
                hashCodeExpression = Expression.Multiply(hashCodeExpression, hashMultiplyFactorExpression);
                hashCodeExpression = Expression.ExclusiveOr(hashCodeExpression, memberHashExpression);
            }

            return Expression.Lambda<Func<object, int>>(hashCodeExpression, parameterExpression).Compile();
        }
    }

}
