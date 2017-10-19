using NodeFlow.Core.Graph;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.CodeGeneration
{
  public class CField
  {
    #region Fields / Properties

    /// <summary>
    ///   The paramater binding this field was created from.
    /// </summary>
    public NParameterBinding ParameterBinding;

    public string Name;
    public string QualifiedTypeName;

    // Used for debug code and comments
    public string SourceNodeGuid;
    public string SourceParamDisplayName;
    public string TargetNodeGuid;
    public string TargetParamDisplayName;

    #endregion

    public CField(NParameterBinding binding, ShortSymbolGenerator generator)
    {
      ParameterBinding = binding;
      Name = generator.GetNextGuid().ToString();
      QualifiedTypeName = NPrimitives.GetSystemTypeFromNType(binding.SourceParameter.Type).FullName;
      SourceNodeGuid = binding.SourceNode.Guid.ToString();
      SourceParamDisplayName = binding.SourceNode.NodeDefinition.DisplayName;
      TargetNodeGuid = binding.TargetNode.Guid.ToString();
      TargetParamDisplayName = binding.TargetNode.NodeDefinition.DisplayName;
    }
  }
}