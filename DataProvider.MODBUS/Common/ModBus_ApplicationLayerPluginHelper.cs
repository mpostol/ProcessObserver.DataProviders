//<summary>
//  Title   : CommSever plug-in providing MODBUS implementation
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPOstol - 25-03-07: 
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
using CAS.Lib.CodeProtect;
using CAS.Lib.CommonBus.ApplicationLayer.Modbus;
using CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{
  ///<summary>
  /// CommSever plug-in providing MODBUS implementation
  ///</summary>
  [LicenseProvider( typeof( CodeProtectLP ) )]
  public partial class ModBus_ApplicationLayerPluginHelper: Component
  {
    #region private
    ///<summary>
    /// Pool of Modbus buffers  implementation
    ///</summary>
    private class MODBUS_buf_pool: SesDBufferPool<ModBusMessage>
    {
      private ModBus_ProtocolParameters myProtocolParameters;
      protected override ModBusMessage CreateISesDBuffer()
      {
        ModBusMessage newMess = new ModBusMessage( this, myProtocolParameters );
        return newMess;
      }
      internal MODBUS_buf_pool( ModBus_ProtocolParameters MyProtocolParameters )
      {
        myProtocolParameters = MyProtocolParameters;
      }
    }//MODBUS_buf_pool
    private MODBUS_buf_pool m_Pool;
    #endregion
    #region public
    /// <summary>
    /// Creates the application layer master.
    /// </summary>
    /// <param name="pStatistic">The statistic.</param>
    /// <param name="pCommLayer">The communication layer.</param>
    /// <param name="pProtParameters">The protocol parameters.</param>
    /// <returns></returns>
    /// <exception cref="System.ComponentModel.LicenseException">The type is licensed, but a <see cref="System.ComponentModel.License"/> cannot be granted.</exception>
    public IApplicationLayerMaster CreateApplicationLayerMaster
      ( IProtocolParent pStatistic, ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters )
    {
      m_Pool = new MODBUS_buf_pool(
      (ModBus_ProtocolParameters)pProtParameters );
      ModBusProtocol mp = new ModBusProtocol( pCommLayer, pProtParameters, pStatistic, m_Pool );
      return new ModBus_ApplicationLayerMaster( m_Pool, mp );
    }
    /// <summary>
    /// Creates the application layer slave.
    /// </summary>
    /// <param name="cCommFactory">The communication factory.</param>
    /// <param name="cProtParameters">The protocol parameters.</param>
    /// <param name="cStatistic">The statistic.</param>
    /// <param name="cName">the name</param>
    /// <param name="cID">The Identifier.</param>
    /// <param name="cParent">The parent.</param>
    /// <returns></returns>
    public IApplicationLayerSlave CreateApplicationLayerSlave
      ( ICommunicationLayerFactory cCommFactory, ProtocolParameters cProtParameters,
        out IProtocolParent cStatistic, string cName, ulong cID, CommonBusControl cParent
      )
    {
      throw new ApplicationLayerInterfaceNotImplementedException();
    }
    #endregion
    #region creators
    /// <summary>
    /// Default creator of the helper component
    /// </summary>
    /// <exception cref="System.ComponentModel.LicenseException">System.ComponentModel.LicenseException - to inform that a license cannot be granted.
    /// </exception>
    public ModBus_ApplicationLayerPluginHelper()
    {
      if ( LicenseManager.UsageMode == LicenseUsageMode.Runtime )
        LicenseManager.Validate( this.GetType(), this );
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public ModBus_ApplicationLayerPluginHelper( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
  }
}
