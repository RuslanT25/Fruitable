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
    internal class CacheAspect : MethodInterception
    {   
        private int _duration;
        private ICacheManager _cacheManager;
        public CacheAspect(ICacheManager cacheManager, int duration = 60)
        {
            _cacheManager = cacheManager;
            _duration = duration;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}";
            var arguments = invocation.Arguments.Select(arg => arg != null ? arg.ToString() : "<Null>").ToArray();
            var key = $"{methodName}({string.Join(", ", arguments)})";

            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get<object>(key);
                return;
            }
        }

        protected override void OnAfter(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}";
            var arguments = invocation.Arguments.Select(arg => arg != null ? arg.ToString() : "<Null>").ToArray();
            var key = $"{methodName}({string.Join(", ", arguments)})";

            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
