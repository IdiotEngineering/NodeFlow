using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Humanizer;
using NodeFlow.Core.Annotations;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   A single node ModuleDefinition (akin to a Python ModuleDefinition, or a C# Assembly)
  /// </summary>
  public class NModuleDefinition
  {
    #region Fields / Properties

    /// <summary>
    ///   The core ModuleDefinition that contains things like the build-in types.
    /// </summary>
    public static readonly NModuleDefinition NCore = new NModuleDefinition("Core", "NodeFlow.Core");

    /// <summary>
    ///   The name displayed in the node editor UI for this ModuleDefinition.
    /// </summary>
    public readonly string DisplayName;

    /// <summary>
    ///   The qualified (full, unique) DisplayName of the ModuleDefinition.
    /// </summary>
    public readonly string QualifiedName;

    /// <summary>
    ///   All types that are a member of this ModuleDefinition (just like Assembly.Types)
    /// </summary>
    public readonly Dictionary<string, NType> TypesByQualifiedNames = new Dictionary<string, NType>();

    /// <summary>
    /// All function (both free and member) node definitions in this ModuleDefinition.
    /// </summary>
    public readonly List<NNodeDefinition> Functions = new List<NNodeDefinition>(); 

    #endregion

    private NModuleDefinition(string displayName, string qualifiedName)
    {
      DisplayName = displayName.Humanize(LetterCasing.Title);
      QualifiedName = qualifiedName;
    }

    /// <summary>
    /// Loads an NModuleDefinition from a C# Assembly and a full namespace path.
    /// </summary>
    public static NModuleDefinition LoadFromAssemblyNamespace(Assembly assembly, string namespc)
    {
      var module = new NModuleDefinition(namespc.Split('.').Last(), namespc);
      // TODO: Could do with a less inificient way of doing this
      var types = assembly.GetTypes().Where(type => type.Namespace == namespc);
      var statics = types.SelectMany(
        type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
        .Where(method => method.GetCustomAttributes(typeof (NFunction), false).Length > 0);
      foreach (var method in statics)
      {
        if (method.ReturnType != typeof (void))
          throw new Exception("An NFunction cannot return a value, use out parameters instead: " + method.Name);
        var nFunction = method.GetCustomAttribute<NFunction>();
        var displayName = nFunction.DisplayName ?? method.Name.Humanize(LetterCasing.Title);
        // Create the node definition
        var nodeDefinition = new NNodeDefinition(null, module, displayName, nFunction.Description, method.DeclaringType?.FullName + '.' + method.Name);
        module.Functions.Add(nodeDefinition);
        // Parameters (Input and Output)
        foreach (var parameter in method.GetParameters())
        {
          nodeDefinition.Parameters.Add(new NParameterDefinition(parameter));
        }
      }
      return module;
    }
  }
}