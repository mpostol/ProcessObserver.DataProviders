//<summary>
//  Title   : COMMUNICATIONS LIBRARY - Protocols Application layer interface - additional data for plugin interface
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPOstol - 21-03-07: 
//      created compnent and added licensing
//    MZbrzezny - 05-02-2005:
//    Description: Created 
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using CAS.Lib.CommonBus.CommunicationLayer;

namespace CAS.Lib.CommonBus.ApplicationLayer.NULL
{
  //using PRIVATE;
  ///<summary>
  /// Modbus implementation
  ///</summary>
  public partial class NULL_ApplicationLayerPluginHelper: Component
  {
    #region IApplicationLayerPluginHelper
    /// <summary>
    /// Creates the application layer master.
    /// </summary>
    /// <param name="pStatistic">The  statistic object.</param>
    /// <param name="pComm">The communication layer.</param>
    /// <param name="pErrorFrequency">The  error frequency.</param>
    /// <returns>object: ApplicationLayerMaster</returns>
    public IApplicationLayerMaster CreateApplicationLayerMaster
      ( CAS.Lib.RTLib.Management.IProtocolParent pStatistic, ICommunicationLayer pComm, int pErrorFrequency )
    {
      return new NULL_ApplicationLayerMaster( pStatistic, pComm, pErrorFrequency );
    }
    #endregion
    #region creators
    /// <summary>
    /// Default creator of the helper component
    /// </summary>
    public NULL_ApplicationLayerPluginHelper()
    {
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public NULL_ApplicationLayerPluginHelper( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
  }
}
