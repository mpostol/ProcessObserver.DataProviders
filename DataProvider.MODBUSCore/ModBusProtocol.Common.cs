//_______________________________________________________________
//  Title   : MODBUS (Common) implementation of the Application Layer Protocol
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;
using System;
using System.Diagnostics;
using System.Threading;

namespace CAS.CommServer.DataProvider.MODBUSCore
{
  /// <summary>
  /// MODBUS implementation of the Application Layer Protocol
  /// </summary>
  public abstract class ModbusProtocol<ModBusMessage>: ALProtocol<ModBusMessage>
    where ModBusMessage: ProtocolALMessage
  {

    #region private
    protected SesDBufferPool<ModBusMessage> m_Pool { get; private set; }
    protected Stopwatch InterCharStopwatch = new Stopwatch();
    protected class DisconnectException: Exception
    {
      internal DisconnectException() : base( "Communication layer unexpectedly disconnected." ) { }
    }
    protected bool CheckCharTimeout( TimeSpan timeout, Stopwatch stopwatch )
    {
      Debug.Assert( stopwatch.IsRunning, "StopWatch is not running" );
      if ( !stopwatch.IsRunning )
      {
        // if we are inside this "if" statement something has gone wrong we have to start the stopwatch to prevent application hang
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
            //because the thread can be inactive for a while we have to check once more character availability.
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
    protected void Flush( TimeSpan timeout )
    {
      if ( !InterCharStopwatch.IsRunning )
      {
        // sometimes e.g. first frame transmit the stopwatch is not running and it is necessary to start it 
        InterCharStopwatch.Reset();
        InterCharStopwatch.Start();
      }
      bool firstRun = true;
      do
      {
        GetICommunicationLayer.Flush();
        if ( firstRun )
          firstRun = false;
        else
        {
          InterCharStopwatch.Reset();
          InterCharStopwatch.Start();
        }
      }
      while ( CheckCharTimeout( timeout, InterCharStopwatch ) );
      InterCharStopwatch.Reset();
    }
    #endregion

    #region creators
    /// <summary>
    /// MODBUS protocol initialization
    /// </summary>
    /// <param name="communicationLayer">Interface responsible for providing the communication</param>
    /// <param name="protocolParameters">Protocol parameters</param>
    /// <param name="protocolParent">Statistical information about the communication performance</param>
    /// <param name="pool">Empty data messages pool to be used by the protocol.</param>
    public ModbusProtocol( ICommunicationLayer communicationLayer, ProtocolParameters protocolParameters, IProtocolParent protocolParent, SesDBufferPool<ModBusMessage> pool )
      : base( communicationLayer, protocolParameters, protocolParent )
    {
      this.m_Pool = pool;
    }
    #endregion

  }
}