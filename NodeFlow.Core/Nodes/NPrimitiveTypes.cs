using System;
using System.Collections.Generic;
using NodeFlow.Core.Utilities;

namespace NodeFlow.Core.Nodes
{
  public static class NPrimitives
  {
    #region Fields / Properties

    public static NType NNull = new NType("NNull", "NPrimitive.NNull", NModule.NCore);

    #endregion

    public static NType NNumber = new NType("NNumber", "NPrimitive.NNumber", NModule.NCore);
    public static NType NFloat = new NType("NFloat", "NPrimitive.NFloat", NModule.NCore);
    public static NType NString = new NType("NString", "NPrimitive.NString", NModule.NCore);
    public static NType NBoolean = new NType("NBoolean", "NPrimitive.NBoolean", NModule.NCore);
    public static NType NEnum = new NType("NEnum", "NPrimitive.NEnum", NModule.NCore);
    public static NType NObject = new NType("NObject", "NPrimitive.NObject", NModule.NCore);

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
      {typeof (Object), NObject}
    };

    public static Dictionary<NType, Type> NTypeToCSharpTypeMap = new Dictionary<NType, Type>
    {
      {NNumber, typeof (Int32)},
      {NFloat, typeof (Single)},
      {NString, typeof (String)},
      {NBoolean, typeof (Boolean)},
      {NEnum, typeof (Enum)},
      {NObject, typeof (Object)}
    };

    public static NType GetNTypeFromSystemType(Type type)
    {
      return CSharpToNTypeMap.GetValueOrDefault(type.GetElementType() ?? type);
    }

    public static Type GetSystemTypeFromNType(NType nType)
    {
      return NTypeToCSharpTypeMap[nType];
    }
  }
}