//<summary>
//  Title   : SBus implementation - master side
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPOstol 04-04-2007:
//      calkowita zmiana struktury klas wcelu wydzielenia części management
//    MZbrzezny 2007-01-31
//      usuwanie mechanizmu bazujacego na porcie w application layer i communication layer
//    MZbrzezny - 20060420
//      dodalem data.ReturnEmptyEnvelope();w writedata
//    Maciej Zbrzezny - 12-04-2006
//      dodalem  w read data z wracane false, gdy na telegram odpowiedzia nie jest ramka z danymi
//    MPostol - 14-08-2004:
//      wywalilem definicje klasy ProtocolParameters jako wspolna
//    MZbrzezny - 29-07-04:
//     module creation
//    <Author> - <date>:
//    <description>
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS
{
  using CommunicationLayer.Generic;
  using PRIVATE;
  ///<summary>
  /// Sbus implementation
  ///</summary>
  internal class SBUS_ApplicationLayerMaster: ApplicationLayerMaster<FrameStateMachine>
  {
    #region creator
    /// <summary>
    /// ApplicationLayerMaster creator
    /// </summary>
    /// <param name="cPool">Empty data messages pool to be used by the protocol.</param>
    /// <param name="cProt">Protocol to be used.</param>
    internal SBUS_ApplicationLayerMaster
      ( SesDBufferPool<FrameStateMachine> cPool, SBUSProtocol cProt )
      : base( cProt, cPool ){}
    #endregion
  }
}
