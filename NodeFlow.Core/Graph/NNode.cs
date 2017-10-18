using System.Collections.Generic;
using NodeFlow.Core.Nodes;

namespace NodeFlow.Core.Graph
{
  /// <summary>
  ///   A node instance in an NGraph. NodeDefinition is the type, this is the instance.
  /// </summary>
  public class NNode
  {
    #region Fields / Properties

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
    public Dictionary<NParameter, NParameterBinding> InputParameterBindings =
      new Dictionary<NParameter, NParameterBinding>();

    /// <summary>
    ///   Bound output parameters. Optional parameters don't need to be bound. Note that this
    ///   is a one to many relationship.
    /// </summary>
    public Dictionary<NParameter, List<NParameterBinding>> ReturnParameterBindings =
      new Dictionary<NParameter, List<NParameterBinding>>();

    /// <summary>
    ///   All possible control flow inputs. Zero or more of them may actually be used.
    /// </summary>
    public List<NNode> PossibleControlFlowInputs = new List<NNode>();

    /// <summary>
    ///   The continuation (control flow) binding for this node. Zero or more of these can
    ///   be bound. For NContinuationSequencing.Single, only a single value will be in this
    ///   dictionary, keyed as NControlFlowParameter.Implicit.
    /// </summary>
    public Dictionary<NControlFlowParameter, NNode> ContinuationBindings =
      new Dictionary<NControlFlowParameter, NNode>();

    #endregion
  }
}