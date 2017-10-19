// Generated Code. Graph [ 22650f28-ce72-4932-9986-b9325dc9a188 ]
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace NodeFlow.Generated
{
  public class SYM_22650f28ce7249329986b9325dc9a188 : NodeFlow.Core.Graph.NGeneratedGraph
  {
    #region Fields / Properties
    // [SYM_B.Value] -> [SYM_A.Value]
    public System.String FLD_A;
    #endregion
    #region Methods
    
    // Node [Print:SYM_A]
    public void SYM_A()
    {
      NodeFlow.Cli.Program.Print(FLD_A);
    }
    
    // Node [Begin:SYM_B] -> [Print:SYM_A]
    public void SYM_B()
    {
      NodeFlow.Cli.Program.Begin(out FLD_A);
      SYM_A();
    }
    #endregion

    // Used to hook the event handler node types. This will be called for each graph instance created.
    protected override void RegisterEventHandlers()
    {
      throw new System.NotImplementedException();
    }
  }
}