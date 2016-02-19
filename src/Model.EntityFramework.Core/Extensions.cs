using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Pally.Model.EntityFramework.Core.Helpers;

namespace Pally.Model.EntityFramework.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Includes entity or entity set for eager loding with possibility of setting condition.
        /// DOES NOT WORK FOR M:N RELATIONS!!!
        /// Take from http://blog.cincura.net/232741-include-with-filtering-or-limiting/
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> IncludeWithCondition<TEntity, TOther>(this IQueryable<TEntity> source, 
            Expression<Func<TEntity, IEnumerable<TOther>>> path) where TEntity : class 
        {
            TEntity main = default(TEntity);
            var dummy = new { Main = main, Included = Enumerable.Empty<TOther>() };
            Expression<Func<TEntity, object>> exampleExpression = x => new
            {
                Main = x,
                Included = Enumerable.Empty<TOther>()
            };

            PropertyInfo mainProperty = dummy.GetType().GetProperty("Main");
            ParameterExpression accessParam = Expression.Parameter(dummy.GetType(), "x");
            Delegate mainBack = Expression.Lambda(Expression.MakeMemberAccess(accessParam, mainProperty), accessParam).Compile();
            NewExpression body = (NewExpression)exampleExpression.Body;
            Type anonymousType = body.Type;
            var parametersMap = exampleExpression.Parameters.Select((f, i) => new { f, s = path.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            Expression reboundBody = ParameterRebinder.ReplaceParameters(parametersMap, path.Body);
            NewExpression newExpression = body.Update(new[] { body.Arguments[0], reboundBody });
            LambdaExpression finalLambda = Expression.Lambda(newExpression, exampleExpression.Parameters);

            MethodInfo miA = typeof(Queryable).GetMethods()
                .Where(m => m.Name == "Select").First(m => m.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments().Length == 2);
            MethodInfo miB = typeof(Enumerable).GetMethods()
                .Where(m => m.Name == "Select").First(m => m.GetParameters()[1].ParameterType.GetGenericArguments().Length == 2);

            MethodInfo selectAMethod = miA.MakeGenericMethod(typeof(TEntity), anonymousType);
            var dataA = selectAMethod.Invoke(null, new object[] { source, finalLambda });
            MethodInfo selectBMethod = miB.MakeGenericMethod(anonymousType, typeof(TEntity));
            var dataB = selectBMethod.Invoke(null, new[] { dataA, mainBack });
            return ((IEnumerable<TEntity>)dataB).AsQueryable();
        } 
    }
}
