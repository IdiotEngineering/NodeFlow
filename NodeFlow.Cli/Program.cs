using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NodeFlow.Core.Annotations;
using NodeFlow.Core.Graph;
using NodeFlow.Core.Nodes;

namespace NodeFlow.Cli
{
  public class Program
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

    //[NFunction("A function with an explicit continuation")]
    //public static void Branch(bool condition, Action isTrue, Action isFalse)
    //{
    //  if (condition) isTrue(); else isFalse();
    //}

    private static void Main(string[] args)
    {
      var module = NModule.LoadFromAssemblyNamespace(Assembly.GetExecutingAssembly(), "NodeFlow.Cli");
      var nGraph = new NGraph();
      // Print
      var printNode = nGraph.MakeNewNode(module.Functions.First(f => f.DisplayName == "Print"));
      var printValue = printNode.NodeDefinition.Parameters.First(p => p.DisplayName == "Value");
      var printIntValue = printNode.NodeDefinition.Parameters.First(p => p.DisplayName == "Int Value");
      var printStringValue = printNode.NodeDefinition.Parameters.First(p => p.DisplayName == "String Value");
      // Begin
      var beginNode = nGraph.MakeNewNode(module.Functions.First(f => f.DisplayName == "Begin"));
      var beginOutValue = beginNode.NodeDefinition.Parameters.First(p => p.DisplayName == "Value");
      var beginStartAction = beginNode.NodeDefinition.Parameters.Find(p => p.DisplayName == "Start");
      var beginTickAction = beginNode.NodeDefinition.Parameters.Find(p => p.DisplayName == "Tick");
      // Begin -> Print
      //beginNode.ImplicitContinuation = printNode;
      beginNode.ParameterBindings.Add(new NParameterBinding(beginStartAction, beginNode, printNode.Guid.ToString()));

      // Bind: Being.value -> Print.value
      var valueBinding = new NParameterBinding(beginOutValue, printValue, beginNode, printNode);
      beginNode.ParameterBindings.Add(valueBinding);
      printNode.ParameterBindings.Add(valueBinding);
      // Print Literals
      printNode.ParameterBindings.Add(new NParameterBinding(printIntValue, printNode, "42"));
      printNode.ParameterBindings.Add(new NParameterBinding(printStringValue, printNode, "\"Hello, world!\""));
      // Code generation
      var codeGenerator = new CSharpCodeGenerator(nGraph);
      var code = codeGenerator.Compile();
      File.WriteAllText("../../Generated.cs", code);
    }
  }
}