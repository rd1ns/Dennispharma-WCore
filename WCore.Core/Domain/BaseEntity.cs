using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WCore.Core.Caching;

namespace WCore.Core.Domain
{
    public class BaseEntity 
    {
        [ForeignKey("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Get key for caching the entity
        /// </summary>
        public string EntityCacheKey => GetEntityCacheKey(GetType(), Id);

        /// <summary>
        /// Get key for caching the entity
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="id">Entity id</param>
        /// <returns>Key for caching the entity</returns>
        public static string GetEntityCacheKey(Type entityType, object id)
        {
            return string.Format(WCoreCachingDefaults.WCoreEntityCacheKey, entityType.Name.ToLower(), id);
        }
    }
}