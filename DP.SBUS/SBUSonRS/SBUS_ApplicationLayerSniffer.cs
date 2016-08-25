//<summary>
//  Title   : SBus implementation - sniffer side
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol 02-04-2007:
//      dostosowa³em do po³aczeniowych CommunicationLayer, ale nie uwzgledni³em rozlaczania w warstwie ni¿szej. 
//      Wymaga jeszcze dopasowania.
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//
//    MZbrzezny - 29-07-05:
//     module creation
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS
{
  using System;
  using PRIVATE;
  using CommunicationLayer;
  using ApplicationLayer;
  using CommonBus;
  using CommunicationLayer.Generic;
  /// <summary>
  /// Summary description for SBUS_ApplicationLayer.
  /// </summary>
  internal class SBUS_ApplicationLayerSniffer: ApplicationLayerCommon, IApplicationLayerSniffer
  {
    #region PRIVATE
    private SBUSProtocol m_ALProtocol;
    private const string m_AssertText = "Assert error in SBUS.SBUS_ApplicationLayerSniffer";
    private const int m_AssertID = (int)CAS.Lib.RTLib.Processes.Error.ApplicationLayer_SBUS_SBUS_ApplicationLayerSlave;
    private AL_ReadData_Result GetFrame
      ( out SBUSbase_message Rxmsg, SBUSbase_message txmsg, bool infinitewait, ref bool reset )
    {
      return m_ALProtocol.GetMessage( out Rxmsg, txmsg, infinitewait, 0, ref reset );
    }//ReadData
    #endregion
    #region IApplicationLayerSniffer
    /// <summary>
    /// Interface of the passive reader station, i.e. responsible for reading master station commands and slave station responses 
    /// and gathering the data from coupled pairs.
    /// </summary>
    /// <param name="block">Data block description.</param>
    /// <param name="data">Message containing obtained data.</param>
    /// <returns>
    ///   ALRes_Success: 
    ///     Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result IApplicationLayerSniffer.ReadData( out IBlockDescription block, out IReadValue data )
    {
      bool resetanswer = false;
      SBUSbase_message SBUS_Frame1, SBUS_Frame2;
      AL_ReadData_Result res = GetFrame( out SBUS_Frame1, null, true, ref resetanswer );
      //otrzymalismy polecenie zapisu lub odczytu
      if ( res == AL_ReadData_Result.ALRes_Success )
      {
        do
        {
          resetanswer = false;
          //TODO it is protected and must be used as an inherited member SBUS_Frame1.CalculateExpectedResulLength();
          switch ( SBUS_Frame1.cmd )
          {
            case SBUSbase_message.ProtocolCmd.coRS0:
            case SBUSbase_message.ProtocolCmd.coRS1:
            case SBUSbase_message.ProtocolCmd.coRS2:
            case SBUSbase_message.ProtocolCmd.coRS3:
            case SBUSbase_message.ProtocolCmd.coRS4:
            case SBUSbase_message.ProtocolCmd.coRS5:
            case SBUSbase_message.ProtocolCmd.coRS6:
            case SBUSbase_message.ProtocolCmd.coRS7:
            case SBUSbase_message.ProtocolCmd.coRR:
            case SBUSbase_message.ProtocolCmd.coRF:
              #region read
              if ( GetFrame( out SBUS_Frame2, SBUS_Frame1, false, ref resetanswer ) == AL_ReadData_Result.ALRes_Success )
              {
                if ( !resetanswer )
                {
                  // odczytywanie danych - wiec pierwsza przechwycona ramka  zawiera opis bloku,
                  // a druga zawiera wlasciwe dane
                  block = SBUS_Frame1;
                  data = SBUS_Frame2;
                  m_ALProtocol.GetIProtocolParent.RxDataBlock( true );
                  return AL_ReadData_Result.ALRes_Success;
                }
                else
                  SBUS_Frame1 = SBUS_Frame2;
              }
              m_ALProtocol.GetIProtocolParent.RxDataBlock( false );
              #endregion
              break;
            case SBUSbase_message.ProtocolCmd.coWR:
            case SBUSbase_message.ProtocolCmd.coWF:
              #region write
              if ( GetFrame( out SBUS_Frame2, SBUS_Frame1, false, ref resetanswer ) == AL_ReadData_Result.ALRes_Success )
              {
                if ( !resetanswer )
                {
                  // zapisywanie danych - wiec pierwsza przechwycona ramka zawiera opis bloku+dane,
                  // a druga zawiera potwierdzenie czy jest OK
                  //jesli w ostatnim IF bylo OK - tzn - ze dostalismy ACK:
                  //wiec :
                  block = SBUS_Frame1;
                  SBUS_Frame2.PrepareDataResponse( block );
                  for ( int idx = 0; idx < block.length; idx++ )
                  {
                    ( (IResponseValue)( SBUS_Frame2 ) )[ idx ] = ( (IReadCMDValue)SBUS_Frame1 )[ idx ];
                  }
                  data = SBUS_Frame2;
                  m_ALProtocol.GetIProtocolParent.TxDataBlock( true );
                  return AL_ReadData_Result.ALRes_Success;
                }
                else
                  SBUS_Frame1 = SBUS_Frame2;
              }
              m_ALProtocol.GetIProtocolParent.TxDataBlock( false );
              #endregion
              break;
            default:
              break;
          }
        } while ( resetanswer );
      }
      block = null;
      data = null;
      return AL_ReadData_Result.ALRes_DatTransferErrr;
    }
    #endregion
    #region creator
    internal SBUS_ApplicationLayerSniffer( ICommunicationLayer commChannel, SBUSProtocol pProtocol )
      : base( commChannel )
    {
      m_ALProtocol = pProtocol;
    }
    #endregion
  }
}
