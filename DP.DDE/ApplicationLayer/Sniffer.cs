//<summary>
//  Title   : DDE implementation - sniffer side
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.CommunicationLayer;

namespace CAS.Lib.CommonBus.ApplicationLayer.DDE
{
  /// <summary>
  /// DDE implementation - sniffer side
  /// </summary>
  internal class Sniffer: ApplicationLayerCommon, IApplicationLayerSniffer
  {
    #region PRIVATE
    /// <summary>
    ///  Title   : NULL implementation of NULL_buf_pool: CommunicationLayer.SesDBufferPool
    /// </summary>
    private class NULL_buf_pool: CommunicationLayer.Generic.SesDBufferPool<Message>
    {
      protected override Message CreateISesDBuffer()
      {
        return new Message( this );
      }
    }//NULL_buf_pool
    private NULL_buf_pool pool = new NULL_buf_pool();
    CAS.Lib.RTLib.Management.IProtocolParent myStatistic;
    #endregion
    #region IApplicationLayerSniffer
    AL_ReadData_Result IApplicationLayerSniffer.ReadData( out IBlockDescription block, out IReadValue data )
    {
      throw new NotImplementedException( "using IApplicationLayerSniffer.ReadData is not valid for this DataProvider" );
    }//ReadData
    #endregion
    #region INIT
    internal Sniffer
      ( CAS.Lib.RTLib.Management.IProtocolParent pStatistic, ICommunicationLayer pComm )
      : base( pComm )
    {
      this.myStatistic = pStatistic;
    }
    #endregion
  }
}
