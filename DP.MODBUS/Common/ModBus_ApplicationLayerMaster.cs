//<summary>
//  Title   : Modbus implementation
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//    20080825: cleanup in namespaces
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//
//    MPostol - 14-08-2004:
//      wywalilem definicje klasy ProtocolParameters jako wspolna
//    MPostol - 05-04-04
//      ProtocolParameters - nie czyta danych z konfiguracji. Wywali³em odpowiedni konstruktor. Klasa
//      inicjowana do wartoœci domyœlnych, pozniej ewentualnie modyfikowwane.
//      Zle wprowadzony byl czas przy pierwszym znaku ramki.
//    MPostol - 30-10-03:
//      WriteData - mistake in the parameter of TxDataBlock
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{
  ///<summary>
  /// Modbus implementation
  ///</summary>
  internal class ModBus_ApplicationLayerMaster: ApplicationLayerMaster<ModBusMessage>
  {
    #region creator
    /// <summary>
    /// ApplicationLayerMaster creator
    /// </summary>
    /// <param name="cPool">Empty data messages pool to be used by the protocol.</param>
    /// <param name="cProt">Protocol to be used.</param>
    internal ModBus_ApplicationLayerMaster( SesDBufferPool<ModBusMessage> cPool, ModBusProtocol cProt )
      : base( cProt, cPool ) { }
    #endregion
  }
}
