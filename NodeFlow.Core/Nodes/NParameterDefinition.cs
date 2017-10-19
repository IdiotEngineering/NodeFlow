using System;
using System.Reflection;
using Humanizer;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   Represents a single parameter for a node (think: an input Boolean called "IsScookum")
  /// </summary>
  public class NParameterDefinition
  {
    #region Fields / Properties

    /// <summary>
    ///   The name of the actual symbol (the name given to the C# parameter)
    /// </summary>
    public string SymbolName;

    /// <summary>
    ///   The name that will be displayed on the node.
    /// </summary>
    public string DisplayName;

    /// <summary>
    ///   The type of the parameter.
    /// </summary>
    public NType Type;

    /// <summary>
    ///   The position of the parameter.
    /// </summary>
    public int Position;

    /// <summary>
    ///   If the parameter is option (doesn't need to be bound for the NGraph to be valid)
    /// </summary>
    public bool IsOptional;

    /// <summary>
    ///   If the parameter is an out param or not. Out params will be bound as different things for different
    ///   languages. For example: [C#: Thing (out value)] [Python: (value) = Thing()]
    /// </summary>
    public bool IsOut;

    #endregion

    public NParameterDefinition(ParameterInfo parameterInfo)
    {
      Type = NPrimitives.GetNTypeFromSystemType(parameterInfo.ParameterType);
      if (Type == null) throw new Exception("Failed to load unknown primitive type: " + parameterInfo.ParameterType.FullName);
      DisplayName = parameterInfo.Name.Humanize(LetterCasing.Title);
      Position = parameterInfo.Position;
      IsOptional = parameterInfo.IsOptional;
      IsOut = parameterInfo.IsOut;
    }

    public NParameterDefinition(string symbolName, string displayName, NType type, int position, bool isOptional, bool isOut)
    {
      SymbolName = symbolName;
      DisplayName = displayName.Humanize(LetterCasing.Title);
      Type = type;
      Position = position;
      IsOptional = isOptional;
      IsOut = isOut;
    }
  }
}