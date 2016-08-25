//<summary>
//  Title   : Ec2_3_sym2, COMMUNICATIONS LIBRARY - Protocols Application layer interface - additional data for plugin interface
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    mzbrzezny 20070521: adaptation to new interface
//    MPOstol - 21-03-07: 
//      created compnent and added licensing
//    MZbrzezny - 05-02-2005:
//    Description: Created 
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM2
{
  using System;
  using CommunicationLayer;
  using System.ComponentModel;
  using CAS.Lib.RTLib.Management;
  //using PRIVATE;
  ///<summary>
  /// Symulator implementation
  ///</summary>
  public partial class EC2_3SYM2_ApplicationLayerPluginHelper : Component
  {
    #region IApplicationLayerPluginHelper
    //string IApplicationLayerPluginHelper.Name
    //{
    //  get
    //  {
    //    return "Symulator sieci cieplnej EC2- EC3";
    //  }
    //}
    //string IApplicationLayerPluginHelper.Author
    //{
    //  get
    //  {
    //    return "CAS Mariusz Postó³";
    //  }
    //}
    //string IApplicationLayerPluginHelper.Description
    //{
    //  get
    //  {
    //    return "NULLBUS protocol (master side) implementation for Commserver - virtual protocol for tests and simulations";
    //  }
    //}
    //Version IApplicationLayerPluginHelper.Version
    //{
    //  get
    //  {
    //    return new Version( 2, 0 );
    //  }
    //}
    //DateTime IApplicationLayerPluginHelper.Date
    //{
    //  get
    //  {
    //    return new DateTime( 2005, 01, 01 );//MZTD: Ew. sprawdzic i wstawic prawdziwa date
    //  }
    //}
    //int IApplicationLayerPluginHelper.Identifier
    //{
    //  get
    //  {
    //    return 4;
    //  }
    //}
    //IApplicationLayerMaster IApplicationLayerPluginHelperMaster.GetApplicationLayerMaster
    //  ( ICommunicationLayerFactory cCommFactory, ProtocolParameters myProtParameters, BaseStation.Management.IProtocolParent Statistic )
    //{
    //  return new NULL_ApplicationLayerMaster( cCommFactory, myProtParameters, Statistic );
    //}
    /// <summary>
    /// This fuction create simulator (application layer)
    /// </summary>
    /// <param name="pStatistic"></param>
    /// <param name="pComm"></param>
    /// <param name="pErrorFrequency"></param>
    /// <returns></returns>
    public IApplicationLayerMaster CreateApplicationLayerMaster
      ( IProtocolParent pStatistic, ICommunicationLayer pComm, int pErrorFrequency)
    {
      return new NULL_ApplicationLayerMaster(pStatistic, pComm, pErrorFrequency);
    }
    #endregion
    #region creators
    /// <summary>
    /// Default creator of the helper component
    /// </summary>
    public EC2_3SYM2_ApplicationLayerPluginHelper()
    {
      InitializeComponent();
    }
    /// <summary>
    ///  Creator of the helper component
    /// </summary>
    /// <param name="container">Parent container</param>
    public EC2_3SYM2_ApplicationLayerPluginHelper( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
  }
}
