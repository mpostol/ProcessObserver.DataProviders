//<summary>
//  Title   : DemoSimulator implementation of IApplicationLayerSlave
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
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{

  /// <summary>
  /// DemoSimulator implementation of IApplicationLayerSlave
  /// </summary>
  internal class ApplicationLayerSlave: ApplicationLayerCommon, IApplicationLayerSlave
  {
    #region PRIVATE
    CAS.Lib.RTLib.Management.IProtocolParent myStatistic;
    #endregion
    #region IApplicationLayerSlave Members
    AL_ReadData_Result IApplicationLayerSlave.ReadCMD( out IReadCMDValue frame )
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    IResponseValue IApplicationLayerSlave.GetEmptySendDataBuffor( IBlockDescription block, int address )
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    AL_ReadData_Result IApplicationLayerSlave.SendData( IResponseValue data )
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    AL_ReadData_Result IApplicationLayerSlave.SendNAKRes()
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    AL_ReadData_Result IApplicationLayerSlave.SendACKRes()
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    #endregion
    #region creators
    internal ApplicationLayerSlave
      ( IProtocolParent pStatistic, ICommunicationLayer pComm )
      : base( pComm )
    {
      this.myStatistic = pStatistic;
    }
    #endregion creators
  }
}
