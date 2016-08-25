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
  class MBUSApplicationLayerVIFBase: MBUSApplicationLayerExtendableDataInformation
  {
    #region private
    protected byte code;
    protected int rangeRatio=0;
    protected string engUnit;
    protected byte unitCode;
    #endregion private

    #region public
    internal byte Code
    {
      get { return code; }
    }
    internal int RangeRatio
    {
      get { return rangeRatio; }
    }
    internal string EngUnit
    {
      get { return engUnit; }
    }
    internal double RangeRatioMultiplikator
    {
      get
      {
        return Math.Pow( 10, RangeRatio );
      }
    }

    internal static DateTime CPToDate( bool IsCP16, byte[] CPbyte )
    {
      int day = 0;
      int month = 0;
      int yearpart1 = 0;
      int year = 0;
      int hour = 0;
      int min = 0;
      DateTime date = new DateTime();

      if ( IsCP16 == true )
      {
        ushort Word = (ushort)( ( CPbyte[ 1 ] ) * 256 + CPbyte[ 0 ] );
        day = ( 31 & Word );
        Word = (ushort)( Word / 32 );
        yearpart1 = Word & 7;
        Word = (ushort)( Word / 8 );
        month = Word & 15;
        Word = (ushort)( ( Word / 2 ) & 0xFFFC );
        year = Word + yearpart1;
        if ( year >= 1 && month >= 1 && month <= 12 && day >= 1 && day <= 31 )
          date = new DateTime( year, month, day );
      }
      if ( IsCP16 == false )
      {
        int Word = ( CPbyte[ 3 ] * 16777216 + CPbyte[ 2 ] * 65536 + CPbyte[ 1 ] * 256 + CPbyte[ 0 ] );
        min = ( 63 & Word );
        Word = ( Word / 256 );
        hour = ( Word & 31 );
        Word = ( Word / 256 );
        day = ( 31 & Word );
        Word = ( Word / 32 );
        yearpart1 = ( Word & 7 );
        Word = ( Word / 8 );
        month = ( Word & 15 );
        Word = ( ( Word / 2 ) & 0xFFFC );
        year = ( Word + yearpart1 );
        if ( year >= 0 && month >= 1 && month <= 12 && day >= 1 && day <= 31 )
          date = new DateTime( year, month, day, hour, min, 00 );
      }
      return date;
    }
    internal object ConvertToValue( byte[] bytes, MBUSApplicationLayerDIF MBUSApplicationLayerDIF )
    {
      switch ( this.Code )
      {
        case 108:
        case 236:
          return CPToDate( true, bytes ).ToString();
        case 109:
        case 237:
          return CPToDate( false, bytes ).ToString();
        default:
          return MBUSApplicationLayerDIF.ConvertToValue( bytes ) * RangeRatioMultiplikator;
      }
    }
    #endregion public
  }
}
