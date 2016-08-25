//<summary>
//  Title   : NULL implementation of IApplicationLayerSlave
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
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.NULL
{

  /// <summary>
  /// Summary description for NULL_ApplicationLayerSlave.
  /// </summary>
  internal class NULL_ApplicationLayerSlave: ApplicationLayerCommon, IApplicationLayerSlave
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
    internal NULL_ApplicationLayerSlave
      ( IProtocolParent pStatistic, ICommunicationLayer pComm )
      : base( pComm )
    {
      this.myStatistic = pStatistic;
    }
    #endregion creators
  }
}
