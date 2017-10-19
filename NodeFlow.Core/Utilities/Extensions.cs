using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NodeFlow.Core.Utilities
{
  public static class Extensions
  {
    #region Fields / Properties

    private static readonly char[] Chars =
      "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".Select(c => c).ToArray();

    #endregion

    #region String Helpers

    public static string ToSymbolSafeGuid(this Guid guid) => "SYM_" + guid.ToString().Replace("-", "");

    public static string RemoveEmptyLines(this string str)
      => Regex.Replace(str, @"^\s*$[\r\n]*", "", RegexOptions.Multiline);

    public static string Indent(this string str, int amount)
    {
      if (amount < 0) throw new ArgumentException("amount cannot be negative.");
      var output = new StringBuilder();
      using (var reader = new StringReader(str))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          output.AppendLine(new string(' ', amount) + line);
        }
      }
      return output.ToString();
    }

    public static string ToSymbolString(this int value)
    {
      var result = string.Empty;
      var targetBase = Chars.Length;
      do
      {
        result = Chars[value%targetBase] + result;
        value = value/targetBase;
      } while (value > 0);
      return result;
    }

    #endregion

    #region Dictionary Helpers

    /// <summary>
    ///   Gets a value, or returns the default(TValue) which is null for referance types, 0 for numeric.
    /// </summary>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
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