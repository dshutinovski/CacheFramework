﻿using System;
using System.Linq;
using System.Security.Principal;
using PostSharp.Aspects;

namespace EasyCaching.Caching
{
  [Serializable]
  class CacheableResultAttribute : MethodInterceptionAspect
  {
    public override void OnInvoke(MethodInterceptionArgs args)
    {
      var cache = MethodResultCache.GetCache(args.Method);
      var arguments = args.Arguments.Union(new[] {WindowsIdentity.GetCurrent().Name}).ToList();
      var result = cache.GetCachedResult(arguments);
      if (result != null)
      {
        args.ReturnValue = result;
        return;
      }

      base.OnInvoke(args);

      cache.CacheCallResult(args.ReturnValue, arguments);
    }
  }
}
