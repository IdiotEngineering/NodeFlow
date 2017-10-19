using System;
using System.Collections.Generic;
using NodeFlow.Core.Graph;
using NodeFlow.Core.Nodes;

namespace NodeFlow.Core.CodeGeneration
{
  public class CParameter
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

    public CParameter(NParameterDefinition nParameterDefinition, NParameterBinding nParameterBinding,
      IReadOnlyDictionary<NParameterBinding, CField> bindingsToCFields, bool isLast)
    {
      IsOut = nParameterDefinition.IsOut;
      NeedsComma = !isLast;
      if (nParameterBinding == null)
      {
        if (!nParameterDefinition.IsOptional && nParameterDefinition.Type != NPrimitives.NAction)
          throw new Exception("Required parameter is not bound.");
        IsUnboundOptional = nParameterDefinition.IsOptional;
        IsUnboundContinuation = !nParameterDefinition.IsOptional;
      }
      else if (nParameterDefinition.Type == NPrimitives.NAction)
      {
        IsBoundContinuation = true;
        FieldNameOrValue = nParameterBinding.LiteralValue;
      }
      else if (nParameterBinding.LiteralValue != null)
      {
        IsLiteral = true;
        FieldNameOrValue = nParameterBinding.LiteralValue;
      }
      else
      {
        IsField = true;
        FieldNameOrValue = bindingsToCFields[nParameterBinding].Name;
      }
      // If it's bound, save some meta-data aboud it for comments and debug code generation.
      if (nParameterBinding == null) return;
      SourceNodeGuid = nParameterBinding.SourceNode?.Guid.ToString();
      SourceParamDisplayName = nParameterBinding.SourceParameterDefinition?.DisplayName;
      TargetNodeGuid = nParameterBinding.TargetNode.Guid.ToString();
      TargetParamDisplayName = nParameterBinding.TargetParameterDefinition.DisplayName;
    }
  }
}