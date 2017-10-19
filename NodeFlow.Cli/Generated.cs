// Generated Code. Graph [ f2763cae-5f41-460c-8f23-e56d92cd9e07 ]
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
namespace NodeFlow.Generated
{
  public class SYM_f2763cae5f41460c8f23e56d92cd9e07 : NodeFlow.Core.Graph.NGeneratedGraph
  {
    #region Fields / Properties

    // [SYM_B.Begin] -> [SYM_A.Print]
    public System.String FLD_A;

    #endregion

    #region Methods
    
    // Node [Print:SYM_A]
    public void SYM_A()
    {
      NodeFlow.Cli.Program.Print(
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
      NodeFlow.Cli.Program.Begin(
        // Start - BoundContinuation
        SYM_A,
        //  - UnboundContinuation
        () => {},
        // Value - Field
        out FLD_A
      );
    }

    #endregion

    // Used to hook the event handler node types. This will be called for each graph instance created.
    protected override void RegisterEventHandlers()
    {
      throw new System.NotImplementedException();
    }
  }
}