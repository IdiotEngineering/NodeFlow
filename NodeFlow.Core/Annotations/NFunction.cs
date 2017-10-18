using System;

namespace NodeFlow.Core.Annotations
{
  [AttributeUsage(AttributeTargets.Method)]
  public class NFunction : Attribute
  {
    #region Fields / Properties

    public string DisplayName;
    public string Description;

    #endregion

    public NFunction(string description)
    {
      Description = description;
    }
  }
}