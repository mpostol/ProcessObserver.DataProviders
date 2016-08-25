//<summary>
//  Title   : SBus implementation - slave side
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//
//    MZbrzezny - 29-07-04:
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
  using CommunicationLayer.Generic;
  /// <summary>
  /// Summary description for SBUS_ApplicationLayer.
  /// </summary>
  internal class SBUS_ApplicationLayerSlave: ApplicationLayerCommon, IApplicationLayerSlave
  {
    #region PRIVATE
    private SBUSProtocol m_ALProtocol;
    private SesDBufferPool<SBUSbase_message> m_Pool;
    #endregion
    #region IApplicationLayerSlave
    /// <summary>
    /// Read command from a master station.
    /// </summary>
    /// <param name="frame">Received frame</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result IApplicationLayerSlave.ReadCMD( out IReadCMDValue frame )
    {
      return m_ALProtocol.ReadCMD( out frame );
    }
    AL_ReadData_Result IApplicationLayerSlave.SendNAKRes()
    {
      SBUSbase_message frame = m_Pool.GetEmptyISesDBuffer();
      frame.PrepareResultFrame( 1 );
      AL_ReadData_Result res = m_ALProtocol.TransmitMessage( frame, FrameType.FT_RES_ANK );
      frame.ReturnEmptyEnvelope();
      m_ALProtocol.GetIProtocolParent.IncStTxNAKCounter();
      return res;
    }
    AL_ReadData_Result IApplicationLayerSlave.SendACKRes()
    {
      SBUSbase_message frame = m_Pool.GetEmptyISesDBuffer();
      frame.PrepareResultFrame( 0 );
      AL_ReadData_Result res = m_ALProtocol.TransmitMessage( frame, FrameType.FT_RES_ANK );
      frame.ReturnEmptyEnvelope();
      m_ALProtocol.GetIProtocolParent.IncStTxACKCounter();
      return res;
    }
    IResponseValue IApplicationLayerSlave.GetEmptySendDataBuffor
      ( IBlockDescription block, int station )
    {
      SBUSbase_message frame = m_Pool.GetEmptyISesDBuffer();
      frame.PrepareDataResponse( block );
      return frame;
    }
    AL_ReadData_Result IApplicationLayerSlave.SendData( IResponseValue data )
    {
      m_ALProtocol.GetIProtocolParent.IncStTxDATACounter();
      return m_ALProtocol.TransmitMessage( (SBUSbase_message)data, FrameType.FT_RES_DAT );
    }
    #endregion
    #region INIT
    internal SBUS_ApplicationLayerSlave
      ( SBUSProtocol cALProtocol, SesDBufferPool<SBUSbase_message> cPool )
      :
      base( cALProtocol.GetICommunicationLayer )
    {
      m_ALProtocol = cALProtocol;
      m_Pool = cPool;
    }
    #endregion
  }
}
