//<summary>
//  Title   : NULL implementation of IApplicationLayerMaster
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080904: mzbrzezny: fixed: ReadData and WriteData always increment the transmited frame counter (nevertheless the read or write ends with success) 
//    20080902: mzbrzezny: fixed: empty data block must be returned when AL_ReadData_Result.ALRes_DatTransferEr in write
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47 in write
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.RTLib.Management;
using NULLTraceSource = CAS.Lib.RTLib.Processes.TraceEvent;
using System.Collections.Generic;
using System.Threading;


namespace CAS.Lib.CommonBus.ApplicationLayer.NULL
{
  /// <summary>
  ///  NULL implementation of the IApplicationLayerMaster
  /// </summary>
  internal class NULL_ApplicationLayerMaster: ApplicationLayerCommon, IApplicationLayerMaster
  {
    #region PRIVATE
    #region loopback communication
    ICommunicationLayer ComPortCli;
    private System.Random rnd = new Random();
    enum State { transmiting, receiving, gettingChar, leaving, error };
    private const int c_FrameTimeout = 100; // [ms]
    private const int c_CharTimeout = 50; //ms
    private const int c_WaitAfterAndBeforeCommunication = 100; //ms
    private byte[] get_buffor()
    {
      byte[] buff = new byte[ rnd.Next( 200 ) + 1 ];
      int idx = 0;
      while ( idx < buff.Length )
        buff[ idx++ ] = (byte)( rnd.Next( 254 ) + 1 ); // nie chcemy dla testow przesylac 0
      return buff;
    }
    private TFrameEndSignalRes TransmitFrame( out byte[] buff, ICommunicationLayer ComPortCli )
    {
      buff = get_buffor();
      NULL_message message = pool.GetEmptyISesDBuffer();
      message.userDataLength = (ushort)buff.Length;
      for ( int i = 0; i < buff.Length; i++ )
        message.WriteByte( buff[ i ] );
      // czas na wyslanie ramki:
      ComPortCli.Flush();
      string msg = "sending frame: " + buff.Length.ToString() + " size";
      m_TraceSource.TraceVerbose( 382, "TransmitFrame", msg );
      TFrameEndSignalRes lastRes = ComPortCli.FrameEndSignal( message );
      m_TraceSource.TraceVerbose( 382, "TransmitFrame", "Frame has been sent" );
      message.ReturnEmptyEnvelope();
      return lastRes;
    }
    private bool CheckData( byte[] buff, List<byte> receivebuff )
    {
      bool ret = true;
      string check_length_result = "";
      if ( buff.Length == receivebuff.Count )
        check_length_result = " Dlugosc otrzymanej ramki ok. ";
      else
      {
        ret = false;
        check_length_result = " Brak zgodnosci dlugosci. ";
      }
      int lenghtocompare = Math.Min( buff.Length, receivebuff.Count );
      string msg = "CheckData: sent:" + buff.Length.ToString() + " receive:" + receivebuff.Count.ToString()
        + " tocompare:" + lenghtocompare.ToString() + check_length_result;
      if ( ret )
        m_TraceSource.TraceVerbose( 318, "CheckData", msg );
      else
        m_TraceSource.TraceInformation( 321, "CheckData", msg );
      string comparationresulst = "";
      int bytes_ok = 0;
      for ( int idx = 0; idx < lenghtocompare; idx++ )
      {
        //comparationresulst += "[idx=" + idx.ToString( "D3" ) +
        //  "] (" + buff[ idx ].ToString( "D3" ) + "=" + buff[ idx ].ToString( "X2" ) +
        //  "," + receivebuff[ idx ].ToString( "D3" ) + "=" +
        //  receivebuff[ idx ].ToString( "X2" ) + ")";
        if ( buff[ idx ] == receivebuff[ idx ] )
        {
          //comparationresulst += "OK";
          bytes_ok++;
        }
        else
        {
          ret = false;
          //comparationresulst += "BAD";
        }
        //comparationresulst += "        \r\n";
      }
      if ( bytes_ok == lenghtocompare && buff.Length == receivebuff.Count )
      {
        msg = "!!! Frame ok.";
        m_TraceSource.TraceVerbose( 344, "CheckData", msg );
      }
      else
      {
        ret = false;
        if ( !string.IsNullOrEmpty( comparationresulst ) )
        {
          m_TraceSource.TraceVerbose( 355, "CheckData", comparationresulst );
        }
      }
      return ret;
    }

    private bool SendReceive()
    {
      m_TraceSource.TraceVerbose( 198, "SendReceive", "SendReceive has started" );
      Thread.Sleep( c_WaitAfterAndBeforeCommunication );
      bool toBeReturned = true;
      string msg = "";
      State m_state = State.transmiting;
      byte[] buff = null;
      List<byte> receivebuff = new List<byte>( 1024 );
      CAS.Lib.RTLib.Processes.Stopwatch mySW = new CAS.Lib.RTLib.Processes.Stopwatch();
      byte oChar = byte.MaxValue;

      while ( m_state != State.leaving )
      {
        switch ( m_state )
        {
          case State.transmiting:
            {
              m_state = State.receiving;
              try
              {
                ComPortCli.Flush();
              }
              catch ( Exception ex )
              {
                msg = String.Format
                  ( msg, "Error: an exception has been thrown by the Flush : {0}", ex.Message );
                m_TraceSource.TraceError( 185, m_state.ToString(), msg );
              }
              try
              {
                switch ( TransmitFrame( out buff, ComPortCli ) )
                {
                  case TFrameEndSignalRes.Success:
                    break;
                  case TFrameEndSignalRes.DisInd:
                    msg = "Error: disconnected while sending frame";
                    m_TraceSource.TraceError( 174, m_state.ToString(), msg );
                    m_state = State.error;
                    break;
                }
              }
              catch ( Exception ex )
              {
                msg = String.Format
                  ( msg, "Error: an exception has been thrown by the TransmitFrame : {0}", ex.Message );
                m_TraceSource.TraceError( 185, m_state.ToString(), msg );
                if ( !ComPortCli.Connected )
                  m_state = State.error;
              }
              break;
            }
          case State.receiving:
            receivebuff.Clear();
            mySW.StartReset();
            m_state = State.gettingChar;
            msg = "State.receiving - ComPortCli.GetChar( out oChar, c_FrameTimeout ) ";
            m_TraceSource.TraceVerbose( 198, m_state.ToString(), msg );
            try
            {
              switch ( ComPortCli.GetChar( out oChar, c_FrameTimeout ) )
              {
                case TGetCharRes.Success:
                  break;
                case TGetCharRes.Timeout:
                  m_state = State.error;
                  msg = "Timeout: no data received";
                  m_TraceSource.TraceInformation( 208, m_state.ToString(), msg );
                  break;
                case TGetCharRes.DisInd:
                  msg = "Error: disconected while waitin for next frame";
                  m_TraceSource.TraceError( 214, m_state.ToString(), msg );
                  m_state = State.error;
                  break;
              }
            }
            catch ( Exception ex )
            {
              msg = String.Format( "Error: an exception has been thrown by the GetChar: {0}", ex.Message );
              m_TraceSource.TraceError( 224, m_state.ToString(), msg );
              if ( !ComPortCli.Connected )
                m_state = State.error;
            }
            break;
          case State.gettingChar:
            receivebuff.Add( oChar );
            try
            {
              switch ( ComPortCli.GetChar( out oChar, c_CharTimeout ) )
              {
                case TGetCharRes.Success:
                  break;
                case TGetCharRes.Timeout:

                  m_state = State.leaving;
                  if ( !CheckData( buff, receivebuff ) )
                    m_state = State.error;
                  break;
                case TGetCharRes.DisInd:
                  msg = "Error: disconected while waitin for data";
                  m_TraceSource.TraceError( 254, m_state.ToString(), msg );
                  m_state = State.error;
                  break;
              }
            }
            catch ( Exception ex )
            {
              msg = String.Format( "Error: an exception has been thrown by the GetChar : {0}", ex.Message );
              m_TraceSource.TraceError( 264, m_state.ToString(), msg );
              if ( !ComPortCli.Connected )
                m_state = State.error;
            }
            break;
          case State.error:
            toBeReturned = false;
            m_state = State.leaving;
            break;
          default:
            break;
        }
      }
      Thread.Sleep( c_WaitAfterAndBeforeCommunication );
      return toBeReturned;
    }
    #endregion loopback communication
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
    private NULLTraceSource m_TraceSource = new NULLTraceSource( "CAS.Lib.CommonBus.ApplicationLayer.NULL.NULL_ApplicationLayerMaster" );
    private int m_errorfrequency = 100;
    private ulong m_rPackNum = 0;
    private ulong m_wPackNum = 0;
    private CAS.Lib.RTLib.Management.IProtocolParent myStatistic;
    private bool CommunicationThroughCommunicationLayer;
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
        m_rPackNum++;
        data = null;
        myStatistic.IncStTxFrameCounter();
        if ( ( m_errorfrequency > 0 ) && ( m_rPackNum % (ulong)m_errorfrequency == 0 ) )
        {
          myStatistic.IncStRxFragmentedCounter();
          myStatistic.RxDataBlock( false );
          return AL_ReadData_Result.ALRes_DatTransferErrr;
        }
        data = (IReadValue)pool.GetEmptyISesDBuffer();
        //        Processes.Timer.Wait(Processes.Timer.TInOneSecond/4);
        ( (NULL_message)data ).SetBlockDescription( station, block );
        ( (NULL_message)data ).ReadFromDB();

        bool success = true;
        if ( CommunicationThroughCommunicationLayer )
        {
          success = SendReceive();
        }
        if ( success )
          myStatistic.IncStRxFrameCounter();
        myStatistic.RxDataBlock( success );
        return AL_ReadData_Result.ALRes_Success;
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
      m_wPackNum++;
      myStatistic.IncStTxFrameCounter();
      if ( ( m_errorfrequency > 0 ) && ( m_wPackNum % (ulong)m_errorfrequency == 0 ) )
      {
        data.ReturnEmptyEnvelope();
        data = null;
        myStatistic.IncStRxFragmentedCounter();
        myStatistic.TxDataBlock( false );
        return AL_ReadData_Result.ALRes_DatTransferErrr;
      }
      ( (NULL_message)data ).WriteToDB();
      data.ReturnEmptyEnvelope();
      data = null;
      myStatistic.IncStRxFrameCounter();
      myStatistic.TxDataBlock( true );
      return AL_ReadData_Result.ALRes_Success;
    }
    #endregion
    #region creator
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
      m_errorfrequency = pErrorFrequency;
      ComPortCli = pComm;
      CommunicationThroughCommunicationLayer = true;
      if ( pComm.GetType().Name.Contains( "NULL" ) )
        CommunicationThroughCommunicationLayer = false;
    }
    #endregion
  }
}
