using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;

namespace NodeFlow.Core.Runtime
{
  public class NModuleRegistry
  {
    public void AddModule(string sourceCode)
    {
      var provider = new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", "v4.0"}});
      var compilerParams = new CompilerParameters
      {
        GenerateInMemory = true,
        GenerateExecutable = false
      };
      compilerParams.ReferencedAssemblies.Add("NodeFlow.Core.dll");
      compilerParams.ReferencedAssemblies.Add("NodeFlow.Core.Module.dll");
      var results = provider.CompileAssemblyFromSource(compilerParams, sourceCode);
      if (results.Errors.Count != 0)
        throw new Exception("Mission failed!");
      var assembly = results.CompiledAssembly;
      var modules =
        assembly.GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof (GeneratedGraph))).ToArray();
      var sample = modules[0];
      var instance = (GeneratedGraph) Activator.CreateInstance(sample);
    }
  }
}