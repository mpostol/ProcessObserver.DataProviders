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
using CAS.Lib.CommonBus.ApplicationLayer.NULL.PRIVATE;
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.NULL
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
      get { return addressSpace.ToString() + "/" + address.ToString( "d4" ); }
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
            return typeof( int );
          case Medium_T.String:
            return typeof( string );
          case Medium_T.Byte:
            return typeof( System.Byte );
          case Medium_T.SByte:
            return typeof( System.SByte );
          case Medium_T.Short:
            return typeof( System.Int16 );
          case Medium_T.UShort:
            return typeof( System.UInt16 );
          case Medium_T.Int:
            return typeof( System.Int32 );
          case Medium_T.Uint:
            return typeof( System.UInt32 );
          case Medium_T.Long:
            return typeof( System.Int64 );
          case Medium_T.Ulong:
            return typeof( System.UInt64 );
          case Medium_T.DateTime:
            return typeof( System.DateTime );
          case Medium_T.Float:
            return typeof( float );
          case Medium_T.Double:
            return typeof( double );
          case Medium_T.Object:
            return typeof( object );
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
          typeof(DateTime),
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
