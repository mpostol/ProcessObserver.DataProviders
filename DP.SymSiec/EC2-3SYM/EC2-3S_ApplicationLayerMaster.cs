//<summary>
//  Title   : NULL implementation of IApplicationLayerMaster
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080904: mzbrzezny: namespaces cleanup
//    mzbrzezny 20070521: adaptation to new interface
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer  - nalezy skonczyc to i zrobic porzadek!!!
//    MZbrzezny - 03-06-2005: podano obsluge pluginow
//    MPostol - 24-10-2003: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM
{
  /// <summary>
  ///  NULL implementation of the IApplicationLayerMaster
  /// </summary>
  internal class NULL_ApplicationLayerMaster: ApplicationLayerCommon, IApplicationLayerMaster
  {
    #region PRIVATE
    /// <summary>
    ///  Title   : NULL implementation of IApplicationLayerMaster
    /// </summary>
    private class NULL_buf_pool: CommunicationLayer.Generic.SesDBufferPool<NULL_message>
    {
      protected override NULL_message CreateISesDBuffer()
      {
        return new NULL_message( this );
      }
    }//NULL_buf_pool
    private NULL_buf_pool pool = new NULL_buf_pool();
    private static ulong rPackNum = 0;
    private IProtocolParent myStatistic;
    #endregion
    #region IApplicationLayerMaster
    /// <summary>
    /// Read Data
    /// </summary>
    /// <param name="block">Data block description to be read</param>
    /// <param name="station">Address of the remote station connected to the common field bus. –1 if not applicable.
    /// </param>
    /// <param name="data">The buffer with the requested data.</param>
    /// <param name="retries">Number of retries to get data.</param>
    /// <returns>Result of the operation</returns>
    AL_ReadData_Result IApplicationLayerMaster.ReadData( IBlockDescription block, int station, out IReadValue data, byte retries )
    {
      lock ( this )
      {
        rPackNum++;
        data = null;
        data = (IReadValue)pool.GetEmptyISesDBuffer();
        System.Threading.Thread.Sleep( TimeSpan.FromSeconds( 1 ) );
        ( (NULL_message)data ).SetBlockDescription( station, block );
        ( (NULL_message)data ).ReadFromDB();
        myStatistic.IncStTxFrameCounter();
        myStatistic.IncStRxFrameCounter();
        myStatistic.RxDataBlock( true );
        return AL_ReadData_Result.ALRes_Success;
      }
    }
    /// <summary>
    /// Gets a buffer from a pool and initiates. After filling it up with the data can be send to the data provider remote 
    /// unit by the WriteData.
    /// </summary>
    /// <param name="block">Data description allowing to prepare appropriate header of the frame.</param>
    /// <param name="station">Address of the remote station connected to the common field bus. –1 if not applicable.
    /// </param>
    /// <returns>A buffer ready to be filled up with the data and write down to the destination – remote station.
    /// </returns>
    IWriteValue IApplicationLayerMaster.GetEmptyWriteDataBuffor( IBlockDescription block, int station )
    {
      lock ( this )
      {
        NULL_message data = (NULL_message)pool.GetEmptyISesDBuffer();
        data.SetBlockDescription( station, block );
        return (IWriteValue)data;
      }
    }
    /// <summary>
    /// Send values to the data provider
    /// </summary>
    /// <param name="data">
    /// Data to be send. Always null after return. Data buffer must be returned to the pool.
    /// </param>
    /// <param name="retries">Number of retries to wrtie data.</param>
    /// <returns>Result of the operation.</returns>
    AL_ReadData_Result IApplicationLayerMaster.WriteData( ref IWriteValue data, byte retries )
    {
      ( (NULL_message)data ).WriteToDB();
      data.ReturnEmptyEnvelope();
      myStatistic.IncStTxFrameCounter();
      myStatistic.IncStRxFrameCounter();
      myStatistic.TxDataBlock( true );
      return AL_ReadData_Result.ALRes_Success;
    }
    #endregion
    #region IDisposable Members
    void IDisposable.Dispose()
    {
      Dispose( true );
    }
    #endregion
    #region init
    /// <summary>
    /// contructor
    /// </summary>
    /// <param name="pStatistic">out statistic class</param>
    /// <param name="pComm"></param>
    /// <param name="pErrorFrequency"></param>
    internal NULL_ApplicationLayerMaster
    ( IProtocolParent pStatistic, ICommunicationLayer pComm, int pErrorFrequency )
      : base( pComm )
    {
      this.myStatistic = pStatistic;
      //m_errorfrequency = pErrorFrequency;
    }
    #endregion
  }
}
