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
    [NFunction("Prints 'Hello, world!' to stdout.")]
    public static void PrintHelloWorld()
    {
      Console.WriteLine("Hello, world!");
    }

    [NFunction("Prints the value to stdout.")]
    public static void Print(string value)
    {
      Console.WriteLine(value);
    }

    [NFunction("The entry point into the graph.")]
    public static void Begin(out string value)
    {
      value = "Hello, world!! Woohooo.";
    }

    private static void Main(string[] args)
    {
      var module = NModule.LoadFromAssemblyNamespace(Assembly.GetExecutingAssembly(), "NodeFlow.Cli");
      var nGraph = new NGraph();
      var print = nGraph.MakeNewNode(module.Functions.First(f => f.DisplayName == "Print"));
      var begin = nGraph.MakeNewNode(module.Functions.First(f => f.DisplayName == "Begin"));
      // Begin -> Print
      begin.ContinuationBindings.Add(NControlFlowParameter.Implicit, print);
      print.PossibleControlFlowInputs.Add(begin);
      // Being.value -> Print.value
      var printInParam = print.NodeDefinition.InputParameters.First(p => p.DisplayName == "Value");
      var beginOutParam = begin.NodeDefinition.ReturnParameters.First(p => p.DisplayName == "Value");
      var binding = new NParameterBinding(beginOutParam, printInParam, begin, print);
      print.InputParameterBindings.Add(binding);
      begin.ReturnParameterBindings.Add(binding);
      // Code generation
      var codeGenerator = new CSharpCodeGenerator(nGraph);
      var code = codeGenerator.Compile();
      File.WriteAllText("../../Generated.cs", code);
    }
  }
}