//<summary>
//  Title   : MBUS application layer message VIFE field,base class for all VIF's
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
  class MBUSApplicationLayerVIFE: MBUSApplicationLayerVIFBase
  {
    #region private

    private static SortedList<byte, MBUSApplicationLayerVIFE> vif_list = new SortedList<byte, MBUSApplicationLayerVIFE>();
    private string group = "";

    private MBUSApplicationLayerVIFE( bool Extensinon, int Code, int RangeRatio, string Description, string Group, int RangeCode )
    {
      code = (byte)( Code + RangeCode );
      if ( Extensinon )
        code = (byte)( code + 128 );
      extension = Extensinon;
      rangeRatio = RangeRatio;
      description = Description;
      group = Group;
      vif_list.Add( code, this );
    }
    static private void CreateAndAddNewVIFE( int Code, int RangeRatio, string Description, string Group, int RangeCode )
    {
      new MBUSApplicationLayerVIFE( false, Code, RangeRatio, Description, Group, RangeCode );
      new MBUSApplicationLayerVIFE( true, Code, RangeRatio, Description, Group, RangeCode );
    }
    static MBUSApplicationLayerVIFE()
    {
      CreateAndAddNewVIFE( 0, 0, "None", "DIF Errors", 0 );
      CreateAndAddNewVIFE( 1, 0, "Too many DIFE's", "DIF Errors", 0 );
      CreateAndAddNewVIFE( 2, 0, "Storage number not implemented", "DIF Errors", 0 );
      CreateAndAddNewVIFE( 3, 0, "Unit number not implemented", "DIF Errors", 0 );
      CreateAndAddNewVIFE( 4, 0, "Tariff number not implemented", "DIF Errors", 0 );
      CreateAndAddNewVIFE( 5, 0, "Function not implemented", "DIF Errors", 0 );
      CreateAndAddNewVIFE( 6, 0, "Data class not implemented", "DIF Errors", 0 );
      CreateAndAddNewVIFE( 7, 0, "Data size not implemented", "DIF Errors", 0 );
      for ( int i = 0; i < 3; i++ )
        CreateAndAddNewVIFE( 8, 0, "Reserved", "DIF Errors", i );
      CreateAndAddNewVIFE( 11, 0, "To many VIFE's", "VIF Errors", 0 );
      CreateAndAddNewVIFE( 12, 0, "Illegal VIF-Group", "VIF Errors", 0 );
      CreateAndAddNewVIFE( 13, 0, "Illegal VIF-Exponent", "VIF Errors", 0 );
      CreateAndAddNewVIFE( 14, 0, "VIF/DIF mismatch", "VIF Errors", 0 );
      CreateAndAddNewVIFE( 15, 0, "Unimplemented action", "VIF Errors", 0 );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 16, 0, "Reserved", "VIF Errors", 0 );
      CreateAndAddNewVIFE( 21, 0, "No data avaliable (undefined value)", "Data Errors", 0 );
      CreateAndAddNewVIFE( 22, 0, "Data overflow", "Data Errors", 0 );
      CreateAndAddNewVIFE( 23, 0, "Data underflow", "Data Errors", 0 );
      CreateAndAddNewVIFE( 24, 0, "Data error", "Data Errors", 0 );
      for ( int i = 0; i < 3; i++ )
        CreateAndAddNewVIFE( 25, 0, "Reserved", "Data Errors", 0 );
      CreateAndAddNewVIFE( 28, 0, "Premature end of record", "Other Errors", 0 );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIFE( 25 + i, 0, "Reserved", "Other Errors", 0 );
      CreateAndAddNewVIFE( 32, 0, "per second", "", 0 );
      CreateAndAddNewVIFE( 33, 0, "per minute", "", 0 );
      CreateAndAddNewVIFE( 34, 0, "per hour", "", 0 );
      CreateAndAddNewVIFE( 35, 0, "per day", "", 0 );
      CreateAndAddNewVIFE( 36, 0, "per week", "", 0 );
      CreateAndAddNewVIFE( 37, 0, "per month", "", 0 );
      CreateAndAddNewVIFE( 38, 0, "per year", "", 0 );
      CreateAndAddNewVIFE( 39, 0, "per revolution/measurement", "", 0 );
      CreateAndAddNewVIFE( 40, 0, "increment per input pules on input channel #0", "", 0 );
      CreateAndAddNewVIFE( 41, 0, "increment per input pulse on input channel #1", "", 0 );
      CreateAndAddNewVIFE( 42, 0, "increment per input pules on output channel #0", "", 0 );
      CreateAndAddNewVIFE( 43, 0, "increment per input pulse on output channel #1", "", 0 );
      CreateAndAddNewVIFE( 44, 0, "per liter", "", 0 );
      CreateAndAddNewVIFE( 45, 0, "per m3", "", 0 );
      CreateAndAddNewVIFE( 46, 0, "per kg", "", 0 );
      CreateAndAddNewVIFE( 47, 0, "per K(Kelvin)", "", 0 );
      CreateAndAddNewVIFE( 48, 0, "per kWh", "", 0 );
      CreateAndAddNewVIFE( 49, 0, "per GJ", "", 0 );
      CreateAndAddNewVIFE( 50, 0, "per kW", "", 0 );
      CreateAndAddNewVIFE( 51, 0, "per (K*l) (Kelvin*liter)", "", 0 );
      CreateAndAddNewVIFE( 52, 0, "per V", "", 0 );
      CreateAndAddNewVIFE( 53, 0, "per A", "", 0 );
      CreateAndAddNewVIFE( 54, 0, "multiplied by sek", "", 0 );
      CreateAndAddNewVIFE( 55, 0, "multiplied by sek/V", "", 0 );
      CreateAndAddNewVIFE( 56, 0, "multiplied by sek/A", "", 0 );
      CreateAndAddNewVIFE( 57, 0, "star date(/time)", "", 0 );
      CreateAndAddNewVIFE( 58, 0, "VIF contains uncorrected unit instead of corrected unit", "", 0 );
      CreateAndAddNewVIFE( 59, 0, "Accumulation only if positive contribution", "", 0 );
      CreateAndAddNewVIFE( 60, 0, "Accumulation of abs value only if negativecontribution", "", 0 );
      for ( int i = 0; i < 3; i++ )
        CreateAndAddNewVIFE( 61, 0, "Reserved", "", i );
      CreateAndAddNewVIFE( 64, 0, "Lower limit value", "", 0 );
      CreateAndAddNewVIFE( 72, 0, "Upper limit value", "", 0 );
      CreateAndAddNewVIFE( 65, 0, "# of exceeds of lower limit", "", 0 );
      CreateAndAddNewVIFE( 73, 0, "# exceeds of upper limit", "", 0 );

      CreateAndAddNewVIFE( 66, 0, "Date(/time) of begin first lower limit exceed", "", 0 );
      CreateAndAddNewVIFE( 67, 0, "Date(/time) of begin first upper limit exceed", "", 0 );
      CreateAndAddNewVIFE( 70, 0, "Date(/time) of begin last lower limit exceed", "", 0 );
      CreateAndAddNewVIFE( 71, 0, "Date(/time) of begin last upper limit exceed", "", 0 );
      CreateAndAddNewVIFE( 74, 0, "Date(/time) of end first lower limit exceed", "", 0 );
      CreateAndAddNewVIFE( 75, 0, "Date(/time) of end first upper limit exceed", "", 0 );
      CreateAndAddNewVIFE( 78, 0, "Date(/time) of end last lower limit exceed", "", 0 );
      CreateAndAddNewVIFE( 79, 0, "Date(/time) of end last upper limit exceed", "", 0 );

      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 80, 0, "Duration of first lower limit exceed n=" + i.ToString(), "", i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 84, 0, "Duration of first upper limit exceed n=" + i.ToString(), "", i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 88, 0, "Duration of last lower limit exceed n=" + i.ToString(), "", i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 92, 0, "Duration of last upper exceed n=" + i.ToString(), "", i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 96, 0, "Duration of first  n=" + i.ToString(), "", i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 100, 0, "Duration of last  n=" + i.ToString(), "", i );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIFE( 104, 0, "Reserved", "", i );
      for ( int i = 0; i < 2; i++ )
        CreateAndAddNewVIFE( 108, 0, "Reserved", "", i );

      CreateAndAddNewVIFE( 106, 0, "Date(/time) of begin first", "", 0 );
      CreateAndAddNewVIFE( 107, 0, "Date(/time) of begin last", "", 0 );
      CreateAndAddNewVIFE( 110, 0, "Date(/time) of end first", "", 0 );
      CreateAndAddNewVIFE( 111, 0, "Date(/time) of end last", "", 0 );
      for ( int i = 0; i < 8; i++ )
        CreateAndAddNewVIFE( 112, i - 6, "Multiplicative correction factor", "", i );

      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIFE( 120, i - 3, "Additive correction constant * unit of VIF (offset)", "", i );

      CreateAndAddNewVIFE( 124, 0, "Reserved", "", 0 );
      CreateAndAddNewVIFE( 125, 3, "Multiplicative correction factor", "", 0 );
      CreateAndAddNewVIFE( 126, 0, "future value", "", 0 );
      CreateAndAddNewVIFE( 127, 0, "next VIFE's and data of this block are manufacturer specific", "", 0 );

    }
    #endregion private
    #region public

    internal static MBUSApplicationLayerVIFE GetVIFDescriptionByByte( byte code )
    {

      return vif_list[ code ];

    }
    internal string Group
    {
      get { return group; }
    }
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append( "VIFE: " );
      sb.Append( "Code: " );
      sb.Append( code );
      sb.Append( "Range Ratio " );
      sb.Append( rangeRatio );
      sb.Append( "Description " );
      sb.Append( description );
      sb.Append( " (" );
      sb.Append( group );
      sb.Append( ") " );
      return sb.ToString();
    }
    #endregion public
  }
}
