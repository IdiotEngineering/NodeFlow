using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodeFlow.Core.Annotations;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Graph;

namespace NodeFlow.UnitTests
{
  [TestClass]
  public class BasicTest
  {

    [TestMethod]
    public void LoadAssemblyNamespace()
    {
      NModule.LoadFromAssemblyNamespace(Assembly.GetExecutingAssembly(), "NodeFlow.UnitTests");
    }

    [TestMethod]
    public void BasicGraph()
    {
    }
  }
}