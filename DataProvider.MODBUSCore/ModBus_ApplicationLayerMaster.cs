//_______________________________________________________________
//  Title   : Modbus implementation
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.CommServer.DataProvider.MODBUSCore
{
  ///<summary>
  /// Modbus implementation
  ///</summary>
  public class ModBus_ApplicationLayerMaster<TModBusMessage> : ApplicationLayerMaster<TModBusMessage>
    where TModBusMessage: ProtocolALMessage
  {
    #region creator
    /// <summary>
    /// ApplicationLayerMaster creator
    /// </summary>
    /// <param name="pool">A pool of empty data messages to be used by the protocol.</param>
    /// <param name="protocol">Protocol to be used.</param>
    public ModBus_ApplicationLayerMaster( SesDBufferPool<TModBusMessage> pool, ALProtocol<TModBusMessage> protocol )
      : base( protocol, pool ) { }
    #endregion
  }
}
