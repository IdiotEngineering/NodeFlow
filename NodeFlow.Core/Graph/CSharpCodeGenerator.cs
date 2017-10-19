using System;
using System.Linq;
using System.Text;
using HandlebarsDotNet;
using NodeFlow.Core.CodeGeneration;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Graph
{
  /// <summary>
  ///   The C# code generator.
  /// </summary>
  public class CSharpCodeGenerator
  {
    #region Fields / Properties

    public readonly NGraph Graph;
    public int Indentation = 2;

    /// <summary>
    ///   Rendered from a CGraph instance.
    /// </summary>
    private readonly string _templateString =
      @"// Generated Code. Graph [ {{Guid}} ]
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
namespace NodeFlow.Generated
{
  public class {{ClassName}} : NodeFlow.Core.Graph.NGeneratedGraph
  {
    #region Fields / Properties

    {{#BoundFields}}
    // [{{SourceNodeGuid}}.{{SourceParamDisplayName}}] -> [{{TargetNodeGuid}}.{{TargetParamDisplayName}}]
    public {{QualifiedTypeName}} {{Name}};
    {{/BoundFields}}

    #endregion

    #region Methods
    {{#Methods}}
    
    // Node [{{DisplayName}}:{{MethodName}}]
    public void {{MethodName}}()
    {
      {{CallQualifiedName}}(
    {{#Parameters}}
      {{#if IsField}}
        // {{TargetParamDisplayName}} - Field
        {{#if IsOut}}out {{/if}}{{{FieldNameOrValue}}}{{#if NeedsComma}},{{/if}}
      {{/if}}
      {{#if IsLiteral}}
        // {{TargetParamDisplayName}} - Literal
        {{{FieldNameOrValue}}}{{#if NeedsComma}},{{/if}}
      {{/if}}
      {{#if IsBoundContinuation}}
        // {{TargetParamDisplayName}} - BoundContinuation
        {{{FieldNameOrValue}}}{{#if NeedsComma}},{{/if}}
      {{/if}}
      {{#if IsUnboundContinuation}}
        // {{TargetParamDisplayName}} - UnboundContinuation
        () => {}{{#if NeedsComma}},{{/if}}
      {{/if}}
      {{#if IsUnboundOptional}}
        // {{TargetParamDisplayName}} - UnboundOptional}
      {{/if}}
    {{/Parameters}}
      );
      {{#if ImplicitContinuation}}
      {{ImplicitContinuation}}();
      {{/if}}
    }
    {{/Methods}}

    #endregion

    // Used to hook the event handler node types. This will be called for each graph instance created.
    protected override void RegisterEventHandlers()
    {
      throw new System.NotImplementedException();
    }
  }
}";

    #endregion

    public CSharpCodeGenerator()
    {
    }

    public CSharpCodeGenerator(NGraph graph)
    {
      Graph = graph;
    }

    public string Compile()
    {
      var cGraph = new CGraph(Graph);
      return Handlebars.Compile(_templateString)(cGraph);
    }
  }
}