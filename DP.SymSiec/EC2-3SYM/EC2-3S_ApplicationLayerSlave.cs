//<summary>
//  Title   : NULL_ApplicationLayerSlave
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
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM
{
  using System;
  using CAS.NetworkConfigLib;
  using CAS.Lib.CommonBus.CommunicationLayer;
  /// <summary>
  /// Summary description for NULL_ApplicationLayerSlave.
  /// </summary>
  internal class NULL_ApplicationLayerSlave: IApplicationLayerSlave
  {
    #region PRIVATE
    BaseStation.Management.IProtocolParent myStatistic;
    #endregion
    #region IApplicationLayerSlave
    ApplicationLayerResults IApplicationLayerSlave.ReadCMD( out IBlockDescription command, int address, out ProtocolCmd cmd, out IReadCMDValue frame )
    {
      command = null;
      cmd = ProtocolCmd.coRR;
      frame = null;
      return ApplicationLayerResults.ConnectionFails; //not implemented
    }
    ApplicationLayerResults IApplicationLayerSlave.SendNAKRes()
    {
      return ApplicationLayerResults.ConnectionFails; //not implemented
    }
    ApplicationLayerResults IApplicationLayerSlave.SendACKRes()
    {
      return ApplicationLayerResults.ConnectionFails; //not implemented
    }
    ISendValue IApplicationLayerSlave.GetEmptySendDataBuffor(IBlockDescription block, int address)
    {
      return null;
    }
    ApplicationLayerResults IApplicationLayerSlave.SendData( ISendValue data )
    {
      return ApplicationLayerResults.ConnectionFails; //not implemented
    }
    #endregion
    #region IApplicationLayerManagement
    ApplicationLayerManagementResults IApplicationLayerManagement.ListenReq( bool state )
    {
      return ApplicationLayerManagementResults.NoConnection; //not implemented
    }
    ApplicationLayerManagementResults IApplicationLayerManagement.ConnectReq( CommunicationLayer.IAddress address )
    {
      return ApplicationLayerManagementResults.NoConnection; //not implemented
    }
    TConnIndRes IApplicationLayerManagement.ConnectInd()
    {
      Processes.Timer.Wait( Processes.Timer.TInOneSecond * 30 );
      return TConnIndRes.NoConnection; //not implemented
    }
    ApplicationLayerManagementResults IApplicationLayerManagement.DisconnectReq()
    {
      return ApplicationLayerManagementResults.NoConnection; //not implemented
    }
    //ApplicationLayerManagementResults IApplicationLayerManagement.DisconnectInd()
    //{
    //  return ApplicationLayerManagementResults.NoConnection; //not implemented
    //}
    BaseStation.Management.IProtocolParent IApplicationLayerManagement.Statistic
    {
      get
      {
        return myStatistic;
      }
    }
    #endregion
    internal NULL_ApplicationLayerSlave( ComunicationNet.ProtocolRow protDsc )
    {
      this.myStatistic      = 
        BaseStation.Management.Protocol.CreateNewProtocol("Ec23 slave protocol", protDsc.Name, protDsc. ProtocolID);
    }
  }
}
