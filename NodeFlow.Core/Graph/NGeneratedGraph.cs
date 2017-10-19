using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeFlow.Core.Graph
{
  public abstract class NGeneratedGraph
  {
    protected NGeneratedGraph()
    {
      RegisterEventHandlers();
    }

    protected abstract void RegisterEventHandlers();
  }
}
