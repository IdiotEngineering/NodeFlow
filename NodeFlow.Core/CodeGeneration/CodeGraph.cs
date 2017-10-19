using System.Linq;
using NodeFlow.Core.Graph;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.CodeGeneration
{
  /// <summary>
  ///   Code representation of a entire graph.
  /// </summary>
  public class CodeGraph
  {
    #region Fields / Properties

    public string Guid;
    public string ClassName;
    public CodeField[] BoundFields;
    public CodeMethod[] Methods;

    #endregion

    public CodeGraph(Graph.Graph graph)
    {
      Guid = graph.Guid.ToString();
      ClassName = graph.Guid.ToSymbolSafeGuid();
      // Collect all bound RETURN fields. We don't care about bound input fields that
      // aren't connected as outputs (like constants). First create a map of
      // Bindings->CodeField for use with method generation.
      var fieldNameGenerator = new ShortSymbolGenerator {Predicate = "FLD_"};
      var bindingsToCFields = graph.Nodes.Values
        .SelectMany(node => node.ParameterBindings.Where(b => b.TargetNode == node))
        .Where(binding => binding.LiteralValue == null)
        .Select(binding => new CodeField(binding, fieldNameGenerator))
        .ToDictionary(cField => cField.ParameterBinding, cField => cField);
      // Also save all fields to the BoundFields member variable
      BoundFields = bindingsToCFields.Values.ToArray();
      // Methods are generated directly from nodes using the fields map.
      Methods = graph.Nodes.Values.Select(node => new CodeMethod(node, bindingsToCFields)).ToArray();
    }
  }
}