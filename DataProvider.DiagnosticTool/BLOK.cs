//<summary>
//  Title   : BLOK - XBus Measurement
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2007-04 mpostol created
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
using CAS.Lib.CommonBus.ApplicationLayer;

namespace CAS.DPDiagnostics
{
  /// <summary>
  /// IBlockDescription implementation
  /// </summary>
  internal class BLOK: IBlockDescription
  {
    #region private
    private int len;
    private int adr;
    private short myDataType;
    #endregion

    #region public
    internal BLOK()
    {
      length = 2;
      startAddress = 100;
      dataType = 10;
    }
    internal void Change( int adres )
    {
      startAddress = adres;
    }
    internal void Change( int adres, int length, short dataType )
    {
      this.length = length;
      this.startAddress = adres;
      this.dataType = dataType;
    }
    #endregion

    #region IBlockDescription Members
    public int startAddress
    {
      get
      {
        return adr;
      }
      set
      {
        adr = value;
      }
    }
    public int length
    {
      get
      {
        return len;
      }
      set
      {
        len = value;
      }
    }
    public short dataType
    {
      get
      {
        return myDataType;
      }
      set
      {
        myDataType = value;
      }
    }
    #endregion

  }
}
