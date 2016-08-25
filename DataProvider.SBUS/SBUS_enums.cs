//<summary>
//  Title   : Enums
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2003: created
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE
{
  /// <summary>
  /// Attribute Character values 
  /// </summary>
  internal enum AttributeCharacter: byte
  {
    /// <summary>
    /// the actual frame is a telegram
    /// </summary>
    Telegram = 0x0,
    /// <summary>
    /// The actual frame is a response consisting of data
    /// </summary>
    ResponseData = 0x1,
    /// <summary>
    /// The actual frame is a response consisting or ACK/NAK with additional information
    /// </summary>
    ResponseACK_NAK = 0x2
  }
  internal enum Medium_T: short
  {
    /// <summary>
    /// The Flag data type
    /// </summary>
    Flag = 0,
    /// <summary>
    /// The Flag data type
    /// </summary>
    Input = 1,
    /// <summary>
    /// The Flag data type
    /// </summary>
    Output = 2,
    /// <summary>
    /// The Flag data type
    /// </summary>
    Register = 10,
    /// <summary>
    /// The Flag data type
    /// </summary>
    Timer = 11,
    /// <summary>
    /// The Flag data type
    /// </summary>
    Counter = 12,
    /// <summary>
    /// The text data type
    /// </summary>
    Text
  };
  internal enum ProcState_T
  {
    PS_Running,
    PS_Halted,
    PS_Stopped,
    PS_Conditional,
    PS_Disconnected
  };
  internal enum ProcRetCode
  {
    PCD_TEXTTYPE = 1,
    PCD_LANTYPE = 2,
    PCD_ILANTYPE = 3,
    PCD_DBTYPE = 4,
    PCD_MAXDB = 7999,
    PCD_MAXDB1 = 3999,
    PCD_MAXDBLEN1 = 383,
    PCD_MAXDBLEN = 16384
  };
  internal enum SbusCode: byte
  {
    /// <summary>
    /// Read Counter
    /// </summary>
    coReadCounter = 0,
    /// <summary>
    /// Read Display Register
    /// </summary>
    coReadDisplayRegister = 1,
    /// <summary>
    /// Read Flag 
    /// </summary>
    coReadFlag = 2,
    /// <summary>
    /// Read Input 
    /// </summary>
    coReadInput = 3,
    /// <summary>
    /// Read Real Time Clock 
    /// </summary>
    coReadRealTimeClock = 4,
    /// <summary>
    /// Read Output
    /// </summary>
    coReadOutput = 5,
    /// <summary>
    /// read register
    /// </summary>
    coReadRegister = 6,
    /// <summary>
    /// Read Timer
    /// </summary>
    coReadTimer = 7,
    /// <summary>
    /// Write Counter
    /// </summary>
    coWriteCounter = 10,
    /// <summary>
    /// write flag
    /// </summary>
    coWriteFlag = 11,
    /// <summary>
    /// write real time clock
    /// </summary>
    coWriteRealTimeClock = 12,
    /// <summary>
    /// write output
    /// </summary>
    coWriteOutput = 13,
    /// <summary>
    /// write register
    /// </summary>
    coWriteRegister = 14,
    /// <summary>
    /// write timer
    /// </summary>
    coWriteTimer = 15,
    ///// <summary>
    ///// Read Write Multi-Medias
    ///// </summary>
    //coMM = 19,
    /// <summary>
    /// Read PCD Status
    /// </summary>
    coRS0 = 20,
    coRS1 = 21,
    coRS2 = 22,
    coRS3 = 23,
    coRS4 = 24,
    coRS5 = 25,
    coRS6 = 26,
    coRS7 = 27,
    /// <summary>
    /// Read S-BUS station number
    /// </summary>
    //coStationNumber = 29,
    //coProgramVersion = 32,
    coReadText = 33,
    coWriteText = 37,
    //coReadByte = 71,
    //coWriteIndexRegister = 82,
    //coXOB_17Interrupt = 130,
    //coXOB_18Interrupt = 131,
    //coXOB_19Interrupt = 132,
    //coReadHangupTimeout = 145,
    //coReadDataBlock = 150,
    //coWriteDataBlock = 151,
    //coReadBlockAddresses = 155,
    //coReadBlockSizes = 156,
    //coReadDBx = 159,
    //coReadUserEEPROMRegister = 161,
    //coWriteUserEEPROMRegister = 163,
  };
  internal enum FramePart
  {
    Head,
    Attribute,
    Response,
    Telegram,
    Acknowledge,
    CRC_1,
    CRC_2,
    FrameEnd
  };
  internal static class Definitions
  {
    /// <summary>
    /// Converts <see cref="Medium_T "/> to equivalent internal frame type <see cref="SbusCode"/> for write operations.
    /// </summary>
    /// <param name="type">The external type.</param>
    /// <returns>The internal frame type <see cref="SbusCode"/> used to write values.</returns>
    internal static SbusCode DataType2SbusCode4Write( Medium_T type )
    {
      SbusCode code = 0;
      switch ( type )
      {
        case Medium_T.Counter:
          code = SbusCode.coWriteCounter;
          break;
        case Medium_T.Flag:
          code = SbusCode.coWriteFlag;
          break;
        case Medium_T.Output:
          code = SbusCode.coWriteOutput;
          break;
        case Medium_T.Register:
          code = SbusCode.coWriteRegister;
          break;
        case Medium_T.Timer:
          code = SbusCode.coWriteTimer;
          break;
        case Medium_T.Text:
          code = SbusCode.coWriteText;
          break;
      }
      return code;
    }
    /// <summary>
    /// /// Converts <see cref="Medium_T "/> to equivalent internal frame type <see cref="SbusCode"/> for read operations
    /// </summary>
    /// <param name="type">The external type.</param>
    /// <returns>The internal frame type <see cref="SbusCode"/> used to read values.</returns>
    internal static SbusCode DataType2SbusCode4Read( Medium_T type )
    {
      SbusCode code = 0;
      switch ( type )
      {
        case Medium_T.Counter:
          code = SbusCode.coReadCounter;
          break;
        case Medium_T.Flag:
          code = SbusCode.coReadFlag;
          break;
        case Medium_T.Output:
          code = SbusCode.coReadOutput;
          break;
        case Medium_T.Input:
          code = SbusCode.coReadInput;
          break;
        case Medium_T.Register:
          code = SbusCode.coReadRegister;
          break;
        case Medium_T.Timer:
          code = SbusCode.coReadTimer;
          break;
        case Medium_T.Text:
          code = SbusCode.coReadText;
          break;
      }
      return code;
    }
    internal const byte ModemDescLength = 21;
    internal const byte CommandLength = 16;
    internal const byte PCD_NPORTS = 8;
    internal const byte WaitFEverTO = 5;
    internal const byte TOTRETRIES = 10;
  }
}

