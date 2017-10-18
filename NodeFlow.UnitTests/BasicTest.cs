using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodeFlow.Core.Annotations;
using NodeFlow.Core.Nodes;

namespace NodeFlow.UnitTests
{
  [TestClass]
  public class BasicTest
  {
    [NFunction("Prints 'Hello, world!' to stdout.")]
    public static void PrintHelloWorld()
    {
      Console.WriteLine("Hello, world!");
    }

    [TestMethod]
    public void LoadAssemblyNamespace()
    {
      var module = NModule.LoadFromAssemblyNamespace(Assembly.GetExecutingAssembly(), "NodeFlow.UnitTests");
    }

    public void BasicGraph()
    {
      var module = NModule.LoadFromAssemblyNamespace(Assembly.GetExecutingAssembly(), "NodeFlow.UnitTests");
    }
  }
}