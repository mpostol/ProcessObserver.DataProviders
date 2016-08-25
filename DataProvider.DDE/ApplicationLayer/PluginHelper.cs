//<summary>
//  Title   : DDE: COMMUNICATIONS LIBRARY - Protocols Application layer interface - additional data for plugin interface
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20090721: mzbrzezny: licensing is added
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using System.Runtime.InteropServices;
using CAS.Lib.CodeProtect;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.DDE
{
  ///<summary>
  /// Modbus implementation
  ///</summary>
  [
   LicenseProvider( typeof( CodeProtectLP ) ),
   GuidAttribute( "1bf2dc74-b605-4e5a-a087-62d20c4d558c" )
  ]
  public partial class PluginHelper: Component
  {
    #region private
    ///<summary>
    /// MBUS implementation
    ///</summary>
    private class Buf_pool: SesDBufferPool<Message>
    {
      protected override Message CreateISesDBuffer()
      {
        Message newMess = new Message( this );
        return newMess;
      }
    }//MBUS_buf_pool
    private Buf_pool m_Pool = new Buf_pool();
    #endregion
    #region IApplicationLayerPluginHelper
    /// <summary>
    /// Creates the application layer master.
    /// </summary>
    /// <param name="pProtParameters">The protocol parameters.</param>
    /// <param name="pStatistic">The statistic.</param>
    /// <param name="pCommLayer">The communication layer.</param>
    /// <returns></returns>
    public IApplicationLayerMaster CreateApplicationLayerMaster
      ( IProtocolParent pStatistic, ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters )
    {
      Protocol mp = new Protocol( pCommLayer, pProtParameters, pStatistic, m_Pool );
      return new Master( m_Pool, mp );
    }
    #endregion
    #region creators
    /// <summary>
    /// Default creator of the helper component
    /// </summary>
    public PluginHelper()
    {
      if ( LicenseManager.UsageMode == LicenseUsageMode.Runtime )
        LicenseManager.Validate( this.GetType(), this );
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public PluginHelper( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
  }
}
