// Generated Code.
// Graph [ 4ed5f20a-2b3d-49d4-9649-e8b686825ca3 ]

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier

namespace NodeFlow.Generated
{
  public class SYM_4ed5f20a2b3d49d49649e8b686825ca3 : NodeFlow.Core.Runtime.NGeneratedGraph
  {
    #region Fields / Properties

    // [SYM_B.Begin] -> [SYM_A.Print]
    public System.String FLD_A;

    #endregion

    #region Methods
    
    // Node [Print:SYM_A]
    public void SYM_A()
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
    
    // Node [Begin:SYM_B]
    public void SYM_B()
    {
      NodeFlow.Core.Module.Samples.Begin(
        // Start - BoundContinuation
        SYM_A,
        //  - UnboundContinuation
        () => {},
        // Value - Field
        out FLD_A
      );
    }

    #endregion
  }
}