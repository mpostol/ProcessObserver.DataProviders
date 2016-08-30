//<summary>
//  Title   : MBUS application layer message VIFE field, extension of primary VIF - codes used with extension indicator $FB (table: 8.4.4b)
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080812: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Text;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE.MBUSApplicationLayerMessage
{
  /// <summary>
  /// MBUS application layer message VIFE field, extension of primary VIF - codes used with extension indicator $FB (table: 8.4.4b)
  /// </summary>
  class MBUSApplicationLayerVIF_Extended_TypeB_FB: MBUSApplicationLayerVIFBase
  {
    #region private
    private static SortedList<byte, MBUSApplicationLayerVIF_Extended_TypeB_FB> vif_list = new SortedList<byte, MBUSApplicationLayerVIF_Extended_TypeB_FB>();

    private MBUSApplicationLayerVIF_Extended_TypeB_FB( bool Extension, int Code, string Description, string EngUnit, int RangeRatio, int RangeCode )
    {
      code = (byte)( Code + RangeCode );
      if ( Extension )
        code = (byte)( code + 128 );
      description = Description;
      extension = Extension;
      rangeRatio = RangeRatio;
      engUnit = EngUnit;
      vif_list.Add( code, this );
    }

    private static void CreateAndAddNewVIF_Extended_TypeB_FB( int Code, string Description, string EngUnit, int RangeRatio, int RangeCode )
    {
      new MBUSApplicationLayerVIF_Extended_TypeB_FB( false, Code, Description, EngUnit, RangeRatio, RangeCode );
      new MBUSApplicationLayerVIF_Extended_TypeB_FB( true, Code, Description, EngUnit, RangeRatio, RangeCode );
    }
    static MBUSApplicationLayerVIF_Extended_TypeB_FB()
    {
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 0 , "Energy", "MWh", i, i );
      for ( int i = 0; i < 6; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 2 , "Reserved", "", 0, i );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 8 , "Energy", "GJ", i - 1, i );
      for ( int i = 0; i < 6; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 10 , "Reserved", "", 0, i );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 16 , "Volume", "m3", i + 2, i );
      for ( int i = 0; i < 6; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 18 , "Reserved", "", 0, i );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 24 , "Mass", "t", i + 2, i );
      for ( int i = 0; i < 6; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 26 , "Reserved", "", 0, i );
      CreateAndAddNewVIF_Extended_TypeB_FB( 33, "Volume", "feet^3", -1, 0 );
      CreateAndAddNewVIF_Extended_TypeB_FB( 34, "Volume", "american gallon", -1, 0 );
      CreateAndAddNewVIF_Extended_TypeB_FB( 35, "Volume", "american gallon", 1, 0 );
      CreateAndAddNewVIF_Extended_TypeB_FB( 36, "Volume flow", "american gallon/min", -3, 0 );
      CreateAndAddNewVIF_Extended_TypeB_FB( 37, "Volume flow", "american gallon/min", 1, 0 );
      CreateAndAddNewVIF_Extended_TypeB_FB( 38, "Volume flow", "american gallon/h", 1, 0 );
      CreateAndAddNewVIF_Extended_TypeB_FB( 39, "Reserved", "", 0, 0 );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 40, "Power", "MW", i - 1, i );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 42, "Reserved", "", 0, 0 );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 44, "Reserved", "", 0, 0 );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 48, "Power", "GJ/h", i - 1, i );
      for ( int i = 0; i < 38; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 50, "Reserved", "", 0, 0 );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 88, "Flow Temperature","F", i - 3, i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 92, "Return Temperature","F", i - 3, i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 96, "Temperature Difference","F", i - 3, i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 100, "External Temperature", "F", i - 3, i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 104, "Reserved", "", 0, 0 );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 112, "Cold/Warm Temperature Limit", "F", i - 3,i );
      for (int i=0;i<4;i++)
        CreateAndAddNewVIF_Extended_TypeB_FB( 116, "Cold/Warm Temperature Limit", "C", i - 3,i );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIF_Extended_TypeB_FB( 120, "cumul. count max power", "W", i - 3,i );
    }

    #endregion private
    #region public


    internal static MBUSApplicationLayerVIF_Extended_TypeB_FB GetVIFDescriptionByByte( byte code )
    {
      return vif_list[ code ];
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append( "VIFE: " );
      sb.Append( "Code: " );
      sb.Append( code );
      sb.Append( "Description " );
      sb.Append( description );
      sb.Append( "Eng. Unit " );
      sb.Append( engUnit );
      sb.Append( "Range Ratio " );
      sb.Append( rangeRatio );
      return sb.ToString();
    }
    #endregion public
  }
}
