//<summary>
//  Title   : MBUS implementation
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//    20080519: mzbrzezny: created based on MBUS prlugin
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS
{
  ///<summary>
  /// MBUS implementation
  ///</summary>
  internal class MBUS_ApplicationLayerMaster: ApplicationLayerMaster<MBUS_message>
  {
    #region creator
    /// <summary>
    /// ApplicationLayerMaster creator
    /// </summary>
    /// <param name="cPool">Empty data messages pool to be used by the protocol.</param>
    /// <param name="cProt">Protocol to be used.</param>
    internal MBUS_ApplicationLayerMaster( SesDBufferPool<MBUS_message> cPool, MBUSProtocol cProt )
      : base( cProt, cPool ) { }
    #endregion
  }
}
