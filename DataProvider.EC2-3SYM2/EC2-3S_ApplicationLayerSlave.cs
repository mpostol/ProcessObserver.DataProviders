//<summary>
//  Title   : EC2_3_sym2
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
namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM2
{
  using System;
  using CommunicationLayer;
  using CAS.NetworkConfigLib;
  /// <summary>
  /// Summary description for NULL_ApplicationLayerSlave.
  /// </summary>
  internal class NULL_ApplicationLayerSlave: ApplicationLayerCommon, IApplicationLayerSlave
  {
    #region PRIVATE
    BaseStation.Management.IProtocolParent myStatistic;
    #endregion
    #region IApplicationLayerSlave
    AL_ReadData_Result IApplicationLayerSlave.ReadCMD( out IBlockDescription command, int address, out ProtocolCmd cmd, out IReadCMDValue frame )
    {
      command = null;
      cmd = ProtocolCmd.coRR;
      frame = null;
      return AL_ReadData_Result.ALRes_DisInd; //not implemented
    }
    AL_ReadData_Result IApplicationLayerSlave.SendNAKRes()
    {
      return AL_ReadData_Result.ALRes_DisInd;; //not implemented
    }
    AL_ReadData_Result IApplicationLayerSlave.SendACKRes()
    {
      return AL_ReadData_Result.ALRes_DisInd;; //not implemented
    }
    ISendValue IApplicationLayerSlave.GetEmptySendDataBuffor( IBlockDescription block, int address )
    {
      return null;
    }
    AL_ReadData_Result IApplicationLayerSlave.SendData( ISendValue data )
    {
      return AL_ReadData_Result.ALRes_DisInd; //not implemented
    }
    #endregion
    internal NULL_ApplicationLayerSlave( ComunicationNet.ProtocolRow protDsc ): base (null)
    {
      this.myStatistic = BaseStation.Management.Protocol.CreateNewProtocol( "EC23 simulator slave protocol", protDsc.Name, protDsc.ProtocolID );
    }
  }
}
