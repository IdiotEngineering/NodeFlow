using System;
using NodeFlow.Core.Annotations;

namespace NodeFlow.Core.Module
{
  /// <summary>
  ///   This class exists for the sole perpose of being included into NodeFlow.Core to ensure this entire assembly
  ///   can be referanced from there.
  /// </summary>
  public static class Samples
  {
    [NFunction("Prints the value to stdout.")]
    public static void Print(string value, int intValue, string stringValue)
    {
      Console.WriteLine(value + intValue + stringValue);
    }

    [NFunction("The entry point into the graph.")]
    public static void Begin(Action start, Action tick, out string value)
    {
      value = "Hello, world!! Woohooo.";
    }
  }
}