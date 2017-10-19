namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   A single control flow parameter for a node. These are not data parameters. They are control
  ///   inputs and outputs.
  /// </summary>
  public class NControlFlowParameter
  {
    #region Fields / Properties

    public static readonly NControlFlowParameter Implicit = new NControlFlowParameter();

    /// <summary>
    ///   The name displayed for the control from ancor. This is only used for Multi-Return nodes and
    ///   apears next to the return control ancor.
    /// </summary>
    public string DisplayName;

    #endregion
  }
}