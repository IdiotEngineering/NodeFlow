using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NodeFlow.Core.Annotations;
using NodeFlow.Core.Graph;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Runtime;

namespace NodeFlow.Cli
{
  public class Program
  {
    private static void Main(string[] args)
    {
      var module = NModuleDefinition.LoadFromAssemblyNamespace(Assembly.LoadFrom("NodeFlow.Core.Module.dll"), "NodeFlow.Core.Module");
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
      // Module loading
      var moduleRegistry = new NModuleRegistry();
      moduleRegistry.AddModule(code);
    }
  }
}