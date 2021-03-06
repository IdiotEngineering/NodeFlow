﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Graph
{
  /// <summary>
  ///   A complete node-flow graph.
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class NGraph
  {
    #region Fields / Properties

    [JsonProperty] public readonly Dictionary<ShortGuid, NNode> Nodes = new Dictionary<ShortGuid, NNode>();

    /// <summary>
    ///   The graph-wide GUID. Individual nodes are assigned very short UUIDs from here.
    /// </summary>
    [JsonProperty] public Guid Guid = Guid.NewGuid();

    /// <summary>
    ///   The short-guid generator for node GUIDs.
    /// </summary>
    [JsonProperty] private int _nextNodeId;

    #endregion

    public NNode MakeNewNode(NNodeDefinition nodeDefinition)
    {
      var nNode = new NNode
      {
        Guid = new ShortGuid("NODE_" + (_nextNodeId++).ToSymbolString()),
        NodeDefinition = nodeDefinition
      };
      Nodes.Add(nNode.Guid, nNode);
      return nNode;
    }

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