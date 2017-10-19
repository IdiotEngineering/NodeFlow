using HandlebarsDotNet;
using NodeFlow.Core.Graph;

namespace NodeFlow.Core.CodeGeneration
{
  /// <summary>
  ///   The C# code generator.
  /// </summary>
  public class CSharpCodeGenerator
  {
    #region Fields / Properties

    public readonly Graph.Graph Graph;
    public int Indentation = 2;

    /// <summary>
    ///   Rendered from a CodeGraph instance.
    /// </summary>
    private readonly string _templateString =
      @"// Generated Code.
// Graph [ {{Guid}} ]

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier

namespace NodeFlow.Generated
{
  public class {{ClassName}} : NodeFlow.Core.Runtime.GeneratedGraph
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
  }
}";

    #endregion

    public CSharpCodeGenerator()
    {
    }

    public CSharpCodeGenerator(Graph.Graph graph)
    {
      Graph = graph;
    }

    public string Compile()
    {
      var cGraph = new CodeGraph(Graph);
      return Handlebars.Compile(_templateString)(cGraph);
    }
  }
}