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
  ///   A single node module (akin to a Python Module, or a C# Assembly)
  /// </summary>
  public class NModule
  {
    #region Fields / Properties

    /// <summary>
    ///   The core module that contains things like the build-in types.
    /// </summary>
    public static readonly NModule NCore = new NModule("Core", "NodeFlow.Core");

    /// <summary>
    ///   The name displayed in the node editor UI for this module.
    /// </summary>
    public readonly string DisplayName;

    /// <summary>
    ///   The qualified (full, unique) DisplayName of the module.
    /// </summary>
    public readonly string QualifiedName;

    /// <summary>
    ///   All types that are a member of this module (just like Assembly.Types)
    /// </summary>
    public readonly Dictionary<string, NType> TypesByQualifiedNames = new Dictionary<string, NType>();

    /// <summary>
    /// All function (both free and member) node definitions in this module.
    /// </summary>
    public readonly List<NNodeDefinition> Functions = new List<NNodeDefinition>(); 

    #endregion

    private NModule(string displayName, string qualifiedName)
    {
      DisplayName = displayName.Humanize(LetterCasing.Title);
      QualifiedName = qualifiedName;
    }

    /// <summary>
    /// Loads an NModule from a C# Assembly and a full namespace path.
    /// </summary>
    public static NModule LoadFromAssemblyNamespace(Assembly assembly, string namespc)
    {
      var module = new NModule(namespc.Split('.').Last(), namespc);
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
        var nodeDefinition = new NNodeDefinition(null, module, displayName, nFunction.Description, method.Name);
        module.Functions.Add(nodeDefinition);
        // Parameters (Input and Output)
        foreach (var parameter in method.GetParameters())
        {
          var nType = NPrimitives.CSharpToNTypeMap.GetValueOrNull(parameter.ParameterType);
          if (nType == null)
            throw new Exception("Failed to load unknown primitive type: " + parameter.ParameterType.FullName);
          (parameter.IsOut ? nodeDefinition.ReturnParameters : nodeDefinition.InputParameters).Add(
            new NParameter(parameter.Name, parameter.Name.Humanize(LetterCasing.Title), nType, parameter.Position, parameter.IsOptional));
        }
      }
      return module;
    }
  }
}