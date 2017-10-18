using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace NodeFlow.Core.Module
{
  /// <summary>
  ///   This class exists for the sole perpose of being included into NodeFlow.Core to ensure this entire assembly
  ///   can be referanced from there.
  /// </summary>
  public static class AssemblyHookType
  {
    public static Assembly GetAssembly() => Assembly.GetAssembly(typeof(AssemblyHookType));
  }
}