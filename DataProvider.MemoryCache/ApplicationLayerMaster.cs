//<summary>
//  Title   : DemoSimulator implementation of IApplicationLayerMaster
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081022: mzbrzezny: stopwatches and time measurement is added
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47 in write
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{
  /// <summary>
  ///  DemoSimulator implementation of the IApplicationLayerMaster
  /// </summary>
  internal class ApplicationLayerMaster: ApplicationLayerCommon, IApplicationLayerMaster
  {
    #region PRIVATE
    private System.Diagnostics.Stopwatch InterFrameStopwatch = new System.Diagnostics.Stopwatch();
    /// <summary>
    ///  Title   : NULL implementation of IApplicationLayerMaster
    /// </summary>
    private class NULL_buf_pool: CommunicationLayer.Generic.SesDBufferPool<Message>
    {
      protected override Message CreateISesDBuffer()
      {
        return new Message( this );
      }
    }//NULL_buf_pool
    private NULL_buf_pool pool = new NULL_buf_pool();
    private int m_errorfrequency = 100;
    private ulong m_rPackNum = 0;
    private ulong m_wPackNum = 0;
    private CAS.Lib.RTLib.Management.IProtocolParent myStatistic;
    #endregion
    #region IApplicationLayerMaster
    /// <summary>
    /// Read Data
    /// </summary>
    /// <param name="block">Block description to be read</param>
    /// <param name="station">Address of the remote station connected to the common field bus. –1 if not applicable.
    /// </param>
    /// <param name="data">The buffer with the requested data.</param>
    /// <param name="retries">Number of retries to get data.</param>
    /// <returns>Result of the operation</returns>
    AL_ReadData_Result IApplicationLayerMaster.ReadData( IBlockDescription block, int station, out IReadValue data, byte retries )
    {
      lock ( this )
      {
        InterFrameStopwatch.Reset();
        InterFrameStopwatch.Start();
        m_rPackNum++;
        data = null;
        bool TransmissionIsOK = true;
        data = (IReadValue)pool.GetEmptyISesDBuffer();
        //informacja ze dziala transmitter:
        ( (Message)data ).TransmitterON( station );
        //Timer.Wait( Timer.TInOneSecond );
        ( (Message)data ).SetBlockDescription( station, block );
        ( (Message)data ).ReadFromDB();
        myStatistic.IncStTxFrameCounter();
        myStatistic.IncStRxFrameCounter();
        myStatistic.TimeCharGapAdd( 1 );
        myStatistic.TimeMaxResponseDelayAdd( InterFrameStopwatch.ElapsedMilliseconds );
        ( (Message)data ).TransmitterOFF( station );
        if ( m_errorfrequency > 0 && ( m_rPackNum % ( 100 / (ulong)m_errorfrequency ) ) == 0 )
        {
          TransmissionIsOK = false;
        }
        if ( TransmissionIsOK && ( (Message)data ).TestCommunication( station ) )
        {
          myStatistic.RxDataBlock( true );
          return AL_ReadData_Result.ALRes_Success;

        }
        else
        {
          myStatistic.RxDataBlock( false );
          data.ReturnEmptyEnvelope();
          data = null;
          return AL_ReadData_Result.ALRes_DatTransferErrr;
        }
      }
    }
    /// <summary>
    /// Gets a buffer from a pool and initiates it. After filling it up with the data can be send to the data provider remote 
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
        Message data = (Message)pool.GetEmptyISesDBuffer();
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
      lock ( this )
      {
        InterFrameStopwatch.Reset();
        InterFrameStopwatch.Start();
        m_wPackNum++;
        myStatistic.IncStTxFrameCounter();
        if ( ( m_errorfrequency > 0 ) && ( m_rPackNum % ( 100 / (ulong)m_errorfrequency ) ) == 0 )
        {
          data.ReturnEmptyEnvelope();
          data = null;
          myStatistic.IncStRxFragmentedCounter();
          myStatistic.TxDataBlock( false );
          return AL_ReadData_Result.ALRes_DatTransferErrr;
        }
        ( (Message)data ).WriteToDB();
        data.ReturnEmptyEnvelope();
        data = null;
        myStatistic.IncStTxFrameCounter();
        myStatistic.IncStRxFrameCounter();
        myStatistic.TxDataBlock( true );
        myStatistic.TimeMaxResponseDelayAdd( InterFrameStopwatch.ElapsedMilliseconds );
        return AL_ReadData_Result.ALRes_Success;
      }
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
    internal ApplicationLayerMaster
    ( IProtocolParent pStatistic, ICommunicationLayer pComm, int pErrorFrequency )
      : base( pComm )
    {
      this.myStatistic = pStatistic;
      m_errorfrequency = pErrorFrequency;
    }
    #endregion
  }
}
