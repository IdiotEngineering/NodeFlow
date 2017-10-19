using System;
using System.Reflection;
using Humanizer;
using Newtonsoft.Json;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   Represents a single parameter for a node (think: an input Boolean called "IsScookum")
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class ParameterDefinition
  {
    #region Fields / Properties

    /// <summary>
    ///   The name that will be displayed on the node.
    /// </summary>
    [JsonProperty] public string DisplayName;

    /// <summary>
    ///   The type of the parameter.
    /// </summary>
    [JsonProperty] public NType Type;

    /// <summary>
    ///   The position of the parameter.
    /// </summary>
    [JsonProperty] public int Position;

    /// <summary>
    ///   If the parameter is option (doesn't need to be bound for the Graph to be valid)
    /// </summary>
    [JsonProperty] public bool IsOptional;

    /// <summary>
    ///   If the parameter is an out param or not. Out params will be bound as different things for different
    ///   languages. For example: [C#: Thing (out value)] [Python: (value) = Thing()]
    /// </summary>
    [JsonProperty] public bool IsOut;

    #endregion

    public ParameterDefinition(ParameterInfo parameterInfo)
    {
      Type = Primitives.GetNTypeFromSystemType(parameterInfo.ParameterType);
      if (Type == null)
        throw new Exception("Failed to load unknown primitive type: " + parameterInfo.ParameterType.FullName);
      DisplayName = parameterInfo.Name.Humanize(LetterCasing.Title);
      Position = parameterInfo.Position;
      IsOptional = parameterInfo.IsOptional;
      IsOut = parameterInfo.IsOut;
    }

    [JsonConstructor]
    // ReSharper disable once UnusedMember.Local
    private ParameterDefinition()
    {
    }
  }
}