﻿using System;
using System.Net.NetworkInformation;
using Humanizer;

namespace NodeFlow.Core.Nodes
{
  /// <summary>
  ///   Represents a Node Type (different from C# types, and serializable)
  /// </summary>
  public class NType
  {
    #region Fields / Properties

    /// <summary>
    ///   The DisplayName of the type (not qualified)
    /// </summary>
    public readonly string DisplayName;

    /// <summary>
    ///   The qualified (full, unique) DisplayName of the type.
    /// </summary>
    public readonly string QualifiedName;

    /// <summary>
    ///   The ModuleDefinition the type is a member of.
    /// </summary>
    public readonly NModuleDefinition ModuleDefinition;

    #endregion

    internal NType(string displayName, string qualifiedName, NModuleDefinition moduleDefinition)
    {
      DisplayName = displayName.Humanize(LetterCasing.Title);
      QualifiedName = qualifiedName;
      ModuleDefinition = moduleDefinition;
    }
  }
}