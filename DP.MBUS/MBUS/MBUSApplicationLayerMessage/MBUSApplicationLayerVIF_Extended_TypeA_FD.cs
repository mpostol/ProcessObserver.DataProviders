//<summary>
//  Title   : MBUS application layer message VIFE field, extension of primary VIF - codes used with extension indicator $FD (table: 8.4.4a)
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080813: mzbrzezny: created
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
  class MBUSApplicationLayerVIF_Extended_TypeA_FD: MBUSApplicationLayerVIFBase
  {
    #region private
    private static SortedList<byte, MBUSApplicationLayerVIF_Extended_TypeA_FD> vif_list = new SortedList<byte, MBUSApplicationLayerVIF_Extended_TypeA_FD>();
    private string group = "";

    private MBUSApplicationLayerVIF_Extended_TypeA_FD( bool Extensinon, int Code, int RangeRatio, string Description, string Group, int RangeCode )
    {
      try
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
      catch ( Exception ex )
      {
        string exinfo = String.Format( "Cannot initialise MBUSApplicationLayerVIF_Extended_TypeA_FD (ext:{0},code:{1},rangert{2},{3},{4},rangecode:{5}):{6}",
          Extension, Code,RangeRatio, Description, Group, RangeCode, ex.ToString() );
        throw new Exception( exinfo );
      }
    }
    #endregion private

    #region private static
    static private void CreateAndAddNewVIF_Extended_TypeA_FD( int Code, int RangeRatio, string Description, string Group, int RangeCode )
    {
      new MBUSApplicationLayerVIF_Extended_TypeA_FD( false, Code, RangeRatio, Description, Group, RangeCode );
      new MBUSApplicationLayerVIF_Extended_TypeA_FD( true, Code, RangeRatio, Description, Group,RangeCode );
    }
    static MBUSApplicationLayerVIF_Extended_TypeA_FD()
    {
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeA_FD( 0 , i - 3, "Credit of 10 of nominal local legal currency units", "Curency Units",i );
      for ( int i = 0; i < 4; i++ )
        CreateAndAddNewVIF_Extended_TypeA_FD( 4 , i - 3, "Debit of 10 of nominal local legal currency units", "Curency Units",i  );
      CreateAndAddNewVIF_Extended_TypeA_FD( 8, 0, "Access Number", "Enhanced Identification",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 9, 0, "Medium", "Enhanced Identification" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 10, 0, "Manufacturer", "Enhanced Identification" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 11, 0, "Parameter set identification", "Enhanced Identification" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 12, 0, "Model/Version", "Enhanced Identification" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 13, 0, "Hardware version#", "Enhanced Identification" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 14, 0, "Firmware version#", "Enhanced Identification" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 15, 0, "Software version#", "Enhanced Identification" ,0);

      CreateAndAddNewVIF_Extended_TypeA_FD( 16, 0, "Customer location", "Implementation off all TC294 WG1 requirements",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 17, 0, "Customer", "Implementation off all TC294 WG1 requirements",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 18, 0, "Access Code User", "Implementation off all TC294 WG1 requirements",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 19, 0, "Access Code Operator", "Implementation off all TC294 WG1 requirements",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 20, 0, "Access Code System Operator", "Implementation off all TC294 WG1 requirements",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 21, 0, "Access Code Developer", "Implementation off all TC294 WG1 requirements" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 22, 0, "Password", "Implementation off all TC294 WG1 requirements" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 23, 0, "Error flag", "Implementation off all TC294 WG1 requirements" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 24, 0, "Error mask", "Implementation off all TC294 WG1 requirements" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 25, 0, "Reserved", "Implementation off all TC294 WG1 requirements" ,0);

      CreateAndAddNewVIF_Extended_TypeA_FD( 26, 0, "Digital Output", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 27, 0, "Digital Input", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 28, 0, "Baudrate[Baud]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 29, 0, "response delay time[bittimes]", "",0 );

      CreateAndAddNewVIF_Extended_TypeA_FD( 30, 0, "Retry", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 31, 0, "Reserved", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 32, 0, "First storage # for cyclic storage", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 33, 0, "Last storage # for cyclic storage", "Enchanced storage management",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 34, 0, "Size of storage block", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 35, 0, "Reserved", "Enchanced storage management" ,0);

      CreateAndAddNewVIF_Extended_TypeA_FD( 36, 0, "Storage interval second", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 37, 0, "Storage interval minute", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 38, 0, "Storage interval hour", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 39, 0, "Storage interval day", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 40, 0, "Storage interval month", "Enchanced storage management",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 41, 0, "Storage interval year", "Enchanced storage management",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 42, 0, "Reserved", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 43, 0, "Reserved", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 44, 0, "Duration since last readout second", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 45, 0, "Duration since last readout minute", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 46, 0, "Duration since last readout hour", "Enchanced storage management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 47, 0, "Duration since last readout day", "Enchanced storage management" ,0);

      CreateAndAddNewVIF_Extended_TypeA_FD( 48, 0, "Start of tariff", "Enchanced tariff management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 49, 0, "Duration of tariff [minute]", "Enchanced tariff management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 50, 0, "Duration of tariff [hour]", "Enchanced tariff management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 51, 0, "Duration of tariff [day]", "Enchanced tariff management" ,0);

      CreateAndAddNewVIF_Extended_TypeA_FD( 52, 0, "Period of tariff  last readout second", "Enchanced tariff management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 53, 0, "DPeriod of tariff  last readout minute", "Enchanced tariff management",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 54, 0, "Period of tariff last readout hour", "Enchanced tariff management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 55, 0, "Period of tariff  last readout day", "Enchanced tariff management",0 );

      CreateAndAddNewVIF_Extended_TypeA_FD( 56, 0, "Period of tariff monts", "Enchanced tariff management",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 57, 0, "Period of tariff year", "Enchanced tariff management",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 58, 0, "dimensionless/no VIF", "Enchanced tariff management" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 59, 0, "Reserved", "Enchanced tariff management" ,0);
      for (int i=0;i<4;i++)
      CreateAndAddNewVIF_Extended_TypeA_FD( 60, 0, "Reserved", "Enchanced tariff management" ,i);
      for ( int i = 0; i < 16; i++ )
        CreateAndAddNewVIF_Extended_TypeA_FD( 64, i - 9, "Volts", "electrical units" ,i);
      for ( int i = 0; i < 16; i++ )
        CreateAndAddNewVIF_Extended_TypeA_FD( 80 , i - 12, "A", "electrical units" ,i);
      CreateAndAddNewVIF_Extended_TypeA_FD( 96, 0, "Reset counter", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 97, 0, "Cumulation counter", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 98, 0, "Control sign", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 99, 0, "Day of week", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 100, 0, "Week number", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 101, 0, "Time point of day change", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 102, 0, "State of parameter activatio", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 103, 0, "Special supplier informatio", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 104, 0, "Duration since last cumulation [hour]", "",0 );
      CreateAndAddNewVIF_Extended_TypeA_FD( 105, 0, "Duration since last cumulation [day]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 106, 0, "Duration since last cumulation [month]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 107, 0, "Duration since last cumulation [year]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 108, 0, "Operating time batery [hour]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 109, 0, "Operating time batery [day]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 110, 0, "Operating time batery [month]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 111, 0, "Operating time batery [year]", "" ,0);
      CreateAndAddNewVIF_Extended_TypeA_FD( 112, 0, "Date and time of batery change", "" ,0);
      for ( int i = 0; i < 15; i++ )
        CreateAndAddNewVIF_Extended_TypeA_FD( 113, 0, "Reserved", "" ,i);

    }
    #endregion

    #region public
    internal static MBUSApplicationLayerVIF_Extended_TypeA_FD GetVIFDescriptionByByte( byte code )
    {
      //if (vif_list.ContainsKey(code))
      return vif_list[ code ];
      //else return blankVIF;
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
      sb.Append( ";Range Ratio " );
      sb.Append( rangeRatio );
      sb.Append( ";Description " );
      sb.Append( description );
      sb.Append( " (" );
      sb.Append( group );
      sb.Append( ") " );
      return sb.ToString();
    }
    #endregion public


  }
}
