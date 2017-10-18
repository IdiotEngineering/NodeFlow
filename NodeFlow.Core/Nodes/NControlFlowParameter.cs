using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  /// A single control flow parameter for a node. These are not data parameters. They are control
  /// inputs and outputs.
  /// </summary>
  public class NControlFlowParameter
  {
    public static readonly NControlFlowParameter Implicit = new NControlFlowParameter();

    /// <summary>
    /// The name displayed for the control from ancor. This is only used for Multi-Return nodes and
    /// apears next to the return control ancor.
    /// </summary>
    public string DisplayName;
  }
}
