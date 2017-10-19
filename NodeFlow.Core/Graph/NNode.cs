using System.Collections.Generic;
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
    ///   Bound input parameters. Optional parameters don't need to be bound.
    /// </summary>
    public List<NParameterBinding> InputParameterBindings = new List<NParameterBinding>();

    /// <summary>
    ///   Bound output parameters. Optional parameters don't need to be bound. Note that this
    ///   is a one to many relationship.
    /// </summary>
    public List<NParameterBinding> ReturnParameterBindings = new List<NParameterBinding>();

    /// <summary>
    ///   The continuation (control flow) binding for this node. Zero or more of these can
    ///   be bound. For NContinuationSequencing.Single, only a single value will be in this
    ///   dictionary, keyed as NControlFlowParameter.Implicit.
    /// </summary>
    public Dictionary<NControlFlowParameter, NNode> ContinuationBindings =
      new Dictionary<NControlFlowParameter, NNode>();

    #endregion

    public string GenerateCode()
    {
      // Support return types
      return string.Format("private void {0}() {{ {1}(); }}", Guid, NodeDefinition.SymbolName);
    }
  }
}