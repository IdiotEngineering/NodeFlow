using System.Linq;

namespace NodeFlow.Core.Utilities
{
  /// <summary>
  ///   Simple short Guid representation.
  /// </summary>
  public struct ShortGuid
  {
    #region Fields / Properties

    private readonly string _value;

    #endregion

    public ShortGuid(string value)
    {
      _value = value;
    }

    public override bool Equals(object obj) =>
      obj is ShortGuid && ((ShortGuid) obj)._value == _value;

    public override int GetHashCode() => _value.GetHashCode();
    public override string ToString() => _value;
  }

  /// <summary>
  ///   A dead-basic class to generate very short symbol names (that can be used for compilation) from an
  ///   incrementing counter "NextNumberId".
  /// </summary>
  public class ShortSymbolGenerator
  {
    #region Fields / Properties

    public string Predicate = "SYM_";
    public int NextNumberId { get; private set; } = 0;

    #endregion

    public ShortGuid GetNextGuid() =>
      new ShortGuid(Predicate + (NextNumberId++).ToSymbolString());
  }
}