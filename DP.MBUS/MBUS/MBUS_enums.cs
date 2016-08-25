//<summary>
//  Title   : MBUS_Exceptions
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//    20081008: mzbrzezny: MBUS not used class 1 data enums are turned of, item defaults are improved
//    20080519: mzbrzezny: created based on MBUS plugin
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE
{
  /// <summary>
  /// type of data analysis
  /// </summary>
  internal enum DataAnalysisMode
  {
    /// <summary>
    /// all received data are returned
    /// </summary>
    Full,
    /// <summary>
    /// only record values are returned
    /// </summary>
    Short
  }
  /// <summary>
  /// coeds returend by deposit character function
  /// </summary>
  internal enum DepCharacterTypeEnum
  {
    /// <summary>
    /// ordinary character is depisited
    /// </summary>
    DCT_Ordinary,
    /// <summary>
    /// start of header is depisited
    /// </summary>
    DCT_SOH,
    /// <summary>
    /// last characted is depisited
    /// </summary>
    DCT_Last,
    /// <summary>
    /// new frame is received or something wrong has occured, answer should be reset
    /// </summary>
    DCT_Reset_Answer
  }

  /// <summary>
  /// Control Codes of the M-Bus protocol
  /// </summary>
  internal enum MBUSControlCodes: byte
  {
    /// <summary>
    /// Telegram: Short Frame, Initialisation of Slave
    /// </summary>
    SND_NKE = 0x40,
    /// <summary>
    /// Telegram: Long/Control Frame,Send User Data to Slave
    /// </summary>
    SND_UD = 0x53,
    /// <summary>
    /// Telegram: Long/Control Frame,Send User Data to Slave
    /// </summary>
    SND_UD_FCBSet = 0x73,
    /// <summary>
    /// Request for Class 2 Data
    /// </summary>
    REQ_UD2 = 0x5b,
    /// <summary>
    /// Request for Class 2 Data FCB is set 
    /// </summary>
    REQ_UD2_FCBSet = 0x7b,
    /// <summary>
    /// Request for Class 1 Data
    ///  </summary>
    REQ_UD1 = 0x5A,
    /// <summary>
    /// Request for Class 1 Data FCB is set 
    /// </summary>
    REQ_UD1_FCBSet = 0x7A,
    /// <summary>
    /// Telegram: Long/Control Frame, DataTRansfer FromSlave To Master after Request
    /// </summary>
    RSP_UD = 0x08,
    /// <summary> 
    /// Telegram: Long/Control Frame, DataTRansfer FromSlave To Master after Request, ACD is set
    /// </summary>
    RSP_UD_ACDSet = 0x28,
    /// <summary>
    /// Telegram: Long/Control Frame, DataTRansfer FromSlave To Master after Request, DFC is Set
    /// </summary>
    RSP_UD_DFCSet = 0x18,
    /// <summary>
    /// Telegram: Long/Control Frame, DataTRansfer FromSlave To Master after Request, ACD is set, DFC is Set
    /// </summary>
    RSP_UD_ACDSet_DFCSet = 0x38
  }

  /// <summary>
  /// Constans that are used in mbus
  /// </summary>
  internal enum MBUSConstans: byte
  {
    /// <summary>
    /// Single character frame
    /// </summary>
    SingleCharacter = 0xe5,
    /// <summary>
    /// start byte in short frame
    /// </summary>
    StartShort = 0x10,
    /// <summary>
    /// start byte in control frame or long frame
    /// </summary>
    StartLong = 0x68,
    /// <summary>
    /// stop byte
    /// </summary>
    Stop = 0x16,
    /// <summary>
    /// Value of control frame L (length field)
    /// </summary>
    ControlFrameLField = 0x03
  }
  /// <summary>
  /// type of data that is requested
  /// </summary>
  internal enum MediumData: byte
  {
    /// <summary>
    /// user data class 2
    /// </summary>
    Class2_Data = 0,
    /// <summary>
    /// user data class 2 - return only values
    /// </summary>
    Class2_Data_Short = 1,
    ///// <summary>
    ///// user data class 1
    ///// </summary>
    //Class1_Data = 2,
    ///// <summary>
    ///// user data class 1 - return only values
    ///// </summary>
    //Class1_Data_Short = 3
  }
  /// <summary>
  /// mbus frame types
  /// </summary>
  internal enum MBusFrameTypes
  {
    /// <summary>
    /// single charater frame
    /// </summary>
    SingleCharacter,
    /// <summary>
    /// short  frame
    /// </summary>
    ShortFrame,
    /// <summary>
    /// control frame
    /// </summary>
    ControlFrame,
    /// <summary>
    /// long frame
    /// </summary>
    LongFrame,
    /// <summary>
    /// invalid frame
    /// </summary>
    Invalid
  }

}//namespace BaseStation.MBUS.PRIVATE
