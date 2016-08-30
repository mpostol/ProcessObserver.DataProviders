//<summary>
//  Title   : CommSever plug-in providing MODBUS implementation (protocol specific part
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//	  20080828: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CodeProtect;
using CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;
using System;
using CAS.Lib.CommonBus.ApplicationLayer.Modbus;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{
  ///<summary>
  /// CommSever plug-in providing MODBUS implementation
  /// ! This is protocol specific part !
  ///</summary>
  [GuidAttribute("1C38514B-6801-44de-AAB9-1E406B3AAE77")]
  ///<summary>
  /// CommSever plug-in providing MODBUS implementation
  ///</summary>
  [LicenseProvider(typeof(CodeProtectLP))]
  public sealed class ModBus_ApplicationLayerPluginHelper : CAS.CommServer.DataProvider.MODBUSCore.ModBus_ApplicationLayerPluginHelperBase<ModBusMessage, ModBus_ProtocolParameters, ModBusProtocol>
  {
    protected override ModBusMessage CreateModBusMessage(SesDBufferPool<ModBusMessage> pool, ModBus_ProtocolParameters parameters)
    {
      return new ModBusMessage(pool, parameters);
    }

    protected override ModBusProtocol CreateModBusProtocol(ICommunicationLayer communicationLayer, ModBus_ProtocolParameters parameters, IProtocolParent protocolParent, SesDBufferPool<ModBusMessage> pool)
    {
      return new ModBusProtocol(communicationLayer, parameters, protocolParent, pool);
    }

    protected override IApplicationLayerMaster CreateModBus_ApplicationLayerMaster(SesDBufferPool<ModBusMessage> pool, ModBusProtocol protocol)
    {
      return new CAS.CommServer.DataProvider.MODBUSCore.ModBus_ApplicationLayerMaster<ModBusMessage>(pool, protocol);
    }
  }
}