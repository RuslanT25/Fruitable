using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Helpers.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Caching
{
    internal class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheManager _cacheManager;

        public CacheRemoveAspect(ICacheManager cacheManager, string pattern)
        {
            _pattern = pattern;
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
