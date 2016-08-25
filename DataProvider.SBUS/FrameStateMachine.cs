//<summary>
//  Title   : SBUS message implementation supporting receive/transmit state machine
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20090123: mzbrzezny: assertion is now configured to restart the computer, 
//              read value is throwing an exception when problem with bad fram occurs
//              some new tracing messages
//    20081008: mabrzezny: item defaults are improved, conversion to unsigned is added
//    20080904: mzbrzezny: adaptation for new umessage that supports returning of information about status of write operation
//    Maciej Zbrzezny - 12-04-2006
//    zmienilem ze dostep do typu ramki jest typu internal
//	  MZbrzezny - 2005-12-16
//	  created from SBUSMessage (the oryginal class was divided into 2 parts)
//    MZbrzezny - 29-07-04:
//     module creation
//    20130330: Text implementation
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE
{

  /// <summary>
  /// Sbus message implementation supporting receive/transmit state machine.
  /// </summary>
  internal abstract class FrameStateMachine: FrameContent
  {
    #region ProtocolALMessage
    /// <summary>
    /// Checks if the frame is exactly what we are looking for.
    /// </summary>
    /// <param name="txmsg">Sent frame as request  we expect answer to this frame.</param>
    /// <returns>
    /// CR_OK: OK
    /// CR_CRCError: CRC Error
    /// CR_NAK: Negative acknowledge.
    /// CR_SynchError: Synchronization error.
    /// CR_Incomplete: Incomplete frame.
    /// CR_Invalid: Invalid frame
    /// CR_OtherStation: Answer is from unexpected station.
    /// </returns>
    public override CheckResponseResult CheckResponseFrame( ProtocolALMessage txmsg )
    {
      if ( rxMsgCurrError != CheckResponseResult.CR_OK )
      {
        TraceEvent( System.Diagnostics.TraceEventType.Verbose, 64,
          "CheckResponseFrame has found error that  occured during receive, expected: " + rxMsgCurrError.ToString() );
        return rxMsgCurrError;
      }
      if ( Part != FramePart.FrameEnd )
      {
        TraceEvent( System.Diagnostics.TraceEventType.Verbose, 70,
          "CheckResponseFrame frame is incompleted " );
        return CheckResponseResult.CR_Incomplete;
      }
      if ( this.FrameAttribute == AttributeCharacter.ResponseACK_NAK && NakCode != 0 )
      {
        TraceEvent( System.Diagnostics.TraceEventType.Verbose, 39, String.Format( "Received NAC response with code: {0}", NakCode ) );
        return CheckResponseResult.CR_NAK;
      }
      if ( this.expectedFrameAttribute != this.FrameAttribute )
      {
        TraceEvent( System.Diagnostics.TraceEventType.Verbose, 76,
          "CheckResponseFrame: CR_Invalid, expected: " + this.expectedFrameAttribute.ToString() + " received: " + this.FrameAttribute.ToString() );
        return CheckResponseResult.CR_Invalid;
      }
      return CheckResponseResult.CR_OK;
    }//CheckResponseFrame
    #endregion

    #region PUBLIC

    #region Receiver
    internal virtual void InitMsg( FrameStateMachine request )
    {
      if ( request == null )
        throw new ArgumentNullException( "txmsg", "Message cannot be null" );
      userDataLength = userBuffLength;
      FrameAttribute = AttributeCharacter.ResponseData; //To pass unit test: ResponseWriteValue
      expectedFrameAttribute = request.ExpectedResultFrameType();
      DataType = request.DataType;
      userDataLength = request.FrameResponseLength();
      offset = 0;
      Count = 0;
      Rxdcrc = 0;
      Part = FramePart.Head;
      rxMsgCurrError = CheckResponseResult.CR_OK;
      crc.clear();
    }
    /// <summary>
    /// enum used by DepositeChar function to indicate received character
    /// </summary>
    internal enum DepCharacterTypeEnum { DCT_Ordinary, DCT_SOH, DCT_Last, DCT_Reset_Answer }
    /// <summary>
    /// DepositeChar - this function stores the carracter in frame 
    /// </summary>
    /// <param name="lastChr">character to deposite</param>
    /// <returns>type of received character</returns>
    internal virtual DepCharacterTypeEnum DepositeChar( byte lastChr )
    {
      try
      {
        switch ( Part )
        {
          case FramePart.Head:
            if ( !ReadHead( lastChr ) )
              return DepCharacterTypeEnum.DCT_Ordinary;
            Part = FramePart.Attribute;
            return DepCharacterTypeEnum.DCT_SOH;
          case FramePart.Attribute:
            //odczytujemy atrybut i w zaleznosci od tego odczytuejmy dalsze dane
            #region FramePart.Attribute
            WriteByte( lastChr );
            switch ( (AttributeCharacter)lastChr )
            {
              case AttributeCharacter.Telegram:
                Part = FramePart.Telegram;
                break;
              case AttributeCharacter.ResponseData:
                if ( this.expectedFrameAttribute != AttributeCharacter.ResponseData )
                  rxMsgCurrError = CheckResponseResult.CR_SynchError;
                Part = FramePart.Response;
                break;
              case AttributeCharacter.ResponseACK_NAK:
                userDataLength = ACKNAKFrameLength;
                Part = FramePart.Acknowledge;
                break;
              default:
                rxMsgCurrError = CheckResponseResult.CR_Invalid;
                break;
            }//switch (lastChr)
            break;
            #endregion FramePart.Attribute
          case FramePart.Response:
            //odczytujemy odpowiedz tak dlugo jak dluga jest
            WriteByte( lastChr );
            if ( offset == userDataLength )
              Part = FramePart.CRC_1;
            break;
          case FramePart.Telegram:
            throw new ApplicationLayerInterfaceNotImplementedException(); //Slave is not implemented 
          case FramePart.Acknowledge:
            WriteByte( lastChr );
            if ( Count == 0 )
              Count++;
            else if ( Count == 1 )
              Part = FramePart.CRC_1;
            break;
          case FramePart.CRC_1:
            Rxdcrc = (ushort)( lastChr << 8 );
            Part = FramePart.CRC_2;
            break;
          case FramePart.CRC_2:
            Part = FramePart.FrameEnd;
            Rxdcrc = (ushort)( Rxdcrc | (ushort)lastChr );
            if ( Rxdcrc == crc.getCrc() )
              return DepCharacterTypeEnum.DCT_Last;
            else
            {
              rxMsgCurrError = CheckResponseResult.CR_CRCError;
              return DepCharacterTypeEnum.DCT_Last; //tutaj wazne jest by zwrocic ze to ostatni znak - a check response sprawdzic ze jest to zla ramka
            }
          default:
            break;
        }//switch (Part)
        return DepCharacterTypeEnum.DCT_Ordinary;
      }
      catch ( WriteException )
      {
        return DepCharacterTypeEnum.DCT_Reset_Answer;
      }
    }
    #endregion

    #region transmitter
    /// <summary>
    /// Prepares the frame to be send, i.e. copy the frame from <paramref name="sourceFrame"/> and adds crc and escape sequence.
    /// </summary>
    /// <param name="sourceFrame">To be send frame.</param>
    internal void PrepareFrameToBeSend( FrameStateMachine sourceFrame )
    {
      int length = sourceFrame.userDataLength;
      offset = 0;
      userDataLength = userBuffLength;
      crc.clear();
      byte ch = 0;
      for ( int idx = 0; idx < length; idx++ )
      {
        ch = sourceFrame[ idx ];
        CopyNextByte( ch );
      }
      ushort Crc = (ushort)crc.getCrc();
      WriteCRC( Crc );
      userDataLength = (ushort)( offset );
    }
    #endregion

    #endregion

    #region private

    #region protected.crc
    /// <summary>
    /// class responsible for computing CRC x16 + x12 + x5 + 1 generator polynomial 
    /// </summary>
    protected class Scomcrc
    {
      #region static
      private static int poly = 0x1021;
      private static int[] crcTable = new int[ 256 ];
      private static int crc_calc( int b )
      {
        int crc = b << 8;
        for ( int i = 0; i < 8; i++ )
        {
          if ( ( crc & 0x8000 ) > 0 )
          {
            crc = ( ( crc << 1 ) ^ poly ) & 0xffff;
          }
          else
          {
            crc = ( crc << 1 ) & 0xffff;
          }
        }
        return crc;
      }
      static Scomcrc()
      {
        for ( int i = 0; i < 256; i++ )
        {
          crcTable[ i ] = crc_calc( i );
        }
      }
      #endregion static
      #region private
      private int CRC;
      #endregion
      #region internal
      internal Scomcrc()
      {
        CRC = 0;
      }
      internal void CRC_Calc( byte b )
      {
        CRC = ( crcTable[ ( ( CRC >> 8 ) ^ b ) < 0 ? ( ( CRC >> 8 ) ^ b ) + 256 :
          ( CRC >> 8 ) ^ b ] ^ ( CRC << 8 ) ) & 0xffff;
      }
      internal int getCrc()
      {
        return CRC;
      }
      internal void clear()
      {
        CRC = 0;
      }
      internal byte getHighCrc()
      {
        return (byte)( CRC >> 8 );
      }
      internal byte getLowCrc()
      {
        return (byte)( CRC & 0xff );
      }
      internal void setHighCrc( int high )
      {
        int tmp = high << 8;
        CRC = CRC & 0x00ff;
        CRC = CRC | tmp;
      }
      internal void setLowCrc( int low )
      {
        CRC = CRC & 0xff00;
        CRC = CRC | low;
      }
      internal bool equals( Scomcrc O )
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
    /// <summary>
    /// used for computing crc of the frame
    /// </summary>
    protected Scomcrc crc = new Scomcrc();
    #endregion

    #region Rxmsg analize
    protected ushort Count = 0;
    protected ushort Rxdcrc = 0;
    protected FramePart Part { get; private set; }
    private AttributeCharacter expectedFrameAttribute;
    protected CheckResponseResult rxMsgCurrError;
    #endregion

    /// <summary>
    /// Reads the head of the frame.
    /// </summary>
    /// <param name="lastChr">The last CHR.</param>
    /// <returns></returns>
    protected abstract bool ReadHead( byte lastChr );
    /// <summary>
    /// Writes the CRC.
    /// </summary>
    /// <param name="Crc">The CRC.</param>
    protected abstract void WriteCRC( ushort Crc );
    /// <summary>
    /// Writes the <paramref name="val" /> into the frame and adds escape sequence if required.
    /// </summary>
    /// <param name="val">The val.</param>
    protected abstract void CopyNextByte( byte val );
    #endregion

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="FrameStateMachine" /> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    /// <param name="fspart_length">The FS part length.</param>
    internal FrameStateMachine( IBufferLink homePool, byte fspart_length )
      : base( homePool, fspart_length )
    {
      m_traceName = "SBUSbase_message";
    }
    #endregion

  }

}
