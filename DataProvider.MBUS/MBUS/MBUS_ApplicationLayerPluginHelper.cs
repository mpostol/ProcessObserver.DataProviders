//<summary>
//  Title   : CommSever plug-in providing MBUS implementation
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

using System.ComponentModel;
using System.Runtime.InteropServices;
using CAS.Lib.CodeProtect;
using CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS
{
  ///<summary>
  /// CommSever DataProvider plug-in providing MBUS implementation
  ///</summary>
  [
  LicenseProvider( typeof( CodeProtectLP ) ),
  GuidAttribute( "B421239F-7051-44cf-BC87-B8B659BE31EC" )
  ]
  public partial class MBUS_ApplicationLayerPluginHelper: Component
  {
    #region private
    ///<summary>
    /// MBUS implementation
    ///</summary>
    private class MBUS_buf_pool: SesDBufferPool<MBUS_message>
    {
      protected override MBUS_message CreateISesDBuffer()
      {
        MBUS_message newMess = new MBUS_message( this );
        return newMess;
      }
    }//MBUS_buf_pool
    private MBUS_buf_pool m_Pool = new MBUS_buf_pool();
    #endregion
    #region public
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
      MBUSProtocol mp = new MBUSProtocol( pCommLayer, pProtParameters, pStatistic, m_Pool );
      return new MBUS_ApplicationLayerMaster( m_Pool, mp );
    }
    /// <summary>
    /// Creates the application layer slave.
    /// </summary>
    /// <param name="cCommFactory">The communication layer factory.</param>
    /// <param name="cProtParameters">The protocol parameters.</param>
    /// <param name="cStatistic">The statistic.</param>
    /// <param name="cName">Name </param>
    /// <param name="cID">The Identifier</param>
    /// <param name="cParent">The parent common bus control.</param>
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
    public MBUS_ApplicationLayerPluginHelper()
    {
      if ( LicenseManager.UsageMode == LicenseUsageMode.Runtime )
        LicenseManager.Validate( this.GetType(), this );
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public MBUS_ApplicationLayerPluginHelper( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
  }
}
