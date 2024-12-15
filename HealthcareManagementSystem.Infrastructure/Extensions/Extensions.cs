using Castle.Core.Internal;
using HealthcareManagementSystem.Domain.Filters;
using HealthcareManagementSystem.Infrastructure.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.Extensions
{
    public static class Extensions
    {
        public static IQueryable<T> OrderBy<T>(
            this IQueryable<T> queryable,
            Type resourceType,
            string orderBy)
        {
            if (orderBy == null)
                return queryable;

            //split the orderby string by comma
            //key is the column name without the + or -
            // value is + or - (+ for ascending and - for descending)
            var dictionary = orderBy.Split(',')
                .ToDictionary(item => item.Replace(new[]
                {
                    "+",
                    "-"
                }, ""), item => item.Left(1));

            //IOrderedQuerable<T> to order by more than one column using ThenBy and ThenByDescending
            IOrderedQueryable<T> orderedResult = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "e");
            //for each column name in the resouce, get the column name in the entity (what we specify in the IncludeInOrderByAttribute)
            foreach (var pair in dictionary)
            {
                var exp = CreateOrderbyExpression(resourceType, param, pair);

                //order by descending if - otherwise ascending
                if (orderedResult == null)
                {
                    orderedResult = pair.Value == "-"
                        ? queryable.OrderByDescending(Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param))
                        : queryable.OrderBy(Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param));
                }
                else
                {
                    orderedResult = pair.Value == "-"
                        ? orderedResult.ThenByDescending(Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param))
                        : orderedResult.ThenBy(Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param));
                }
            }

            return orderedResult;
        }

        private static Expression CreateOrderbyExpression(Type resourceType, ParameterExpression param, KeyValuePair<string, string> pair)
        {
            Expression exp;

            if (pair.Key.Contains('.'))
            {
                exp = param;
                foreach (var item in pair.Key.Split('.'))
                    exp = Expression.PropertyOrField(exp, item);
            }
            else
            {
                var entityPropertyName = resourceType.GetPropertiesWithAttribute<IncludeInOrderByAttribute>()
                    .First(p => string.Equals(p.Property.Name, pair.Key, StringComparison.CurrentCultureIgnoreCase))
                    .Attribute.EntityPropertyName;
                exp = Expression.PropertyOrField(param, entityPropertyName);
            }

            return exp;
        }

        public static IQueryable<TEntity> ApplyFilters<TEntity>(
            this IQueryable<TEntity> entities,
            IEnumerable<Expression<Func<TEntity, bool>>> filters)
        {
            return filters == null ? entities : filters.Aggregate(entities, (current, filter) => current.Where(filter));
        }

        public static IQueryable<TEntity> ApplyFilter<TEntity>(
            this IQueryable<TEntity> entities,
            Expression<Func<TEntity, bool>> filter)
        {
            if (filter != null)
            {
                entities = entities.Where(filter);
            }
            return entities;
        }

        public static IQueryable<TEntity> ApplyIncludes<TEntity>(
            this IQueryable<TEntity> entities,
            IEnumerable<Expression<Func<TEntity, object>>> includes)
            where TEntity : class, new()
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    entities = entities.Include(include);
                }
            }
            return entities;
        }

        public static IQueryable<TEntity> ApplyIncludes<TEntity>(
            this IQueryable<TEntity> entities,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
            where TEntity : class, new()
        {
            if (includes != null)
            {
                entities = includes.Invoke(entities);
            }
            return entities;
        }

        public static IQueryable<TEntity> ApplyPaging<TEntity>(
            this IQueryable<TEntity> entities,
            ListFilter pagingFilter)
        {
            if (pagingFilter != null)
            {
                entities = entities.Skip(pagingFilter.Skip ?? 0)
                    .Take(pagingFilter.Take ?? 999);
            }
            return entities;
        }

        public static IQueryable<TEntity> ApplySorting<TEntity, TResource>(
            this IQueryable<TEntity> entities,
            ListFilter pagingFilter)
        {
            return string.IsNullOrEmpty(pagingFilter?.OrderBy)
                ? entities
                : entities.OrderBy(typeof(TResource), pagingFilter.OrderBy);
        }



        /// <summary>
        /// Get's all the properties with a certain attribute present, and returns in some Tuple
        /// </summary>
        public static IEnumerable<PropertyAttribute<TAttribute>> GetPropertiesWithAttribute<TAttribute>(this Type member) where TAttribute : Attribute
        {
            return member
                .GetTypeInfo()
                .GetProperties()
                .Select(p => new PropertyAttribute<TAttribute>
                {
                    Attribute = p.GetAttribute<TAttribute>(),
                    Property = p
                })
                .Where(a => a.Attribute != null);
        }

        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            oldValues.Each(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }

        public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            foreach (T obj in instance)
                action(obj);
        }

        public static string Left(this string value, int length)
        {
            return value?.Substring(0, Math.Min(length, value.Length));
        }

        public static string Right(this string value, int length)
        {
            if (value == null)
                return (string)null;
            if (value.Length == 0)
                return string.Empty;
            return value.Substring(Math.Max(0, value.Length - length));
        }

        public static object GetPropValue(this object obj, string name)
        {
            string str = name;
            char[] chArray = new char[1] { '.' };
            foreach (string name1 in str.Split(chArray))
            {
                if (obj == null)
                    return (object)null;
                PropertyInfo property = obj.GetType().GetTypeInfo().GetProperty(name1);
                if ((object)property == null)
                    return (object)null;
                obj = property.GetValue(obj, (object[])null);
            }
            return obj;
        }

        //public static async Task RunTransaction<T, I>(this IRepository<T, I> repo, Func<Task> transaction)
        //    where T : class,  new()
        //{
        //    using (await repo.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            await transaction();
        //            repo.CommitTransaction();
        //        }
        //        catch
        //        {
        //            repo.RollBackTransaction();
        //            throw;
        //        }
        //    }
        //}



        public static IEnumerable<T> Distinct<T, TProperty>(this IEnumerable<T> e, Func<T, TProperty> getter)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            return e.Distinct(new PropertyEqualityComparer<T, TProperty>(getter));
        }
    }

    public class PropertyAttribute<TAttribute>
    {
        public TAttribute Attribute { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
