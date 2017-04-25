using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DAL_Database.Dapper.Helpers
{
    public static class QueryExtenstions
    {
        public static IEnumerable<TParent> QueryParentChild<TParent, TChild, TParentKey>(this IDbConnection connection,
            string sql, Func<TParent, TParentKey> parentKeySelector, Func<TParent, IList<TChild>> childSelector,
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            Dictionary<TParentKey, TParent> cache = new Dictionary<TParentKey, TParent>();
            connection.Query<TParent, TChild, TParent>(sql, (parent, child) => JoinQuery(parent, child, parentKeySelector, childSelector, cache), param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }

        public static async Task<IEnumerable<TParent>> QueryParentChildAsync<TParent, TChild, TParentKey>(this IDbConnection connection,
            string sql, Func<TParent, TParentKey> parentKeySelector, Func<TParent, IList<TChild>> childSelector,
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            Dictionary<TParentKey, TParent> cache = new Dictionary<TParentKey, TParent>();
            await connection.QueryAsync<TParent, TChild, TParent>(sql, (parent, child) => JoinQuery(parent, child, parentKeySelector, childSelector, cache), param as object, transaction, buffered, splitOn, commandTimeout, commandType);
            return cache.Values;
        }

        private static TParent JoinQuery<TParent, TChild, TParentKey>(TParent parent, TChild child, Func<TParent, TParentKey> parentKeySelector, Func<TParent, IList<TChild>> childSelector, IDictionary<TParentKey, TParent> cache)
        {
            // Check if parent already exsist in cache
            if (!cache.ContainsKey(parentKeySelector(parent)))
            {
                cache.Add(parentKeySelector(parent), parent);
            }

            // Retrieve parent
            TParent cachedParent = cache[parentKeySelector(parent)];

            // Add all the children to the parent.
            IList<TChild> children = childSelector(cachedParent);
            children.Add(child);

            return cachedParent;
        }
    }
}
