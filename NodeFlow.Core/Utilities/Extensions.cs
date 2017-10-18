using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace NodeFlow.Core.Utilities
{
  public static class Extensions
  {
    #region Dictionary Helpers

    /// <summary>
    /// Gets a value, or returns the default(TValue) which is null for referance types, 0 for numeric.
    /// </summary>
    public static TValue GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
      TValue outVal;
      return dictionary.TryGetValue(key, out outVal) ? outVal : default(TValue);
    }

    public static TValue GetValueOrAddLambda<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
      Func<TValue> generator)
    {
      TValue outVal;
      if (dictionary.TryGetValue(key, out outVal)) return outVal;
      outVal = generator();
      dictionary.Add(key, outVal);
      return outVal;
    }

    #endregion
  }
}