using System;
using System.Linq;
using System.Text;
using HandlebarsDotNet;
using NodeFlow.Core.Nodes;
using NodeFlow.Core.Utilities;
using System.Dynamic;

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
    ///   Rendered from a Graph instance.
    /// </summary>
    private readonly string _templateString =
      @"// Generated Code. Graph [ {{Guid}} ]
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace NodeFlow.Generated
{
  public class {{ClassName}} : NodeFlow.Core.Graph.NGeneratedGraph
  {
    #region Fields / Properties
    {{#Fields}}
    // [{{SourceNodeGuid}}.{{SourceParamDisplayName}}] -> [{{TargetNodeGuid}}.{{TargetParamDisplayName}}]
    public {{TypeQualifiedName}} {{FieldName}};
    {{/Fields}}
    #endregion
    #region Methods
    {{#Methods}}
    
    // Node [{{DisplayName}}:{{MethodName}}]{{#if ContinuationDispayName}} -> [{{ContinuationDispayName}}:{{ContinuationQualifiedName}}]{{/if}}
    public void {{MethodName}}()
    {
      {{QualifiedName}}({{{Params}}});
      {{#if ContinuationQualifiedName}}
      {{ContinuationQualifiedName}}();
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
      var fieldNameGenerator = new ShortSymbolGenerator {Predicate = "FLD_"};
      var allInputParameterBindings = Graph.Nodes.Values.SelectMany(node => node.InputParameterBindings);
      var allReturnParameterBindings = Graph.Nodes.Values.SelectMany(node => node.ReturnParameterBindings);
      // Generate a list of fields from the RETURN parameters
      var fields = allReturnParameterBindings
        .Where(binding => binding.LiteralValue == null)
        .Select(binding => new
        {
          FieldName = fieldNameGenerator.GetNextGuid(),
          TypeQualifiedName = NPrimitives.GetSystemTypeFromNType(binding.SourceParameter.Type).FullName,
          Binding = binding,
          SourceNodeGuid = binding.SourceNode.Guid,
          TargetNodeGuid = binding.TargetNode.Guid,
          SourceParamDisplayName = binding.SourceParameter.DisplayName,
          TargetParamDisplayName = binding.TargetParameter.DisplayName
        }).ToArray();
      // Map that to binding->field name
      var bindingToFieldName = fields.ToDictionary(field => field.Binding, field => field.FieldName);
      var data = new
      {
        Graph.Guid,
        ClassName = Graph.Guid.ToSymbolSafeGuid(),
        Fields = fields,
        Methods = Graph.Nodes.Values.Select(node => new
        {
          MethodName = node.Guid,
          node.NodeDefinition.DisplayName,
          QualifiedName = node.NodeDefinition.SymbolName,
          ContinuationQualifiedName = node.ContinuationBindings.GetValueOrDefault(NControlFlowParameter.Implicit)?.Guid,
          ContinuationDispayName = node.ContinuationBindings.GetValueOrDefault(NControlFlowParameter.Implicit)?.NodeDefinition.DisplayName,
          Params = string.Join(", ", node.InputParameterBindings.Select(param => new
          {
            Order = param.TargetParameter.Position,
            Value = bindingToFieldName.ContainsKey(param) ? bindingToFieldName[param].ToString() : param.LiteralValue
          }).Concat(node.ReturnParameterBindings.Select(param => new
          {
            Order = param.TargetParameter.Position,
            Value = "out " + (bindingToFieldName.ContainsKey(param) ? bindingToFieldName[param].ToString() : param.LiteralValue)
          })).OrderBy(param => param.Order).Select(param => param.Value))
        })
      };
      return Handlebars.Compile(_templateString)(data);
//      const string codeLayout = @"// Generated Code. Graph [{0}]
//namespace NodeFlow.Generated
//{{
//  public static class {1}
//  {{
//    #region Fields / Properties
//{2}
//    #endregion

//{3}
//  }}
//}}";
//      var fields = new StringBuilder();
//      var methodsStrBuilder = new StringBuilder();
//      foreach (var node in Graph.Nodes.Values)
//      {
//        CompileNode(node, methodsStrBuilder);
//      }
//      return string.Format(codeLayout,
//        Graph.Guid,
//        Graph.Guid.ToSymbolSafeGuid(),
//        fields.ToString().RemoveEmptyLines().Indent(Indentation*2),
//        methodsStrBuilder.ToString().Indent(Indentation*2));
    }

    private void CompileNode(NNode node, StringBuilder code)
    {
      //code.AppendLine(string.Format("// Node [ {0} ]", node.NodeDefinition.DisplayName));
      //code.AppendLine(string.Format("public static void {1}()", node.Guid));
      //code.AppendLine("{");
      //code.Append(CompileSymCall(node, code))
      //code.Append(CompileContinuation(node).Indent(Indentation));
      //code.AppendLine("}");
      //// TODO: Handle node return-data types
      //// Final function format
      //return string.Format(functionLayout,
      //  node.NodeDefinition.DisplayName,


      //  CompileContinuation(node).Indent(Indentation)).RemoveEmptyLines();
    }

    private static void CompileSymCall(NNode node, StringBuilder code)
    {
      // TODO: Handle node input types
      // TODO: Handle unused output args
      code.AppendLine(string.Format("{0}({1});", node.NodeDefinition.SymbolName, ""));
    }

    private static string CompileContinuation(NNode node)
    {
      var code = new StringBuilder();
      if (!node.ContinuationBindings.Any())
      {
        code.AppendLine("// No implicit or explicit continuation.");
      }
      else if (node.ContinuationBindings.ContainsKey(NControlFlowParameter.Implicit))
      {
        var continuation = node.ContinuationBindings[NControlFlowParameter.Implicit];
        code.AppendLine(string.Format("// Implicit continuation to node [ {0}: {1} ].",
          continuation.Guid, continuation.NodeDefinition.DisplayName));
        code.AppendLine(string.Format("{0}();", continuation.Guid));
      }
      return code.ToString().RemoveEmptyLines();
    }
  }
}