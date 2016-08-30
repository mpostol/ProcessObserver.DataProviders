//<summary>
//  Title   : MBUS application layer message DIFE field
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
  internal class MBUSApplicationLayerDIFE: MBUSApplicationLayerExtendableDataInformation
  {
    private int subunit=0;
    private int tariff=0;
    private int storagenumber;
    internal int SubUnit
    {
      get { return subunit; }
    }
    internal int Tariff
    {
      get { return tariff; }
    }
    internal int StorageNumber
    {
      get { return storagenumber; }
    }
    internal void AddNextDIFE( byte b )
    {
      storagenumber = storagenumber * 16+ (int)( b & 15 );
      b = (byte) (b / 16);
      tariff = tariff * 4 + (int)( b & 3 );
      b = (byte) (b / 4);
      subunit = subunit * 2 + (int)( b & 1 );
      b = (byte) (b / 2);
      if ( b > 0 )
        extension = true;
      else
        extension = false;
    }
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append( "(SubUnit (device): " );
      sb.Append( subunit.ToString() );
      sb.Append( ",Tariff: " );
      sb.Append( tariff.ToString() );
      sb.Append( ",StorageNumber: " );
      sb.Append( storagenumber.ToString() );
      sb.Append( ")" );
      return sb.ToString();
    }
    internal MBUSApplicationLayerDIFE()
    {
      extension = true;
    }
  }
}
