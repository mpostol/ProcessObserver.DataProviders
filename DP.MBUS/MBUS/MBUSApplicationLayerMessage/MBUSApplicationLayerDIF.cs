//<summary>
//  Title   : MBUS application layer message DIF field
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080813: mzbrzezny: description is moved to base class
//  20080812: mzbrzezny: implementation of MBUSApplicationLayerExtendableDataInformation
//  20080704: mzbrzezny: ToString function is changed to avoid duplicating of displayed informations.
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
  internal class MBUSApplicationLayerDIF: MBUSApplicationLayerExtendableDataInformation
  {
    internal enum FunctionFields: byte
    {
      InstantenousValue = 0,
      MaximumValue = 1,
      MinimumValue = 2,
      ValueDuringErrorState = 3
    }
    internal enum DataFields: byte
    {
      NoData = 0,
      Integer8Bit = 1,
      Integer16bit = 2,
      Integer24bit = 3,
      Integer32bit = 4,
      Real32bit = 5,
      Integer48bit = 6,
      Integer64bit = 7,
      SelectionForReadout = 8,
      Digit2BCD = 9,
      Digit4BCD = 10,
      Digit6BCD = 11,
      Digit8BCD = 12,
      VariableLenght = 13,
      Digit12BCD = 14,
      SpecialFunction = 15
    }
    private static SortedList<byte, MBUSApplicationLayerDIF> dif_list = new SortedList<byte, MBUSApplicationLayerDIF>();
    #region members
    private byte code;
    private bool lsb;
    private FunctionFields functionField;
    private string functionFieldDescription;
    private DataFields dataField;
    private int dataFieldLegth;
    #endregion members
    private int GetIntFormInt( byte[] bytes )
    {
      int result=0;
      for ( int idx = DataFieldLengthInBytes - 1; idx >= 0; idx-- )
      {
        result *= 256;
        result += bytes[ idx ];
      }
      return  result;
    }
    private int GetIntFromBCD( byte[] bytes )
    {
      string result ="";
      for ( int idx = DataFieldLengthInBytes-1; idx >=0; idx-- )
      {
        int value;
        value = (bytes[ idx ]/16) & 0xf;
        result += value.ToString();
        value = bytes[ idx ] & 0xf;
        result += value.ToString();
      }
      return System.Convert.ToInt32( result);
    }
    private MBUSApplicationLayerDIF( bool Extension, bool LSB,
      FunctionFields FunctionField, string FunctionFieldDescription,
      DataFields DataField, string DataFieldDescription, int DataFieldLegth )
    {
      code = (byte)( (byte)FunctionField * 16 + (byte)DataField );
      if ( Extension )
        code = (byte)( code + 128 );
      if ( LSB )
        code = (byte)( code + 64 );
      extension = Extension;
      lsb = LSB;
      functionField = FunctionField;
      functionFieldDescription = FunctionFieldDescription;
      dataField = DataField;
      description = DataFieldDescription;
      dataFieldLegth = DataFieldLegth;
      dif_list.Add( code, this );
    }
    static private void CreateAndAddNewDIF( FunctionFields FunctionField, string FunctionFieldDescription,
      DataFields DataField, string DataFieldDescription, int DataFieldLegth )
    {
      new MBUSApplicationLayerDIF( false, false, FunctionField, FunctionFieldDescription,
          DataField, DataFieldDescription, DataFieldLegth );
      new MBUSApplicationLayerDIF( false, true, FunctionField, FunctionFieldDescription,
    DataField, DataFieldDescription, DataFieldLegth );
      new MBUSApplicationLayerDIF( true, false, FunctionField, FunctionFieldDescription,
          DataField, DataFieldDescription, DataFieldLegth );
      new MBUSApplicationLayerDIF( true, true, FunctionField, FunctionFieldDescription,
    DataField, DataFieldDescription, DataFieldLegth );
    }
    static private void CreateAndAddNewDIF( DataFields DataField, string DataFieldDescription, int DataFieldLegth )
    {
      CreateAndAddNewDIF( FunctionFields.InstantenousValue, "Instantenous Value", DataField, DataFieldDescription, DataFieldLegth );
      CreateAndAddNewDIF( FunctionFields.MaximumValue, "Maximum Value", DataField, DataFieldDescription, DataFieldLegth );
      CreateAndAddNewDIF( FunctionFields.MinimumValue, "Minimum Value", DataField, DataFieldDescription, DataFieldLegth );
      CreateAndAddNewDIF( FunctionFields.ValueDuringErrorState, "Value During Error State", DataField, DataFieldDescription, DataFieldLegth );
    }
    static MBUSApplicationLayerDIF()
    {
      CreateAndAddNewDIF( DataFields.NoData, "No Data", 0 );
      CreateAndAddNewDIF( DataFields.Integer8Bit, "8 bit Integer", 8 );
      CreateAndAddNewDIF( DataFields.Integer16bit, "16 bit Integer", 16 );
      CreateAndAddNewDIF( DataFields.Integer24bit, "24 bit Integer", 24 );
      CreateAndAddNewDIF( DataFields.Integer32bit, "32 bit Integer", 32 );
      CreateAndAddNewDIF( DataFields.Real32bit, "32 bit Real", 32 );//TODO: Verify : In documentation is 32/N.  
      CreateAndAddNewDIF( DataFields.Integer48bit, "48 bit Integer", 48 );
      CreateAndAddNewDIF( DataFields.Integer64bit, "64 bit Integer", 64 );
      CreateAndAddNewDIF( DataFields.SelectionForReadout, "Selection from Readout", 0 );
      CreateAndAddNewDIF( DataFields.Digit2BCD, "2 digit BCD", 8 );
      CreateAndAddNewDIF( DataFields.Digit4BCD, "4 digit BCD", 16 );
      CreateAndAddNewDIF( DataFields.Digit6BCD, "6 digit BCD", 24 );
      CreateAndAddNewDIF( DataFields.Digit8BCD, "8 digit BCD", 32 );
      CreateAndAddNewDIF( DataFields.VariableLenght, "Variable lenght", 32 ); //TODO: Verify: In documentation is 32/N.  
      CreateAndAddNewDIF( DataFields.Digit12BCD, "12 digit BCD", 48 );
      CreateAndAddNewDIF( DataFields.SpecialFunction, "Special Function", 64 );
    }
    internal static MBUSApplicationLayerDIF GetDIFDescriptionByByte( byte code )
    {
      return dif_list[ code ];
    }
    #region get properties
    internal byte Code
    {
      get { return code; }
    }
    internal bool Lsb
    {
      get { return lsb; }
    }
    internal FunctionFields FunctionField
    {
      get { return functionField; }
    }
    internal string FunctionFieldDescription
    {
      get { return functionFieldDescription; }
    }
    internal DataFields DataField
    {
      get { return dataField; }
    }
    internal string DataFieldDescription
    {
      get { return description; }
    }
    /// <summary>
    /// Gets the length in bits of the data field.
    /// </summary>
    /// <value>The length in bits of the data field.</value>
    internal int DataFieldLength
    {
      get { return dataFieldLegth; }
    }
    internal int DataFieldLengthInBytes
    {
      get { return dataFieldLegth / 8; }
    }
    #endregion get properties

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </returns>
    public override string ToString()
    {
      string _return;
      _return = "Code:" + this.code.ToString() + "; Extension:" + this.extension.ToString() + "; LSB:" + lsb.ToString() +
        "; FunctionField:" + this.functionFieldDescription +
        "; DataField:" +  this.description +
        "; DataFieldLenght:" + dataFieldLegth.ToString();
      return _return;
    }
    internal double ConvertToValue(byte []bytes)
    {
      switch ( DataField )
      {
        case DataFields.Digit2BCD:
        case DataFields.Digit4BCD:
        case DataFields.Digit6BCD:
        case DataFields.Digit8BCD:
          return GetIntFromBCD( bytes );
        case DataFields.Integer32bit:
        case DataFields.Integer16bit:
          return GetIntFormInt( bytes );
        default:
          throw new NotImplementedException();
      }
    }
  }
}
