using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Graph
{
  /// <summary>
  ///   A node instance in an Graph. NodeDefinition is the type, this is the instance.
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class Node
  {
    #region Fields / Properties

    /// <summary>
    ///   The GUID for this node, assigned from the Graph. This is used to referance this node
    ///   while generating code and for things like serialization.
    /// </summary>
    [JsonProperty] public ShortGuid Guid;

    /// <summary>
    ///   The definition of the node (this is akin to it's "type").
    /// </summary>
    public NodeDefinition NodeDefinition;

    /// <summary>
    ///   Bound nput and return parameters. Optional parameters don't need to be bound.
    /// </summary>
    [JsonProperty] public List<ParameterBinding> ParameterBindings = new List<ParameterBinding>();

    /// <summary>
    ///   The implicit continuation after this node (default control flow ancor is bound). This can
    ///   be null for two reasons: the node's output control flow ancor isn't bound to anything, or
    ///   this is a node with explicit continuations (like a branch node).
    /// </summary>
    [JsonProperty] public Node ImplicitContinuation;

    #endregion

    public string GenerateCode()
    {
      // Support return types
      return string.Format("private void {0}() {{ {1}(); }}", Guid, NodeDefinition.SymbolName);
    }

    public IEnumerable<ParameterDefinition> GetUnboundExplicitContinuationParameters() =>
      // Return all parameters that are actions and don't apear in the ParameterBinding list.
      NodeDefinition.Parameters.Where(
        p => p.Type == Primitives.NAction && ParameterBindings.All(pb => p != pb.SourceParameterDefinition));
  }
}