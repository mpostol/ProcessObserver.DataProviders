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
using CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM2.PRIVATE;
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM2
{
  class ItemDefaultSettings: IItemDefaultSettings
  {
    #region private
    private Medium_T addressSpace;
    private ulong address;
    #endregion private
    #region IItemDefaultSettings Members
    string IItemDefaultSettings.Name
    {
      get { return addressSpace.ToString().Substring( 0, 1 ) + "/" + address.ToString(); }
    }
    Type IItemDefaultSettings.DefaultType
    {
      get
      {
        switch ( addressSpace )
        {
          case Medium_T.Bool:
          case Medium_T.Flag:
          case Medium_T.Input:
          case Medium_T.Output:
            return typeof( bool );
          case Medium_T.Register:
          case Medium_T.Int:
            return typeof( int );
          case Medium_T.String:
            return typeof( string );
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
          case Medium_T.Bool:
          case Medium_T.Flag:
          case Medium_T.Input:
          case Medium_T.Output:
            return new Type[] { typeof( object ), typeof( bool ) };
          case Medium_T.Register:
          case Medium_T.Int:
            return new Type[]{typeof(object),
          typeof(byte),
          typeof(sbyte),
          typeof(int),
          typeof(uint),
          typeof(short),
          typeof(ushort),
          typeof(long),
          typeof(ulong),
          typeof(float),
          typeof(double),
          typeof(decimal),
          typeof(bool),
          typeof(char)
          };
          case Medium_T.String:
            return new Type[] { typeof( object ), typeof( string ) };
        }
        return new Type[]{typeof(object),
          typeof(byte),
          typeof(sbyte),
          typeof(int),
          typeof(uint),
          typeof(short),
          typeof(ushort),
          typeof(long),
          typeof(ulong),
          typeof(float),
          typeof(double),
          typeof(decimal),
          typeof(bool),
          typeof(char)
          };
      }
    }
    ItemAccessRights IItemDefaultSettings.AccessRights
    {
      get { return ItemAccessRights.ReadWrite; }
    }
    #endregion
    internal ItemDefaultSettings( Medium_T AddressSpace, ulong Address )
    {
      addressSpace = AddressSpace;
      address = Address;
    }
  }
}
