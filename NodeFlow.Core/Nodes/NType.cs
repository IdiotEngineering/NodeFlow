using Humanizer;
using Newtonsoft.Json;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   Represents a Node Type (different from C# types, and serializable)
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class NType
  {
    #region Fields / Properties

    /// <summary>
    ///   The DisplayName of the type (not qualified)
    /// </summary>
    [JsonProperty] public readonly string DisplayName;

    /// <summary>
    ///   The qualified (full, unique) DisplayName of the type.
    /// </summary>
    [JsonProperty] public readonly string QualifiedName;

    #endregion

    internal NType(string displayName, string qualifiedName)
    {
      DisplayName = displayName.Humanize(LetterCasing.Title);
      QualifiedName = qualifiedName;
    }

    [JsonConstructor]
    // ReSharper disable once UnusedMember.Local
    private NType()
    {
    }

    protected bool Equals(NType other)
    {
      return string.Equals(QualifiedName, other?.QualifiedName);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((NType) obj);
    }

    public override int GetHashCode()
    {
      return QualifiedName?.GetHashCode() ?? 0;
    }

    public static bool operator ==(NType left, NType right)
    {
      return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
    }

    public static bool operator !=(NType left, NType right)
    {
      return !(left == right);
    }
  }
}