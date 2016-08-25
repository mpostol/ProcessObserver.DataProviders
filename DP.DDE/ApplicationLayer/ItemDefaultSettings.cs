//<summary>
//  Title   : NULL plugin Data Provider Item Default Settings
//  System  : Microsoft Visual C# .NET 2005
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
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.DDE
{
  class ItemDefaultSettings: IItemDefaultSettings
  {
    #region private
    private short addressSpace;
    private ulong address;
    #endregion private
    #region IItemDefaultSettings Members
    string IItemDefaultSettings.Name
    {
      get { return DS_DataProviderID.AvailiableAddressSpaces[addressSpace-1].Name +  address.ToString(); }
    }
    Type IItemDefaultSettings.DefaultType
    {
      get
      {
        return typeof( string );
      }
    }
    Type[] IItemDefaultSettings.AvailiableTypes
    {
      get
      {
        return new Type[]{typeof(object),
          typeof(int),
          typeof(uint),
          typeof(short),
          typeof(ushort),
          typeof(double),
          typeof(bool),
          typeof(DateTime),
          };
      }
    }
    ItemAccessRights IItemDefaultSettings.AccessRights
    {
      get { return ItemAccessRights.ReadWrite; }
    }
    #endregion
    internal ItemDefaultSettings( short AddressSpace, ulong Address )
    {
      addressSpace = AddressSpace;
      address = Address;
    }
  }
}
