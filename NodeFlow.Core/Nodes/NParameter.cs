using Humanizer;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   Represents a single parameter for a node (think: an input Boolean called "IsScookum")
  /// </summary>
  public class NParameter
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

    #endregion

    public NParameter(string symbolName, string displayName, NType type, int position, bool isOptional)
    {
      SymbolName = symbolName;
      DisplayName = displayName.Humanize(LetterCasing.Title);
      Type = type;
      Position = position;
      IsOptional = isOptional;
    }
  }
}