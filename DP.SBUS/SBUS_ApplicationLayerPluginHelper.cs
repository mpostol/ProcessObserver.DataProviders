//<summary>
//  Title   : Commsever plug-in providing SBUS/UDP/RS implementation
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPOstol - 25-03-07: 
//      created compnent and added licensing
//    MZbrzezny - 05-02-2005:
//      Description: Created 
//
//  Copyright (C)2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using CAS.Lib.CodeProtect;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS
{

  ///<summary>
  /// CommSever plug-in providing SBUS RS/UDP implementation
  ///</summary>
  [LicenseProvider( typeof( CodeProtectLP ) )]
  public partial class SBUS_ApplicationLayerPluginHelper: Component
  {
    #region private
    private class SBUSDBPool: SesDBufferPool<FrameStateMachine>
    {
      protected override FrameStateMachine CreateISesDBuffer()
      {
#if SBUSNET
        FrameStateMachine mes = new SBUSNet_message( this );
#else
        FrameStateMachine mes = new SBUSRS_message( this );
#endif
        return mes;
      }
    }
    private SBUSDBPool m_Pool = new SBUSDBPool();
    #endregion

    #region public
    /// <summary>
    /// Creates the application layer master.
    /// </summary>
    /// <param name="pCommLayer">The communication layer.</param>
    /// <param name="pProtParameters">The protocol parameters.</param>
    /// <param name="pStatistic">The  statistic.</param>
    /// <returns>IApplicationLayerMaster</returns>
    public IApplicationLayerMaster CreateApplicationLayerMaster
      ( IProtocolParent pStatistic, ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters )
    {
      SBUSProtocol mp = new SBUSProtocol( pStatistic, pCommLayer, pProtParameters, m_Pool );
      return new SBUS_ApplicationLayerMaster( m_Pool, mp );
    }
    /// <summary>
    /// Creates the application layer slave.
    /// </summary>
    /// <param name="pCommLayer">The communication layer.</param>
    /// <param name="pProtParameters">The protocol parameters.</param>
    /// <param name="pStatistic">The  statistic.</param>
    /// <returns>IApplicationLayerSlave</returns>
    public IApplicationLayerSlave CreateApplicationLayerSlave
      ( ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters, IProtocolParent pStatistic )
    {
      throw new ApplicationLayerInterfaceNotImplementedException();
    }
    /// <summary>
    /// Creates the application layer sniffer.
    /// </summary>
    /// <param name="pCommLayer">The communication layer.</param>
    /// <param name="pProtParameters">The protocol parameters.</param>
    /// <param name="pStatistic">The  statistic.</param>
    /// <returns>IApplicationLayerSlave</returns>
    public IApplicationLayerSniffer CreateApplicationLayerSniffer
      ( ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters, IProtocolParent pStatistic )
    {
      throw new ApplicationLayerInterfaceNotImplementedException();
    }
    #endregion

    #region creators
    /// <summary>
    /// Default creator of the helper component
    /// </summary>
    /// <exception cref="System.ComponentModel.LicenseException">
    /// System.ComponentModel.LicenseException - to inform that a license cannot be granted.
    /// </exception>
    public SBUS_ApplicationLayerPluginHelper()
    {
      if ( LicenseManager.UsageMode == LicenseUsageMode.Runtime )
        LicenseManager.Validate( this.GetType(), this );
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public SBUS_ApplicationLayerPluginHelper( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
  }
}
