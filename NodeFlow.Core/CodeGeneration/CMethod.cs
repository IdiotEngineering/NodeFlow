using System.Collections.Generic;
using System.Linq;
using NodeFlow.Core.Graph;

namespace NodeFlow.Core.CodeGeneration
{
  public class CMethod
  {
    #region Fields / Properties

    /// <summary>
    ///   The NNode this CMethod was created from.
    /// </summary>
    public NNode Node;

    public string MethodName;
    public string CallQualifiedName;
    public string ImplicitContinuation;
    public CParameter[] Parameters;
    // Used for debug code and comments
    public string DisplayName;

    #endregion

    public CMethod(NNode node, IReadOnlyDictionary<NParameterBinding, CField> bindingsToCFields)
    {
      Node = node;
      MethodName = node.Guid.ToString();
      DisplayName = node.NodeDefinition.DisplayName;
      CallQualifiedName = node.NodeDefinition.SymbolName;
      ImplicitContinuation = node.ImplicitContinuation?.Guid.ToString();
      // Start by creating a map of NParameterDefinition -> NBoundProperty
      var nPropToBoundProps = node.NodeDefinition.Parameters.ToDictionary(nParameter => nParameter,
        nParameter => node.ParameterBindings.FirstOrDefault(binding =>
          (nParameter.IsOut ? binding.SourceParameterDefinition : binding.TargetParameterDefinition) == nParameter));
      // Generate a property for all NParameters in the NNode, not just the bound ones, in-order.
      Parameters = node.NodeDefinition.Parameters.Select((nParameter, index) =>
        new CParameter(nParameter,
          nPropToBoundProps[nParameter],
          bindingsToCFields,
          index + 1 == node.NodeDefinition.Parameters.Count)
        ).ToArray();
    }
  }
}