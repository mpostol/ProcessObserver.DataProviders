//<summary>
//  Title   : Common ModBus parameters
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2009, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Text;
using CAS.Lib.CommonBus.ApplicationLayer;
using System.Xml;
using CAS.Lib.CommonBus.Xml;
using System.ComponentModel;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus.Common
{
  /// <summary>
  /// ProtocolParameters for Modbus
  /// </summary>
  [System.Serializable]
  public abstract class ModBus_CommonProtocolParameters: ProtocolParameters
  {
    /// <summary>
    /// Floating point types
    /// </summary>
    public enum FloatingPointType: int
    {
      /// <summary>
      /// standard (modicon)
      /// </summary>
      Standard_Modicon = 0,
      /// <summary>
      /// inverted (IEE)
      /// </summary>
      Inverted_IEE = 1
    }
    /// <summary>
    /// Order of registers (for 32bit registers)
    /// </summary>
    public enum RegisterOrderEnum: int
    {
      /// <summary>
      /// Standard First Register Is More Significant
      /// </summary>
      Standard_FirstRegisterIsMoreSignificant = 0,
      /// <summary>
      /// Inverted Second Register Is More Significant
      /// </summary>
      Inverted_SecondRegisterIsMoreSignificant = 1
    }
    #region PRIVATE
    #region XML identifiers
    private const string m_Tag_RegisterOrderIn32mode = "RegisterOrderIn32mode";
    private const string m_Tag_FloatingPoint = "FloatingPoint";
    #endregion
    #endregion
    #region PUBLIC
    /// <summary>
    /// Gets or sets a value indicating whether [first register is more significant in32mode].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [first register is more significant in32mode]; otherwise, <c>false</c>.
    /// </value>
    [BrowsableAttribute( true )]
    public RegisterOrderEnum RegisterOrderIn32mode { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether [modicon floating point_non IEEE].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [modicon floating point_non IEEE]; otherwise, <c>false</c>.
    /// </value>
    [BrowsableAttribute( true )]
    public FloatingPointType FloatingPoint{ get; set; }
    /// <summary>
    /// this function  reads settings from xml stream
    /// </summary>
    /// <param name="pSettings">XmlReader strea</param>
    protected void CommonReadSettings( System.Xml.XmlReader pSettings )
    {
      RegisterOrderIn32mode = (RegisterOrderEnum)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_RegisterOrderIn32mode );
      FloatingPoint = (FloatingPointType) XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_FloatingPoint );
    }
    /// <summary>
    /// this function  writes settings to xml stream
    /// </summary>
    /// <param name="pSettings">XmlWriter strea</param>
    protected void CommonWriteSettings( System.Xml.XmlWriter pSettings )
    {
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_RegisterOrderIn32mode,(int) RegisterOrderIn32mode );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_FloatingPoint, (int)FloatingPoint );
    }
    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="portSpeed">Baudrate of the communication line.</param>
    /// <param name="TimeoutResponse">Maximum response time this station is willing to wait.</param>
    /// <param name="NumberOfRetries">Maximum number of retries this station will try.</param>
    public ModBus_CommonProtocolParameters( uint portSpeed, uint TimeoutResponse, ushort NumberOfRetries )
      : base( portSpeed, TimeoutResponse, NumberOfRetries )
    {
      RegisterOrderIn32mode = RegisterOrderEnum.Standard_FirstRegisterIsMoreSignificant;
      FloatingPoint=FloatingPointType.Standard_Modicon;
    }
    #endregion
  }
}
