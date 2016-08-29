//_______________________________________________________________
//  Title   : CommSever plug-in providing MODBUS implementation
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

using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;
using System;
using System.ComponentModel;

namespace CAS.CommServer.DataProvider.MODBUSCore
{
  /// <summary>
  /// Class ModBus_ApplicationLayerPluginHelper.
  /// </summary>
  /// <typeparam name="TModbusMessage">The type of the Modbus message.</typeparam>
  /// <typeparam name="TProtocolParameters">The type of the Modbus protocol parameters.</typeparam>
  /// <seealso cref="System.ComponentModel.Component" />
  public abstract partial class ModBus_ApplicationLayerPluginHelperBase<TModbusMessage, TProtocolParameters, TModbusProtocol> : Component
    where TModbusMessage : ProtocolALMessage
    where TProtocolParameters : ProtocolParameters
    where TModbusProtocol : ModbusProtocol<TModbusMessage>
  {
    #region private
    ///<summary>
    /// Buffer of the <typeparamref name="TModbusMessage"/> messages
    ///</summary>
    private class PrivateBufferPool : SesDBufferPool<TModbusMessage>
    {
      private TProtocolParameters myProtocolParameters;
      private Func<PrivateBufferPool, TProtocolParameters, TModbusMessage> m_CreateModBusMessage;

      protected override TModbusMessage CreateISesDBuffer()
      {
        TModbusMessage newMess = m_CreateModBusMessage(this, myProtocolParameters);
        return newMess;
      }
      internal PrivateBufferPool(TProtocolParameters protocolParameters, Func<PrivateBufferPool, TProtocolParameters, TModbusMessage> createModBusMessage)
      {
        myProtocolParameters = protocolParameters;
        m_CreateModBusMessage = createModBusMessage;
      }
    }//MODBUS_buf_pool
    private PrivateBufferPool m_Pool;
    #endregion

    #region public
    /// <summary>
    /// Creates the application layer master.
    /// </summary>
    /// <param name="statistic">The statistic.</param>
    /// <param name="pCommLayer">The communication layer.</param>
    /// <param name="protocolParameters">The protocol parameters.</param>
    /// <returns></returns>
    /// <exception cref="System.ComponentModel.LicenseException">The type is licensed, but a <see cref="License"/> cannot be granted.</exception>
    public IApplicationLayerMaster CreateApplicationLayerMaster(IProtocolParent statistic, ICommunicationLayer communicationLayer, TProtocolParameters protocolParameters)
    {
      m_Pool = new PrivateBufferPool(protocolParameters, CreateModBusMessage);
      TModbusProtocol mp = CreateModBusProtocol(communicationLayer, protocolParameters, statistic, m_Pool);
      return CreateModBus_ApplicationLayerMaster(m_Pool, mp);
    }

    protected abstract TModbusMessage CreateModBusMessage(SesDBufferPool<TModbusMessage> pool, TProtocolParameters parameters);
    protected abstract IApplicationLayerMaster CreateModBus_ApplicationLayerMaster(SesDBufferPool<TModbusMessage> pool, TModbusProtocol protocol);
    protected abstract TModbusProtocol CreateModBusProtocol(ICommunicationLayer communicationLayer, TProtocolParameters parameters, IProtocolParent protocolParent, SesDBufferPool<TModbusMessage> pool);

    /// <summary>
    /// Creates the application layer slave.
    /// </summary>
    /// <param name="communicationLayerFactory">The communication factory.</param>
    /// <param name="protocolParameters">The protocol parameters.</param>
    /// <param name="protocolParent">The statistic.</param>
    /// <param name="cName">the name</param>
    /// <param name="cID">The Identifier.</param>
    /// <param name="cParent">The parent.</param>
    /// <returns></returns>
    public IApplicationLayerSlave CreateApplicationLayerSlave(ICommunicationLayerFactory communicationLayerFactory, ProtocolParameters protocolParameters, out IProtocolParent protocolParent, string cName, ulong cID, CommonBusControl cParent)
    {
      throw new ApplicationLayerInterfaceNotImplementedException();
    }
    #endregion

    #region constructors
    /// <summary>
    /// Default creator of the helper component
    /// </summary>
    public ModBus_ApplicationLayerPluginHelperBase()
    {
      if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
        LicenseManager.Validate(this.GetType(), this);
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public ModBus_ApplicationLayerPluginHelperBase(IContainer container)
      : this()
    {
      container.Add(this);
    }
    #endregion
  }
}
