//<summary>
//  Title   : Enums
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20081007: mzbrzezny: Arrays support is added
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator.PRIVATE
{
  internal enum Medium_T:short
  {
    /// <summary>
    /// data type is not set yet
    /// </summary>
    None=-1,
      /// <summary>
      /// 
      /// </summary>
    Flag = 0,
      /// <summary>
      /// 
      /// </summary>
    Input = 1,
      /// <summary>
      /// 
      /// </summary>
    Output = 2,
      /// <summary>
      /// 
      /// </summary>
    Register = 10,
      /// <summary>
      /// 
      /// </summary>
    Timer = 11,
      /// <summary>
      /// 
      /// </summary>
    Counter = 12,
    SimulationRegister=20,
    SimulationFlags=21,
      /// <summary>
      /// 
      /// </summary>
    SByte = 100,
      /// <summary>
      /// 
      /// </summary>
    Byte = 101,
      /// <summary>
      /// 
      /// </summary>
    Short = 102,
      /// <summary>
      /// 
      /// </summary>
    UShort = 103,
      /// <summary>
      /// 
      /// </summary>
    Int = 104,
      /// <summary>
      /// 
      /// </summary>
    Uint = 105,
      /// <summary>
      /// 
      /// </summary>
    Long = 106,
      /// <summary>
      /// 
      /// </summary>
    Ulong = 107,
      /// <summary>
      /// 
      /// </summary>
    Float = 108,
      /// <summary>
      /// 
      /// </summary>
    Double = 109,
      /// <summary>
      /// 
      /// </summary>
    Decimal = 110,
      /// <summary>
      /// 
      /// </summary>
    Bool = 111,
      /// <summary>
      /// 
      /// </summary>
    DateTime = 112,
      /// <summary>
      /// 
      /// </summary>
    TimeSpan = 113,
      /// <summary>
      /// 
      /// </summary>
    String = 114,
      /// <summary>
      /// 
      /// </summary>
    Object = 115,
    ArrayOfInts=200,
    ArrayOfStrings=201,
    ArrayOfDoulbes=202
  };
}
