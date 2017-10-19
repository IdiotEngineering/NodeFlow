using NodeFlow.Core.Graph;
using NodeFlow.Core.Utilities;
using System.Linq;
using NodeFlow.Core.Nodes;

namespace NodeFlow.Core.CodeGeneration
{
  /// <summary>
  ///   Code representation of a entire graph.
  /// </summary>
  public class CGraph
  {
    public string Guid;
    public string ClassName;
    public CField[] BoundFields;
    public CMethod[] Methods;

    public CGraph(NGraph nGraph)
    {
      Guid = nGraph.Guid.ToString();
      ClassName = nGraph.Guid.ToSymbolSafeGuid();
      // Collect all bound RETURN fields. We don't care about bound input fields that
      // aren't connected as outputs (like constants). First create a map of
      // Bindings->CField for use with method generation.
      var fieldNameGenerator = new ShortSymbolGenerator {Predicate = "FLD_"};
      var bindingsToCFields = nGraph.Nodes.Values
        .SelectMany(node => node.ParameterBindings.Where(b => b.TargetNode == node))
        .Where(binding => binding.LiteralValue == null)
        .Select(binding => new CField(binding, fieldNameGenerator))
        .ToDictionary(cField => cField.ParameterBinding, cField => cField);
      // Also save all fields to the BoundFields member variable
      BoundFields = bindingsToCFields.Values.ToArray();
      // Methods are generated directly from nodes using the fields map.
      Methods = nGraph.Nodes.Values.Select(node => new CMethod(node, bindingsToCFields)).ToArray();
    }
  }
}