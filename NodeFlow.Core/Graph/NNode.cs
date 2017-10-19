using System.Collections.Generic;
using System.Linq;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Graph
{
  /// <summary>
  ///   A node instance in an NGraph. NodeDefinition is the type, this is the instance.
  /// </summary>
  public class NNode
  {
    #region Fields / Properties

    /// <summary>
    ///   The GUID for this node, assigned from the NGraph. This is used to referance this node
    ///   while generating code and for things like serialization.
    /// </summary>
    public ShortGuid Guid;

    /// <summary>
    ///   The definition of the node (this is akin to it's "type").
    /// </summary>
    public NNodeDefinition NodeDefinition;

    /// <summary>
    ///   What object node owns this function call. This is only set if this node is a
    ///   member function, otherwise owner is just null (global scope function). This is
    ///   the instance of the NodeDefinition.Scope type.
    /// </summary>
    public NNode Owner;

    /// <summary>
    ///   Bound nput and return parameters. Optional parameters don't need to be bound.
    /// </summary>
    public List<NParameterBinding> ParameterBindings = new List<NParameterBinding>();

    /// <summary>
    ///   The implicit continuation after this node (default control flow ancor is bound). This can
    ///   be null for two reasons: the node's output control flow ancor isn't bound to anything, or
    ///   this is a node with explicit continuations (like a branch node).
    /// </summary>
    public NNode ImplicitContinuation;

    #endregion

    public string GenerateCode()
    {
      // Support return types
      return string.Format("private void {0}() {{ {1}(); }}", Guid, NodeDefinition.SymbolName);
    }

    public IEnumerable<NParameterDefinition> GetUnboundExplicitContinuationParameters() =>
      // Return all parameters that are actions and don't apear in the ParameterBinding list.
      NodeDefinition.Parameters.Where(
        p => p.Type == NPrimitives.NAction && ParameterBindings.All(pb => p != pb.SourceParameterDefinition));
  }
}