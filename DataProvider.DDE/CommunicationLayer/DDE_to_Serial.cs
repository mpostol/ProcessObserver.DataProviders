//<summary>
//  Title   : Facade implementation of ICommunicationLayer.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MZbrzezny 20081028: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using NDde.Client;

namespace CAS.Lib.CommonBus.CommunicationLayer.DDE
{
  /// <summary>
  /// Simulated implementation of ICommunicationLayer
  /// </summary>
  internal class DDE_to_Serial: ICommunicationLayer, IDisposable
  {
    string current_string = "";
    byte[] bytes;
    private void ClearBytesArray()
    {
      if ( bytes != null )
        for ( int idx = 0; idx < bytes.Length; idx++ )
          bytes[ idx ] = 0;
    }
    private class QueueElement
    {
      internal bool IsOK { get; set; }
      internal string Value { get; set; }
      internal QueueElement( string value, bool isOk )
      {
        IsOK = isOk;
        Value = value;
      }
    }
    private DdeClient client;
    Queue<QueueElement> ValueQueue = new Queue<QueueElement>();
    #region private
    private bool m_connected = false;
    private void client_Disconnected( object sender, DdeDisconnectedEventArgs e )
    {
      m_connected = false;
    }
    #endregion
    #region ICommunicationLayer Members
    /// <summary>
    /// Check if there is data awaiting in the buffer. 
    /// </summary>
    /// <returns>
    /// NoDataAvailable:
    ///   No data available.
    /// </returns>
    /// <remarks>Not implemented in this provider.</remarks>
    TCheckCharRes ICommunicationLayer.CheckChar()
    {
      if ( current_string == "" && ValueQueue.Count == 0 )
        return TCheckCharRes.NoDataAvailable;
      else
        return TCheckCharRes.DataInd;
    }
    /// <summary>
    /// Get the next character from the receiving stream. Blocks until next character is available.
    /// </summary>
    /// <param name="lastChr">The character</param>
    /// <returns>Never returns.</returns>
    /// <remarks>Not implemented in this provider. Never returns – blocks the calling thread in the false assertion.</remarks>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr )
    {
      while ( current_string == "" )
      {
        QueueElement el = null;
        if ( ValueQueue.Count > 0 )
          el = ValueQueue.Dequeue();
        else
        {
          lastChr = 0;
          return TGetCharRes.Timeout;
        }
        if ( el != null && el.IsOK )
        {
          current_string = el.Value;
        }
      }
      lastChr = System.Convert.ToByte( current_string[ 0 ] );
      current_string = current_string.Substring( 1, current_string.Length - 1 );
      return TGetCharRes.Success;
    }
    /// <summary>
    /// Get the next character from the receiving stream.
    /// </summary>
    /// <param name="lastChr">Always lastChr = 0</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the character.</param>
    /// <returns>
    /// Timeout:
    ///   Data is unavailable because of a communication error – loss of communication with a station
    ///</returns>
    ///<remarks>Not implemented in this provider. </remarks>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr, int millisecondsTimeout )
    {
      return ( (ICommunicationLayer)this ).GetChar( out lastChr );
    }
    /// <summary>
    /// Transmits the data contained in the frame.
    /// </summary>
    /// <param name="frame">Frame with the data to be transmitted.</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// </returns>
    TFrameEndSignalRes ICommunicationLayer.FrameEndSignal( UMessage frame )
    {
      //client.TryRequest(frame.GetManagedBuffer().ToString(),0,1000,
      //;
      string[] request = System.Text.ASCIIEncoding.ASCII.GetString(
        frame.GetManagedBuffer() ).Split( new char[] { ';' } );
      if ( m_connected == false )
        return TFrameEndSignalRes.DisInd;
      if ( request[ 0 ] == "REQ" )
      {
        ClearBytesArray();
        if ( client.TryRequest( request[ 1 ], 1, 500, out bytes ) == 0 )
        {
          string response = System.Text.ASCIIEncoding.ASCII.GetString( bytes );
          ValueQueue.Enqueue( new QueueElement( response, true ) );
        }
        else
        {
          ValueQueue.Enqueue( new QueueElement( "", false ) );
        }
      }
      else if ( request[ 0 ] == "SND" )
      {
        if ( client.TryPoke( request[ 1 ], new System.Text.ASCIIEncoding().GetBytes( request[ 2 ] ), 1, 500 ) == 0 )
        {
          ValueQueue.Enqueue( new QueueElement( "OK", true ) );
        }
        else
        {
          ValueQueue.Enqueue( new QueueElement( "BAD", false ) );
        }
      }
      return TFrameEndSignalRes.Success;
    }
    /// <summary>
    /// Flushes the buffer - Clean the inbound buffer.
    /// </summary>
    /// <remarks>Do nothing in this implementataion.</remarks>
    void ICommunicationLayer.Flush() { ValueQueue.Clear(); }
    #endregion
    #region IConnectionManagement Members
    /// <summary>
    /// Connect Request, this fuction is used for establishing the connection
    /// </summary>
    /// <param name="remoteAddress">address of the remote unit</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// </returns>
    TConnectReqRes IConnectionManagement.ConnectReq( IAddress remoteAddress )
    {
      try
      {
        client = new DdeClient( "Excel", remoteAddress.address.ToString() );
        client.Disconnected += new EventHandler<DdeDisconnectedEventArgs>( client_Disconnected );
        client.Connect();
        m_connected = true;
        return TConnectReqRes.Success;
      }
      catch ( Exception ex )
      {
        AssemblyTraceEvent.Tracer.TraceEvent(System.Diagnostics.TraceEventType.Information, 127, $"DDE Application Layer Master: Cannot Connect to {remoteAddress.address}, reason: {ex.Message}" );
        return TConnectReqRes.NoConnection;
      }
    }
    /// <summary>
    /// Connect indication – Check if there is a connection accepted to the remote address. 
    /// </summary>
    /// <param name="pRemoteAddress">
    /// The address of the remote unit we are waiting for connection from. Null if we are waiting for any connection.
    /// </param>
    /// <param name="pTimeOutInMilliseconds">
    /// How long the client is willing to wait for an incoming connection in ms.
    /// </param>
    /// <returns>
    /// NoConnection:
    ///   There is no incoming connection awaiting.
    /// </returns>
    /// <remarks>Not implemented in this provider.</remarks>
    TConnIndRes IConnectionManagement.ConnectInd( IAddress pRemoteAddress, int pTimeOutInMilliseconds )
    {
      return TConnIndRes.NoConnection;
    }
    /// <summary>
    /// Disconnect Request - Unconditionally disconnect the connection if any.
    /// </summary>
    void IConnectionManagement.DisReq()
    {
      client.Disconnect();
      m_connected = false;
    }
    /// <summary>
    /// true if the layer is connected for connection oriented communication or ready for communication 
    /// for connectionless communication.
    /// </summary>
    bool IConnectionManagement.Connected { get { return m_connected; } }
    #endregion
    #region IDisposable Members
    private bool disposed = false;
    #endregion
    #region creatoor
    internal DDE_to_Serial( object[] param )
      : this( (string)param[ 0 ], (CommonBusControl)param[ 1 ] )
    {
    }
    internal DDE_to_Serial( string pTraceName, CommonBusControl pParent )
    {
      bytes = new byte[ 255 ];
    }
    #endregion

    #region IDisposable Members

    void IDisposable.Dispose()
    {
      if ( disposed )
        return;
      if ( client != null )
        client.Dispose();
      client = null;
      disposed = true;
    }

    #endregion
  }
}
