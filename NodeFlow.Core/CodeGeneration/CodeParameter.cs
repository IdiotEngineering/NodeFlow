using System;
using System.Collections.Generic;
using NodeFlow.Core.Graph;
using NodeFlow.Core.Nodes;

namespace NodeFlow.Core.CodeGeneration
{
  public class CodeParameter
  {
    #region Fields / Properties

    /// <summary>
    ///   Only valid in the following cases:
    ///   - IsField: Value will be the field name.
    ///   - IsLiteral: Value will be the literal value (with quotes for strings already there).
    ///   - IsBoundContinuation: Value will be the method name the lambda action should call.
    /// </summary>
    public string FieldNameOrValue;

    // Types (In this ugly format for Mustache.Net, sadly)
    public bool IsField;
    public bool IsLiteral;
    public bool IsBoundContinuation;
    public bool IsUnboundContinuation;
    public bool IsUnboundOptional;
    // Other meta data for Mustache.Net
    public bool IsOut;
    public bool NeedsComma;
    // Used for debug code and comments
    public string SourceNodeGuid;
    public string SourceParamDisplayName;
    public string TargetNodeGuid;
    public string TargetParamDisplayName;

    #endregion

    public CodeParameter(ParameterDefinition parameterDefinition, ParameterBinding parameterBinding,
      IReadOnlyDictionary<ParameterBinding, CodeField> bindingsToCFields, bool isLast)
    {
      IsOut = parameterDefinition.IsOut;
      NeedsComma = !isLast;
      if (parameterBinding == null)
      {
        if (!parameterDefinition.IsOptional && parameterDefinition.Type != Primitives.NAction)
          throw new Exception("Required parameter is not bound.");
        IsUnboundOptional = parameterDefinition.IsOptional;
        IsUnboundContinuation = !parameterDefinition.IsOptional;
      }
      else if (parameterDefinition.Type == Primitives.NAction)
      {
        IsBoundContinuation = true;
        FieldNameOrValue = parameterBinding.LiteralValue;
      }
      else if (parameterBinding.LiteralValue != null)
      {
        IsLiteral = true;
        FieldNameOrValue = parameterBinding.LiteralValue;
      }
      else
      {
        IsField = true;
        FieldNameOrValue = bindingsToCFields[parameterBinding].Name;
      }
      // If it's bound, save some meta-data aboud it for comments and debug code generation.
      if (parameterBinding == null) return;
      SourceNodeGuid = parameterBinding.SourceNode?.Guid.ToString();
      SourceParamDisplayName = parameterBinding.SourceParameterDefinition?.DisplayName;
      TargetNodeGuid = parameterBinding.TargetNode.Guid.ToString();
      TargetParamDisplayName = parameterBinding.TargetParameterDefinition.DisplayName;
    }
  }
}