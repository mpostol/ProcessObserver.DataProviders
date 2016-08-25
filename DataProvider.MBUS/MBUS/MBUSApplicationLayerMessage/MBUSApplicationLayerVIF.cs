//<summary>
//  Title   : MBUS application layer message VIF field
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080813: mzbrzezny: implementation of MBUSApplicationLayerVIFBase
//                       implementation of Special VIF's
//  20080812: mzbrzezny: implementation of MBUSApplicationLayerExtendableDataInformation
//  20080704: mzbrzezny: 
//            CPtoDate check whether year >1 (not 0) this is required to create System.DateTime object
//            Fixed: some of VIF information for temperature and pressure were missing
//  20080529: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE.MBUSApplicationLayerMessage
{
  internal class MBUSApplicationLayerVIF: MBUSApplicationLayerVIFBase
  {
    #region private
    private static SortedList<byte, MBUSApplicationLayerVIF> vif_list = new SortedList<byte, MBUSApplicationLayerVIF>();

    private MBUSApplicationLayerVIF( bool Extension, byte UnitCode, string Description, string EngUnit, int RangeRatio, byte RangeCode )
    {
      unitCode = UnitCode;
      code = (byte)( UnitCode * 8 + RangeCode );
      if ( Extension )
        code = (byte)( code + 128 );
      description = Description;
      extension = Extension;
      rangeRatio = RangeRatio;
      engUnit = EngUnit;
      vif_list.Add( code, this );
    }
    private MBUSApplicationLayerVIF( bool Extension, byte Code, string Description )
    {
      unitCode = 0;
      code = Code;
      description = Description;
      extension = Extension;
      rangeRatio = 0;
      engUnit = "";
      vif_list.Add( code, this );
    }
    #endregion private
    #region private static
    static private void CreateAndAddNewVIF( byte UnitCode, string Description, string EngUnit, int RangeRatio, byte RangeCode )
    {
      new MBUSApplicationLayerVIF( false, UnitCode, Description, EngUnit, RangeRatio, RangeCode );
      new MBUSApplicationLayerVIF( true, UnitCode, Description, EngUnit, RangeRatio, RangeCode );
    }
    static private void CreateAndAddNewVIF( bool Extension, SpecialVIF Code, string Description )
    {
      new MBUSApplicationLayerVIF( Extension, (byte) Code, Description );
    }
    static MBUSApplicationLayerVIF()
    {
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 0, "Energy", "Wh", i - 3, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 1, "Energy", "J", i, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 2, "Volume", "m3", i - 6, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 3, "Mass", "kg", i - 3, (byte)i );

      CreateAndAddNewVIF( 4, "On Time", "seconds", 0, (byte)0 );
      CreateAndAddNewVIF( 4, "On Time", "minutes", 0, (byte)1 );
      CreateAndAddNewVIF( 4, "On Time", "hours", 0, (byte)2 );
      CreateAndAddNewVIF( 4, "On Time", "days", 0, (byte)3 );
      CreateAndAddNewVIF( 4, "Operating Time", "seconds", 0, (byte)4 );
      CreateAndAddNewVIF( 4, "Operating Time", "minutes", 0, (byte)5 );
      CreateAndAddNewVIF( 4, "Operating Time", "hours", 0, (byte)6 );
      CreateAndAddNewVIF( 4, "Operating Time", "days", 0, (byte)7 );

      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 5, "Power", "W", i - 3, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 6, "Power", "J/h", i, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 7, "Volume Flow", "m3/h", i - 6, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 8, "Volume Flow ext.", "m3/min", i - 7, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 9, "Volume Flow ext.", "m3/s", i - 9, (byte)i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF( 10, "Mass flow", "kg/h", i - 3, (byte)i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF( 11, "Flow Temperature", "C", i - 3, (byte)i );
      for ( int i = 4; i < 8; i++ )
        CreateAndAddNewVIF( 11, "Return Temperature", "C", i - 3 - 4, (byte)i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF( 12, "Temperature Difference", "K", i - 3, (byte)i );
      for ( int i = 4; i < 8; i++ )
        CreateAndAddNewVIF( 12, "External Temperature", "C", i - 3 - 4, (byte)i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF( 13, "Presure", "bar", i - 3, (byte)i );
      CreateAndAddNewVIF( 13, "Time Point", "date", 0, (byte)4 );
      CreateAndAddNewVIF( 13, "Time Point", "time & date", 0, (byte)5 );
      CreateAndAddNewVIF( 13, "Units for H.C.A.", "", 0, (byte)6 );
      CreateAndAddNewVIF( 13, "Reserver", "", 0, (byte)7 );

      CreateAndAddNewVIF( 14, "Averaging Duration", "seconds", 0, (byte)0 );
      CreateAndAddNewVIF( 14, "Averaging Duration", "minutes", 0, (byte)1 );
      CreateAndAddNewVIF( 14, "Averaging Duration", "hours", 0, (byte)2 );
      CreateAndAddNewVIF( 14, "Averaging Duration", "days", 0, (byte)3 );
      CreateAndAddNewVIF( 14, "Actuality Duration", "seconds", 0, (byte)4 );
      CreateAndAddNewVIF( 14, "Actuality Duration", "minutes", 0, (byte)5 );
      CreateAndAddNewVIF( 14, "Actuality Duration", "hours", 0, (byte)6 );
      CreateAndAddNewVIF( 14, "Actuality Duration", "days", 0, (byte)7 );

      CreateAndAddNewVIF( 15, "Fabric No", "", 0, (byte)0 );
      CreateAndAddNewVIF( 15, "(Enhanced) Identification", "", 0, (byte)1 );
      CreateAndAddNewVIF( 15, "Buss Address", "", 0, (byte)2 );

      //VIF-Codes for special purposes (8.4.3 - second table):
      CreateAndAddNewVIF( true, SpecialVIF.Extension_FB, "Extension of VIF-codes, true VIF is given in the first VIFE)" );
      CreateAndAddNewVIF( true, SpecialVIF.Extension_FD, "Extension of VIF-codes, true VIF is given in the first VIFE)" );
      CreateAndAddNewVIF( false, SpecialVIF.String_0, "Data is encoded as string in the free user defined form" );
      CreateAndAddNewVIF( true, SpecialVIF.String_1, "Data is encoded as string in the free user defined form" );
      CreateAndAddNewVIF( false, SpecialVIF.AnyVIF_0, "used for readout selection" );
      CreateAndAddNewVIF( true, SpecialVIF.AnyVIF_1, "used for readout selection" );
      CreateAndAddNewVIF( false, SpecialVIF.ManufacturerSpecific_0, "Manufacturer Specific" );
      CreateAndAddNewVIF( true, SpecialVIF.ManufacturerSpecific_1, "Manufacturer Specific" );
    }

    #endregion private static
    #region public
    internal static MBUSApplicationLayerVIF GetVIFDescriptionByByte( byte code )
    {
      return vif_list[ code ];
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </returns>
    public override string ToString( )
    {    
      string _return;
      _return = "Code:" + this.code.ToString() + "; Description:" + this.description +
        "; RangeRatio:" + this.rangeRatio.ToString() + "; EngUnit:" + this.engUnit + "; Extension:" + this.extension.ToString();
      return _return;
    }
    internal enum SpecialVIF: byte
    {
      None=0,
      Extension_FB=0xFB,
      String_0 = 0x7C,
      String_1 = 0xFC,
      Extension_FD = 0xFD,
      AnyVIF_0 = 0x7E,
      AnyVIF_1 = 0xFE,
      ManufacturerSpecific_0 = 0x7F,
      ManufacturerSpecific_1 = 0xFF
    }
    #endregion public
  }
}
