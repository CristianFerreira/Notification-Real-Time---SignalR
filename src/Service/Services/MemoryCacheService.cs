using Domain.AbstractionModels;
using Domain.RequestModels;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Service.Services
{
    public static class MemoryCacheService
    {
        private readonly static MemoryCache _memoryCache = new MemoryCache("NotificationCache");

        public static void Add(ClientConnectedRequest connected)
        {
            var cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            _memoryCache.Set(connected.ConnectionId, 
                             new CacheValueAbstraction(connected.UserId, connected.ApplicationId), 
                             cacheItemPolicy);
        }       

        public static IList<string> GetConnectionId(IList<string> usersId, int applicationId)
        {
            var list = new List<string>();
            foreach (var userId in usersId)
            {
                foreach (var item in _memoryCache.ToList())
                {
                    var value = item.Value as CacheValueAbstraction;
                    if (value.UserId.Equals(userId) && value.ApplicationId.Equals(applicationId))
                        list.Add(item.Key);
                }
            }
            return list;
        }

        public static void Remove(string key)
            => _memoryCache.Remove(key);

        public static bool ExistUserId(string key, string userId)
         => _memoryCache.Any(x=>x.Value.ToString() == userId);

        public static bool ExistKey(string key)
            => (List<ClientConnectedRequest>)_memoryCache.Get(key) != null;
     
        
    }
}
