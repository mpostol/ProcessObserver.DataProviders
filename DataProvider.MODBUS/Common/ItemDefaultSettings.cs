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
using CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE;
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
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
      get { return addressSpace.ToString().Substring( 0, 1 ) + "/" + address.ToString( "d4" ); }
    }
    Type IItemDefaultSettings.DefaultType
    {
      get
      {
        switch ( addressSpace )
        {
          case Medium_T.Coil:
          case Medium_T.Discrete_input:
            return typeof( bool );
          case Medium_T.Holding_register:
          case Medium_T.Input_register:
          case Medium_T.Register_MemoryBank_CONTROL:
            return typeof( System.Int16 );
          case Medium_T.Holding_8bit_register_CONTROL:
            return typeof( byte );
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
          case Medium_T.Coil:
          case Medium_T.Discrete_input:
            return new Type[] { typeof( object ), typeof( bool ) };
          case Medium_T.Holding_register:
          case Medium_T.Input_register:
          case Medium_T.Register_MemoryBank_CONTROL:
          case Medium_T.Holding_8bit_register_CONTROL:
            return new Type[]{typeof(object),
          typeof(short),
          typeof(ushort),
          typeof(string)
          };
          case Medium_T.Holding_32bit_register:
            return new Type[]{typeof(object),
          typeof(System.Int32),
          typeof(System.UInt32),
          typeof(System.Single)
          };
        }
        return new Type[] { typeof( object ) };
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
