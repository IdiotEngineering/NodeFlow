using System;
using System.Collections.Generic;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Nodes
{
  public static class Primitives
  {
    #region Fields / Properties

    public static NType NNull = new NType("Null", "Primitive.Null");

    #endregion

    public static NType GetNTypeFromSystemType(Type type)
    {
      return CSharpToNTypeMap.GetValueOrDefault(type.GetElementType() ?? type);
    }

    public static Type GetSystemTypeFromNType(NType nType)
    {
      return NTypeToCSharpTypeMap[nType.QualifiedName];
    }

    public static NType NNumber = new NType("Number", "Primitive.Number");
    public static NType NFloat = new NType("Float", "Primitive.Float");
    public static NType NString = new NType("String", "Primitive.String");
    public static NType NBoolean = new NType("Boolean", "Primitive.Boolean");
    public static NType NEnum = new NType("Enum", "Primitive.Enum");
    public static NType NObject = new NType("Object", "Primitive.Object");
    public static NType NAction = new NType("Action", "Primitive.Action");

    public static Dictionary<Type, NType> CSharpToNTypeMap = new Dictionary<Type, NType>
    {
      {typeof (Byte), NNumber},
      {typeof (UInt16), NNumber},
      {typeof (UInt32), NNumber},
      {typeof (UInt64), NNumber},
      {typeof (SByte), NNumber},
      {typeof (Int16), NNumber},
      {typeof (Int32), NNumber},
      {typeof (Int64), NNumber},
      {typeof (Decimal), NFloat},
      {typeof (Single), NFloat},
      {typeof (Double), NFloat},
      {typeof (String), NString},
      {typeof (Boolean), NBoolean},
      {typeof (Enum), NEnum},
      {typeof (Object), NObject},
      {typeof (Action), NAction}
    };

    public static Dictionary<string, Type> NTypeToCSharpTypeMap = new Dictionary<string, Type>
    {
      {NNumber.QualifiedName, typeof (Int32)},
      {NFloat.QualifiedName, typeof (Single)},
      {NString.QualifiedName, typeof (String)},
      {NBoolean.QualifiedName, typeof (Boolean)},
      {NEnum.QualifiedName, typeof (Enum)},
      {NObject.QualifiedName, typeof (Object)},
      {NAction.QualifiedName, typeof (Action)}
    };
  }
}