//<summary>
//  Title   : MBUS message implementation
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//    20090119:  mschabowski: Assert that is checking this.WriteByte is fixed (the condition is now reverted)
//    20081008: mzbrzezny: MBUS not used class 1 data enums are turned of, item defaults are improved
//    20080904: mzbrzezny:  adaptation for new umessage that supports returning of information about status of write operation
//    20080812: mzbrzezny:  we accept any address if request was sent to bradcast address (254)
//    20080602: mzbrzezny:  fixed problem that occurs if STOP byte occurs in the middle of the frame;
//    20080519: mzbrzezny:  created based on MBUS plugin
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE.MBUSApplicationLayerMessage;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Processes;
using System;
using System.Diagnostics;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE
{
  /// <summary>
  /// MBUS message implementation
  /// </summary>
  internal class MBUS_message: ProtocolALMessage
  {
    #region Classes
    internal delegate void TRACE( TraceEventType EventType, int Identifier, string Message );
    private class MBUScrc
    {
      #region private
      private byte CRC;
      #endregion
      #region internal
      internal MBUScrc()
      {
        CRC = 0;
      }
      internal MBUScrc( byte initialvalue )
      {
        CRC = initialvalue;
      }
      internal void CRC_Calc( byte b )
      {
        CRC = (byte)( ( CRC + b ) & 0xFF );
      }
      internal byte CurrentCRC
      {
        get
        {
          return CRC;
        }
      }
      internal void clear()
      {
        CRC = 0;
      }
      internal void setcrc( byte crc )
      {
        CRC = crc;
      }
      internal bool equals( MBUScrc O )
      {
        if ( O.CRC == this.CRC )
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      #endregion
    }
    #endregion //classes
    #region PRIVATE
    private MBUScrc crc = new MBUScrc();
    private TRACE myTrace;
    private const byte ShortFrame_StationAddressPos = 2;
    private const byte LongFrame_Length1Pos = 1;
    private const byte LongFrame_Length2Pos = 2;
    private const byte LongFrame_MiddleStartPos = 3;
    private const byte LongFrame_ControlFieldPos = 4;
    private const byte LongFrame_StationAddressPos = 5;
    private const byte LongFrame_ControlInformationFieldPos = 6;
    private const byte LongFrame_StartUserDataPos = 7;
    private const byte ShortFrame_ControlFieldPos = 1;
    private const byte startbytepos = 0;
    private static Assertion myAssert = new Assertion( "myAssert.Assert error in MBUS_message", 200, false );
    private MBusFrameTypes expectedframetype = MBusFrameTypes.Invalid;
    private DataAnalysisMode expectedDataAnalysisMode = DataAnalysisMode.Full;
    private MBUSApplicationLayerMessageAnalyser MbusApplicationLayerMessageAnalyser = new MBUSApplicationLayerMessageAnalyser();
    private object ConvertFromCanonicalType( object source, Type type )
    {
      //TODO:Define of other conversion
      return source;
    }
    private object ConvertToCanonicalType( object source )
    {
      //TODO:Define of other conversion
      return source;
    }
    private void PutCRC()
    {
      myAssert.Assert( this.WriteByte( crc.CurrentCRC ), 113, "MBUS: unable to put CRC" );
    }
    private bool IsCRCGood()
    {
      throw new NotImplementedException();
    }
    private DepCharacterTypeEnum DepositeCharStartTest( byte ByteToDeposit, MBUSConstans StartByteTest )
    {
      if ( offset == 0 )
      {
        if ( ByteToDeposit == (byte)StartByteTest )
        {
          if ( !WriteByte( ByteToDeposit ) )
            return DepCharacterTypeEnum.DCT_Reset_Answer;
          return DepCharacterTypeEnum.DCT_SOH;
        }
        else
          return DepCharacterTypeEnum.DCT_Reset_Answer;
      }
      return DepCharacterTypeEnum.DCT_Ordinary;
    }
    private void MBUSTrace( TraceEventType EventType, int Identifier, string Message )
    {
      TraceEvent( EventType, Identifier, "MBUSmessage: " + Message );
    }
    #endregion
    #region ProtocolALMessage implementation
    public override object ReadValue( int regAddressOffset, Type pCanonicalType )
    {
      return ConvertFromCanonicalType( MbusApplicationLayerMessageAnalyser[ regAddressOffset ], pCanonicalType );
    }
    protected override object ReadCMD( int regAddressOffset ) //odpowiednik getvalue i taka powinien miec nazwe
    {
      throw new NotImplementedException( "protected override object ReadCMD( int regAddressOffset ) //odpowiednik getvalue i taka powinien miec nazwe" );

    }
    public override int GetCommand
    {
      get { throw ( new Exception( "The method or operation is not implemented." ) ); }
    }
    public override void WriteValue( object pValue, int pRegAddress )
    {
      throw new NotImplementedException( "Writing is not implemented" );
    }
    protected override void SetValue( object regValue, int regAddressOffset )
    {
      throw new NotImplementedException( "protected override void SetValue( object regValue, int regAddressOffset )" );
    }
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      crc.clear();
      userDataLength = 5;	// stala dlugosc zapytania
      this.startbyte = MBUSConstans.StartShort;
      this.ShortFrameStation = (byte)station;
      crc.CRC_Calc( (byte)station );
      switch ( (MediumData)block.dataType )
      {
        case MediumData.Class2_Data:
        case MediumData.Class2_Data_Short:
          ShortFrameControlField = MBUSControlCodes.REQ_UD2_FCBSet;
          crc.CRC_Calc( (byte)ShortFrameControlField );
          if ( (MediumData)block.dataType == MediumData.Class2_Data_Short )
            this.expectedDataAnalysisMode = DataAnalysisMode.Short;
          else
            this.expectedDataAnalysisMode = DataAnalysisMode.Full;
          break;
        //case MediumData.Class1_Data:
        //case MediumData.Class1_Data_Short:
        //  ShortFrameControlField = MBUSControlCodes.REQ_UD1_FCBSet;
        //  crc.CRC_Calc( (byte)ShortFrameControlField );
        //  if ( (MediumData)block.dataType == MediumData.Class1_Data_Short )
        //    this.expectedDataAnalysisMode = DataAnalysisMode.Short;
        //  else
        //    this.expectedDataAnalysisMode = DataAnalysisMode.Full;
        //  break;
        //TODO: dodac inne mozliwosci
        default:
          throw new NotImplementedException( "PrepareRequest is implemented only for REQ_UD2" );
      }
      offset = 3;
      PutCRC();
      myAssert.Assert( this.WriteByte( (byte)MBUSConstans.Stop ), 193, "Unable to put MBUSConstans.Stop in MBUS PrepareRequest" );
      MBUSTrace( TraceEventType.Verbose, 182, "PrepareRequest:" + DateTime.Now.ToString( "MM/dd/yyyy hh:mm:ss.fff tt" ) + this.ToString() );
    }
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      throw new NotImplementedException( "protected override void PrepareReqWriteValue( IBlockDescription block, int station )" );
    }
    /// <summary>
    /// ilosc rejestrow faktyczna 
    /// </summary>
    //internal enum CheckResponseResult { OK, CRCError, NAK, SynchError, Incomplete, Invalid };
    #endregion
    #region PUBLIC
    internal MBUSControlCodes ShortFrameControlField // function code
    {
      set
      {
        this[ ShortFrame_ControlFieldPos ] = (byte)value;
      }
      get
      {
        return (MBUSControlCodes)this[ ShortFrame_ControlFieldPos ];
      }
    }
    internal MBUSControlCodes LongFrameControlField // function code
    {
      set
      {
        this[ LongFrame_ControlFieldPos ] = (byte)value;
      }
      get
      {
        return (MBUSControlCodes)this[ LongFrame_ControlFieldPos ];
      }
    }
    internal byte ShortFrameStation // slave address
    {
      set
      {
        this[ ShortFrame_StationAddressPos ] = value;
      }
      get
      {
        return this[ ShortFrame_StationAddressPos ];
      }
    }
    internal byte LongFrameStation // slave address
    {
      set
      {
        this[ LongFrame_StationAddressPos ] = value;
      }
      get
      {
        return this[ LongFrame_StationAddressPos ];
      }
    }

    internal MBUSConstans startbyte // start byte 
    {
      set
      {
        this[ startbytepos ] = (byte)value;
      }
      get
      {
        return (MBUSConstans)this[ startbytepos ];
      }
    }
    internal short reqQuantity
    {
      set
      {
        offset = 4;
        myAssert.Assert( WriteInt16( value ), 268, "MBUS: Cannot write reqQuantity" );
      }
      get
      {
        offset = 4;
        return ReadInt16();
      }
    }
    internal DataAnalysisMode ExpectedDataAnalysisMode
    {
      get
      {
        return expectedDataAnalysisMode;
      }
    }
    public override CheckResponseResult CheckResponseFrame( ProtocolALMessage TransmitedMessage )
    {
      MBUS_message transmitedMbusMessage = (MBUS_message)TransmitedMessage;
      MBUSTrace( TraceEventType.Verbose, 266, "CheckResponseFrame:" + DateTime.Now.ToString( "MM/dd/yyyy hh:mm:ss.fff tt" ) + this.ToString() );
      switch ( expectedframetype )
      {
        case MBusFrameTypes.LongFrame:
          #region MBusFrameTypes.LongFrame
          crc.clear();
          // wyslana ramka typu short - powinnismy otrzymac odpowiedz dluga ramke typu UserData
          if ( this.userDataLength < 9 )
            return CheckResponseResult.CR_Incomplete;
          // sprawdzamy start 68h
          if ( this.startbyte != MBUSConstans.StartLong )
            return CheckResponseResult.CR_Invalid;
          //sprawdzamy dlugosc
          if ( this[ LongFrame_Length1Pos ] != this[ LongFrame_Length2Pos ] )
            return CheckResponseResult.CR_Invalid;
          int UserApplicationDataLength = this[ LongFrame_Length1Pos ] - 3;
          //sprawdzamy drugi start 68h
          if ( this[ LongFrame_MiddleStartPos ] != (byte)MBUSConstans.StartLong )
            return CheckResponseResult.CR_Invalid;
          //sprawdzamy code (function) field (C)
          if ( !( this[ LongFrame_ControlFieldPos ] == (byte)MBUSControlCodes.RSP_UD ||
            this[ LongFrame_ControlFieldPos ] == (byte)MBUSControlCodes.RSP_UD_ACDSet ||
            this[ LongFrame_ControlFieldPos ] == (byte)MBUSControlCodes.RSP_UD_ACDSet_DFCSet ||
            this[ LongFrame_ControlFieldPos ] == (byte)MBUSControlCodes.RSP_UD_DFCSet
            ) )
            return CheckResponseResult.CR_Invalid;
          crc.CRC_Calc( this[ LongFrame_ControlFieldPos ] );
          //sprawdzamy addesss field (A)
          if ( this.LongFrameStation != transmitedMbusMessage.ShortFrameStation && transmitedMbusMessage.ShortFrameStation != 254 )
          {
            //we accept any address if request was sent to bradcast address (254)
            return CheckResponseResult.CR_OtherStation;
          }
          crc.CRC_Calc( this.LongFrameStation );
          //sprawdzamy ControlInformation Field (CI)
          //MZNI: co zrobic z ControlInformation Field
          crc.CRC_Calc( this[ LongFrame_ControlInformationFieldPos ] );

          // teraz przegladamy userdata
          offset = LongFrame_StartUserDataPos;
          for ( int idx = 0; idx < UserApplicationDataLength; idx++ )
          // tutaj czytamy ramke - ale wazne jest ze nie mozemy przekroczycdlugosci calej ramki - 2 (na CRC +bajt stopu)
          {
            byte userbyte = ReadByte();
            crc.CRC_Calc( userbyte );
            if ( offset == userDataLength - 1 )
              return CheckResponseResult.CR_Incomplete;
          }
          //teraz sprawdzamy sume kontrolna i stop byte na koncu: 
          byte currentCRC = ReadByte();
          byte lastByte = ReadByte();
          //crc:
          if ( crc.CurrentCRC != currentCRC )
            return CheckResponseResult.CR_CRCError;
          //czy na koncu jest bajt stopu:
          if ( lastByte != (byte)MBUSConstans.Stop )
            return CheckResponseResult.CR_Invalid;
          // teraz robimy przetworzenie danych w application layer i zwracamy wynik
          return MbusApplicationLayerMessageAnalyser.AnalyseFrame( this, LongFrame_StartUserDataPos,
            transmitedMbusMessage.ExpectedDataAnalysisMode, myTrace );
          #endregion //MBusFrameTypes.LongFrame
        case MBusFrameTypes.ShortFrame:
        case MBusFrameTypes.ControlFrame:
        default:
          myAssert.Assert( false, 311 );
          break;
      }
      return CheckResponseResult.CR_Invalid;
    } //CheckResponseFrame
    internal void InitMsg( MBUS_message TransmitedMbusMessage )
    {
      userDataLength = userBuffLength;
      offset = 0;
      switch ( TransmitedMbusMessage.CheckFrameType() )
      {
        case MBusFrameTypes.ShortFrame:
          switch ( TransmitedMbusMessage.ShortFrameControlField )
          {
            case MBUSControlCodes.REQ_UD1:
            case MBUSControlCodes.REQ_UD1_FCBSet:
            case MBUSControlCodes.REQ_UD2:
            case MBUSControlCodes.REQ_UD2_FCBSet:
              expectedframetype = MBusFrameTypes.LongFrame;
              break;
            case MBUSControlCodes.SND_NKE:
              expectedframetype = MBusFrameTypes.SingleCharacter;
              break;
            default:
              myAssert.Assert( false, 449 );
              break;
          }
          break;
        case MBusFrameTypes.LongFrame:
          switch ( TransmitedMbusMessage.ShortFrameControlField )
          {
            case MBUSControlCodes.SND_UD:
            case MBUSControlCodes.SND_UD_FCBSet:
              expectedframetype = MBusFrameTypes.SingleCharacter;
              break;
            default:
              myAssert.Assert( false, 461 );
              break;
          }
          break;
        default:
          myAssert.Assert( false, 466 );
          break;
      }
      crc.clear();
    }
    internal MBusFrameTypes CheckFrameType()
    {
      if ( userDataLength == 1 && startbyte == MBUSConstans.SingleCharacter )
        return MBusFrameTypes.SingleCharacter;
      if ( userDataLength == 5 && startbyte == MBUSConstans.StartShort )
        return MBusFrameTypes.ShortFrame;
      if ( userDataLength == 9 && startbyte == MBUSConstans.StartLong
        && this[ LongFrame_Length1Pos ] == (byte)MBUSConstans.ControlFrameLField )
        return MBusFrameTypes.ControlFrame;
      if ( userDataLength >= 9 && startbyte == MBUSConstans.StartLong
        && this[ LongFrame_Length1Pos ] != (byte)MBUSConstans.ControlFrameLField )
        return MBusFrameTypes.LongFrame;

      return MBusFrameTypes.Invalid;
    }
    internal DepCharacterTypeEnum DepositeChar( byte ByteToDeposit )
    {
      switch ( expectedframetype )
      {
        case MBusFrameTypes.SingleCharacter:
          if ( offset == 0 )
            if ( !WriteByte( ByteToDeposit ) )
              return DepCharacterTypeEnum.DCT_Reset_Answer;
          this.userDataLength = (ushort)( offset + 0 );
          return DepCharacterTypeEnum.DCT_Last;
        case MBusFrameTypes.ShortFrame:
          DepCharacterTypeEnum AfterStartTest = DepositeCharStartTest( ByteToDeposit, MBUSConstans.StartShort );
          if ( AfterStartTest != DepCharacterTypeEnum.DCT_Ordinary )
            return AfterStartTest;
          if ( !WriteByte( ByteToDeposit ) )
            return DepCharacterTypeEnum.DCT_Reset_Answer;
          if ( ByteToDeposit == (byte)MBUSConstans.Stop && offset >= 4 )
          {
            this.userDataLength = (ushort)( offset + 0 );
            return DepCharacterTypeEnum.DCT_Last;
          }
          return DepCharacterTypeEnum.DCT_Ordinary;
        case MBusFrameTypes.LongFrame:
          DepCharacterTypeEnum AfterStartLongTest = DepositeCharStartTest( ByteToDeposit, MBUSConstans.StartLong );
          if ( AfterStartLongTest != DepCharacterTypeEnum.DCT_Ordinary )
            return AfterStartLongTest;
          if ( !WriteByte( ByteToDeposit ) )
            return DepCharacterTypeEnum.DCT_Reset_Answer;
          if ( ByteToDeposit == (byte)MBUSConstans.Stop && offset >= 6 + this[ LongFrame_Length1Pos ] )
          {
            this.userDataLength = (ushort)( offset + 0 );
            return DepCharacterTypeEnum.DCT_Last;
          }
          return DepCharacterTypeEnum.DCT_Ordinary;
        default:
          return DepCharacterTypeEnum.DCT_Reset_Answer;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MBUS_message"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    internal MBUS_message( IBufferLink homePool )
      : base( 300, homePool, true )
    {
      myTrace = new TRACE( MBUSTrace );
    }
    #endregion
  }
}
