//<summary>
//  Title   : MODBUS (Common) implementation of the Application Layer Protocol
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2008-08-28: mzbrzezny: created based on ModBusProtocol
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Threading;
using CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{
  /// <summary>
  /// MODBUS implementation of the Application Layer Protocol
  /// </summary>
  internal partial class ModBusProtocol
  {
    #region private
    private SesDBufferPool<ModBusMessage> m_Pool;
    private System.Diagnostics.Stopwatch IntercharStopwatch = new System.Diagnostics.Stopwatch();
    private class DisconnectException: Exception
    {
      internal DisconnectException() : base( "Communication layer unexpectedly disconnected." ) { }
    }
    private bool CheckCharTimeout( TimeSpan timeout, System.Diagnostics.Stopwatch stopwatch )
    {
      System.Diagnostics.Debug.Assert( stopwatch.IsRunning, "StopWatch is not running" );
      if ( !stopwatch.IsRunning )
      {
        // if we are inside this "if" statement something has gone wrong we have to start the stopwatch to prevent
        // application hang
        stopwatch.Reset();
        stopwatch.Start();
      }
      TimeSpan minWaitTime = TimeSpan.FromMilliseconds( 20 );
      while ( true )
      {
        switch ( GetICommunicationLayer.CheckChar() )
        {
          case TCheckCharRes.DataInd:
            return true;
          case TCheckCharRes.NoDataAvailable:
            //because the thread can be unactive for a while we have to check once more character availability.
            if ( stopwatch.Elapsed > timeout && GetICommunicationLayer.CheckChar() != TCheckCharRes.DataInd )
              return false;
            else
              break;
            if ( stopwatch.Elapsed + minWaitTime < timeout )
              Thread.Sleep( 1 );
            break;
          case TCheckCharRes.DisInd:
            throw ( new DisconnectException() );
        }
      }
    }
    private void Flush( TimeSpan timeout )
    {
      if ( !IntercharStopwatch.IsRunning )
      {
        // sometimes e.g. first frame transmit the stopwatch is not running and it is neccessary to start it 
        IntercharStopwatch.Reset();
        IntercharStopwatch.Start();
      }
      bool firstRun = true;
      do
      {
        GetICommunicationLayer.Flush();
        if ( firstRun )
          firstRun = false;
        else
        {
          IntercharStopwatch.Reset();
          IntercharStopwatch.Start();
        }
      }
      while ( CheckCharTimeout( timeout, IntercharStopwatch ) );
      IntercharStopwatch.Reset();
    }
    #endregion
    #region creators
    /// <summary>
    /// MODBUS protocol initialization
    /// </summary>
    /// <param name="pCommLayer">Interface responsible for providing the communication</param>
    /// <param name="pProtParameters">Protocol parameters</param>
    /// <param name="pStatistic">Statistical information about the communication performance</param>
    /// <param name="pPool">Empty data messages pool to be used by the protocol.</param>
    internal ModBusProtocol
     ( ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters,
       CAS.Lib.RTLib.Management.IProtocolParent pStatistic, SesDBufferPool<ModBusMessage> pPool )
      : base( pCommLayer, pProtParameters, pStatistic )
    {
      this.m_Pool = pPool;
    }
    #endregion

  }
}