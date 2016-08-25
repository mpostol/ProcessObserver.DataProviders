//<summary>
//  Title   : MBUS application layer message MMVS field
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080601: mSchabowski: created
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
  internal class MBUSApplicationLayerMMVS
  {
    #region private
    private static SortedList<byte, MBUSApplicationLayerMMVS> mmvs_list = new SortedList<byte, MBUSApplicationLayerMMVS>();
    private byte code;
    private string description;
    private MBUSApplicationLayerMMVS( byte UnitCode, string Description )
    {
      code = UnitCode;
      description = Description;
      mmvs_list.Add( code, this );
    }
    #endregion

    #region private static
    static private void CreateAndAddNewMMVS( byte UnitCode, string Description )
    {
      new MBUSApplicationLayerMMVS( UnitCode, Description );

    }
    static MBUSApplicationLayerMMVS()
    {
      CreateAndAddNewMMVS( 0, "Other" );
      CreateAndAddNewMMVS( 1, "Oil" );
      CreateAndAddNewMMVS( 2, "Electricity" );
      CreateAndAddNewMMVS( 3, "Gas" );
      CreateAndAddNewMMVS( 4, "Heat-outlet" );
      CreateAndAddNewMMVS( 5, "Steam" );
      CreateAndAddNewMMVS( 6, "Hot Water" );
      CreateAndAddNewMMVS( 7, "Water" );
      CreateAndAddNewMMVS( 8, "Heat Cost Allocator." );
      CreateAndAddNewMMVS( 9, "Compressed Air" );
      CreateAndAddNewMMVS( 10, "Cooling load meter-outlet" );
      CreateAndAddNewMMVS( 11, "Cooling load meter-inlet" );
      CreateAndAddNewMMVS( 12, "Heat-intlet" );
      CreateAndAddNewMMVS( 13, "Heat/Cooling load meter" );
      CreateAndAddNewMMVS( 14, "Bus/System" );
      CreateAndAddNewMMVS( 15, "Unknown Medium" );
      for ( int i = 16; i < 22; i++ )
        CreateAndAddNewMMVS( (byte)i, "Reserved" );
      CreateAndAddNewMMVS( 22, "Cold Water" );
      CreateAndAddNewMMVS( 23, "Dual Water" );
      CreateAndAddNewMMVS( 24, "Presure" );
      CreateAndAddNewMMVS( 25, "A/D Converter" );
      for ( int i = 26; i < 256; i++ )
        CreateAndAddNewMMVS( (byte)i, "Reserved" );

    }

    #endregion
    #region public
    internal static MBUSApplicationLayerMMVS GetMMVSDescriptionByByte( byte code )
    {
      return mmvs_list[ code ];
    }
    internal byte Code
    {
      get { return code; }
    }
    internal string Description
    {
      get { return description; }
    }
    public override string ToString()
    {
      return "Code: " + this.code.ToString() + " Description: " + this.description;
    }
    #endregion

  }
}
