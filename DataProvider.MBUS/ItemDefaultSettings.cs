//<summary>
//  Title   : NULL plugin Data Provider Item Default Settings
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20081002 mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE;
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS
{
  class ItemDefaultSettings: IItemDefaultSettings
  {
    #region private
    private MediumData addressSpace;
    private ulong address;
    #endregion private
    #region IItemDefaultSettings Members
    string IItemDefaultSettings.Name
    {
      get
      {
        switch ( addressSpace )
        {
          case MediumData.Class2_Data:
            //naming rules:
            //Counter serial number*
            //Manufacturer identifier *
            //Information *
            //The number of received data records (called N in this manual)*
            //Record 1 – Value
            //Record 1 – Description of data type (length in bits, how the value is encoded, etc.)*
            //Record 1 – Description of data  (e.g. Energy, Volume, Flow, etc.)*
            //Record 1 – Engineering unit*
            //Record N – Value
            //Record N – Description of data type (length in bits, how the value is encoded, etc...)*
            //Record N – Description of data  (e.g. Energy, Volume, Flow, etc.)*
            //Record N – Engineering unit*
            switch ( address )
            {
              case 0:
                return "Class2/CounterSerialNumber";
              case 1:
                return "Class2/ManufacturerIdentifier";
              case 2:
                return "Class2/Information";
              case 3:
                return "Class2/NumberOfRecords";
              default:
                ulong newaddress = address - 4;
                string ret = "Class2/Value/" + ( newaddress / 4 ).ToString( "d2" );
                switch ( newaddress % 4 )
                {
                  case 0:
                    return ret;
                  case 1:
                    return ret + "/DescriptionOfDataType";
                  case 2:
                    return ret + "/DescriptionOfData";
                  case 3:
                    return ret + "/EngineeringUnit";
                  default:
                    break;
                }
                break;
            }
            //case MediumData.Class1_Data:
            //case MediumData.Class1_Data_Short:
            break;
          case MediumData.Class2_Data_Short:
            return addressSpace.ToString() + "Class2/Value/" + address.ToString( "d2" );
        }

        return addressSpace.ToString() + "/" + address.ToString();
      }
    }
    Type IItemDefaultSettings.DefaultType
    {
      get
      {
        switch ( addressSpace )
        {
          case MediumData.Class2_Data:
            //naming rules:
            //Counter serial number*
            //Manufacturer identifier *
            //Information
            //The number of received data records (called N in this manual)*
            //Record 1 – Value
            //Record 1 – Description of data type (length in bits, how the value is encoded, etc.)*
            //Record 1 – Description of data  (e.g. Energy, Volume, Flow, etc.)*
            //Record 1 – Engineering unit*
            //Record N – Value
            //Record N – Description of data type (length in bits, how the value is encoded, etc...)*
            //Record N – Description of data  (e.g. Energy, Volume, Flow, etc.)*
            //Record N – Engineering unit*
            switch ( address )
            {
              case 0:
              case 1:
              case 2:
                return typeof( string );
              case 3:
                return typeof( int );
              default:
                ulong newaddress = address - 4;
                switch ( newaddress % 4 )
                {
                  case 0:
                    return typeof( double );
                  case 1:
                  case 2:
                  case 3:
                    return typeof( string );
                  default:
                    break;
                }
                break;
            }
            //case MediumData.Class1_Data:
            //case MediumData.Class1_Data_Short:
            break;
          case MediumData.Class2_Data_Short:
            return typeof( double );
        }
        return typeof( object );
      }
    }
    Type[] IItemDefaultSettings.AvailiableTypes
    {
      get
      {
        switch ( addressSpace )
        {
          case MediumData.Class2_Data:
            //case MediumData.Class1_Data:
            //case MediumData.Class1_Data_Short:
            return new Type[] { typeof( object ), typeof( double ), typeof( string ) };
          case MediumData.Class2_Data_Short:
            return new Type[] { typeof( object ), typeof( double ) };
        }
        return new Type[] { typeof( object ) };
      }
    }
    ItemAccessRights IItemDefaultSettings.AccessRights
    {
      get { return ItemAccessRights.ReadOnly; }
    }
    #endregion
    internal ItemDefaultSettings( MediumData AddressSpace, ulong Address )
    {
      addressSpace = AddressSpace;
      address = Address;
    }
  }
}
