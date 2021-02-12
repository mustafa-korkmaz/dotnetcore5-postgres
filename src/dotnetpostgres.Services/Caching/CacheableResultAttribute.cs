﻿using System;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace dotnetpostgres.Services.Caching
{
    /// <summary>
    /// Aspect oriented caching service attribute.
    /// Default provider is LocalMemoryCacheService.
    /// </summary>
    [PSerializable]
    public class CacheableResultAttribute : MethodInterceptionAspect
    {
        public int ExpireInMinutes { get; set; }

        /// <summary>
        /// specific key for caching. if it is not passed an auto-key will be generated
        /// </summary>
        public string CacheKey { get; set; }

        private string _autoGeneratedCacheKey;

        public string Provider { get; set; } = "InMemoryCacheService";

        /// <summary>
        /// on cacheable method invoked.
        /// </summary>
        /// <param name="args"></param>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var cachePrviderType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(p => p.Name == Provider);

            if (cachePrviderType == null)
            {
                throw new ApplicationException($"{Provider} CacheProviderNotFound");
            }

            var cache = cachePrviderType.GetProperty("Instance").GetValue(null) as ICacheService;

            //if there is a given cache keyword, use it else generate a new one.
            if (string.IsNullOrEmpty(CacheKey))
            {
                // get method args to have method result specific value

                var arguments = args.Arguments.ToList();
                _autoGeneratedCacheKey = Utility.GetMethodResultCacheKey(args.Method, arguments);
            }
            else
            {
                _autoGeneratedCacheKey = CacheKey;
            }

            var mi = args.Method as MethodInfo;

            var type = mi.ReturnType;

            MethodInfo method = cache.GetType().GetMethod("Get")
                             .MakeGenericMethod(new Type[] { type });

            var result = method.Invoke(cache, new object[] { _autoGeneratedCacheKey });
            if (result != null)
            {
                args.ReturnValue = result;
                return;
            }

            base.OnInvoke(args);

            cache.Add(_autoGeneratedCacheKey, args.ReturnValue, ExpireInMinutes);
        }
    }
}
