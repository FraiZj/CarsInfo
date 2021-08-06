using System;
using System.Linq.Expressions;
using System.Reflection;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Assistance
{
    public static class ExpressionExtensions
    {
        public static MemberInfo GetReturnType<T>(this Expression<Func<T, object>> expression)
            where T : BaseEntity
        {
            var memberExpression = (MemberExpression)expression.Body;
            var type = (memberExpression.Member as PropertyInfo)?.PropertyType;

            return ReflectionUtility.IsCollection(type) ? 
                ReflectionUtility.GetCollectionElementType(type) : 
                memberExpression.Member;
        }
    }
}
