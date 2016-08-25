//<summary>
//  Title   : DemoSimulator: COMMUNICATIONS LIBRARY - Protocols Application layer interface - additional data for plugin interface
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

using System.ComponentModel;
using CAS.Lib.CommonBus.CommunicationLayer;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{

  ///<summary>
  /// Modbus implementation
  ///</summary>
  public partial class ApplicationLayerPluginHelper: Component
  {
    #region IApplicationLayerPluginHelper

    /// <summary>
    /// Creates the application layer master.
    /// </summary>
    /// <param name="pStatistic">The statistic.</param>
    /// <param name="pCommLayer">The communication layer.</param>
    /// <param name="pErrorFrequency">The error frequency.</param>
    /// <returns>IApplicationLayerMaster</returns>
    public IApplicationLayerMaster CreateApplicationLayerMaster
      ( CAS.Lib.RTLib.Management.IProtocolParent pStatistic, ICommunicationLayer pCommLayer, int pErrorFrequency )
    {
      return new ApplicationLayerMaster( pStatistic, pCommLayer, pErrorFrequency );
    }
    #endregion
    #region creators
    /// <summary>
    /// Default creator of the helper component
    /// </summary>
    public ApplicationLayerPluginHelper()
    {
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public ApplicationLayerPluginHelper( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
  }
}
