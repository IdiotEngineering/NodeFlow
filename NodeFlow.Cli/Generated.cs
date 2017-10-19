// Generated Code.
// Graph [ 7871648d-638d-4ef3-a055-d27146f6b93a ]

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier

namespace NodeFlow.Generated
{
  public class SYM_7871648d638d4ef3a055d27146f6b93a : NodeFlow.Core.Runtime.GeneratedGraph
  {
    #region Fields / Properties

    // [NODE_B.Begin] -> [NODE_A.Print]
    public System.String FLD_A;

    #endregion

    #region Methods
    
    // Node [Print:NODE_A]
    public void NODE_A()
    {
      NodeFlow.Core.Module.Samples.Print(
        // Value - Field
        FLD_A,
        // Int Value - Literal
        42,
        // String Value - Literal
        "Hello, world!"
      );
    }
    
    // Node [Begin:NODE_B]
    public void NODE_B()
    {
      NodeFlow.Core.Module.Samples.Begin(
        // Start - BoundContinuation
        NODE_A,
        //  - UnboundContinuation
        () => {},
        // Value - Field
        out FLD_A
      );
    }

    #endregion
  }
}