using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodeFlow.Core.Annotations;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   A single node ModuleDefinition (akin to a Python ModuleDefinition, or a C# Assembly)
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class ModuleDefinition
  {
    #region Fields / Properties

    /// <summary>
    ///   The core ModuleDefinition that contains things like the build-in types.
    /// </summary>
    public static readonly ModuleDefinition Core = new ModuleDefinition("Core", "NodeFlow.Core");

    /// <summary>
    ///   The name displayed in the node editor UI for this ModuleDefinition.
    /// </summary>
    [JsonProperty] public readonly string DisplayName;

    /// <summary>
    ///   The qualified (full, unique) DisplayName of the ModuleDefinition.
    /// </summary>
    [JsonProperty] public readonly string QualifiedName;

    /// <summary>
    ///   All function (both free and member) node definitions in this ModuleDefinition.
    /// </summary>
    [JsonProperty] public readonly List<NodeDefinition> Functions = new List<NodeDefinition>();

    #endregion

    [JsonConstructor]
    // ReSharper disable once UnusedMember.Local
    private ModuleDefinition()
    {
    }

    private ModuleDefinition(string displayName, string qualifiedName)
    {
      DisplayName = displayName.Humanize(LetterCasing.Title);
      QualifiedName = qualifiedName;
    }

    /// <summary>
    ///   Loads an ModuleDefinition from a C# Assembly and a full namespace path.
    /// </summary>
    public static ModuleDefinition LoadFromAssemblyNamespace(Assembly assembly, string namespc)
    {
      var module = new ModuleDefinition(namespc.Split('.').Last(), namespc);
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
        var nodeDefinition = new NodeDefinition(null, displayName, nFunction.Description,
          method.DeclaringType?.FullName + '.' + method.Name);
        module.Functions.Add(nodeDefinition);
        // Parameters (Input and Output)
        foreach (var parameter in method.GetParameters())
        {
          nodeDefinition.Parameters.Add(new ParameterDefinition(parameter));
        }
      }
      return module;
    }

    /// <summary>
    ///   Loads the module from a json symbols file.
    /// </summary>
    public static ModuleDefinition LoadFromJson(string json)
    {
      var contractResolver = new DefaultContractResolver
      {
        NamingStrategy = new SnakeCaseNamingStrategy()
      };
      return JsonConvert.DeserializeObject<ModuleDefinition>(json, new JsonSerializerSettings
      {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = contractResolver
      });
    }

    /// <summary>
    ///   Serializes the ModuleDefinition to a JSON symbols file.
    /// </summary>
    public string ToJson()
    {
      var contractResolver = new DefaultContractResolver
      {
        NamingStrategy = new SnakeCaseNamingStrategy()
      };
      return JsonConvert.SerializeObject(this, new JsonSerializerSettings
      {
        ContractResolver = contractResolver,
        Formatting = Formatting.Indented
      });
    }
  }
}