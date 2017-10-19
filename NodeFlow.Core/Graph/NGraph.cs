using System;
using System.Collections.Generic;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Graph
{
  /// <summary>
  ///   A complete node-flow graph.
  /// </summary>
  public class NGraph
  {
    #region Fields / Properties

    /// <summary>
    ///   The graph-wide GUID. Individual nodes are assigned very short UUIDs from here.
    /// </summary>
    public Guid Guid = Guid.NewGuid();

    public readonly Dictionary<ShortGuid, NNode> Nodes = new Dictionary<ShortGuid, NNode>(); 

    /// <summary>
    ///   The short-guid generator for node GUIDs.
    /// </summary>
    private readonly ShortSymbolGenerator _nodeGuidGenerator = new ShortSymbolGenerator();

    #endregion

    public NNode MakeNewNode(NNodeDefinition nodeDefinition)
    {
      var nNode = new NNode
      {
        Guid = _nodeGuidGenerator.GetNextGuid(),
        NodeDefinition = nodeDefinition
      };
      Nodes.Add(nNode.Guid, nNode);
      return nNode;
    }
  }
}