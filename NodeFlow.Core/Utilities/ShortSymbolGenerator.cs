using Newtonsoft.Json;

namespace NodeFlow.Core.Utilities
{
  /// <summary>
  ///   Simple short Guid representation.
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public struct ShortGuid
  {
    #region Fields / Properties

    [JsonProperty] private readonly string _value;

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
  [JsonObject(MemberSerialization.OptIn)]
  public class ShortSymbolGenerator
  {
    #region Fields / Properties

    [JsonProperty] public string Predicate = "SYM_";

    [JsonProperty]
    public int NextNumberId { get; private set; }

    #endregion

    public ShortGuid GetNextGuid() =>
      new ShortGuid(Predicate + (NextNumberId++).ToSymbolString());
  }
}