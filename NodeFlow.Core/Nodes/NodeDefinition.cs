using System.Collections.Generic;
using Humanizer;
using Newtonsoft.Json;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   Defines the call order of a node.
  /// </summary>
  public enum NCallSequencing
  {
    /// <summary>
    ///   The node is called explcitily (there is a control flow edges feeding into the node).
    /// </summary>
    Explicit,

    /// <summary>
    ///   The node is called implcitly (used for properties).
    /// </summary>
    Implicit
  }

  /// <summary>
  ///   The definition of a single node. Note that despite the fact that an Node is subclassed into
  ///   things like a NCallableNode, the definition (just like NType) is not subclassed.
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class NodeDefinition
  {
    #region Fields / Properties

    /// <summary>
    ///   The scope that this node (function) can be run under. If set to NNull, then this is a global
    ///   node that can be run anywhere (like built-in functions) assuming one of it's input types
    ///   matches the binding type.
    /// </summary>
    [JsonProperty] public NType Scope;

    /// <summary>
    ///   The human-readable name to display in the node's title (normally just the humanized function name)
    /// </summary>
    [JsonProperty] public string DisplayName;

    /// <summary>
    ///   The human-readable description for what this node does.
    /// </summary>
    [JsonProperty] public string Description;

    /// <summary>
    ///   The name of the class/function this node is bound to.
    /// </summary>
    [JsonProperty] public string SymbolName;

    /// <summary>
    ///   The input and return data types of this node, 0 or more.
    /// </summary>
    [JsonProperty] public List<ParameterDefinition> Parameters = new List<ParameterDefinition>();

    /// <summary>
    ///   Sets the type of call (input control flow) sequencing for this node. Explicit for things that are
    ///   called, Implcicit for things that need to be called when their value is fetched (properties).
    /// </summary>
    [JsonProperty] public NCallSequencing CallSequencing = NCallSequencing.Explicit;

    #endregion

    internal NodeDefinition(NType scope, string displayName, string description, string symbolName)
    {
      Scope = scope;
      DisplayName = displayName.Humanize(LetterCasing.Title);
      Description = description;
      SymbolName = symbolName;
    }

    [JsonConstructor]
    // ReSharper disable once UnusedMember.Local
    private NodeDefinition()
    {
    }
  }
}