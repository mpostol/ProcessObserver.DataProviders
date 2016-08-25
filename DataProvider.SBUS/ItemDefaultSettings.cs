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
//  Copyright (C)2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS
{
  class ItemDefaultSettings: IItemDefaultSettings
  {
    #region private
    private Medium_T m_AddressSpace;
    private ulong m_Address;
    #endregion private

    #region IItemDefaultSettings Members
    string IItemDefaultSettings.Name
    {
      get { return m_AddressSpace.ToString().Substring( 0, 1 ) + "/" + m_Address.ToString( "d4" ); }
    }
    Type IItemDefaultSettings.DefaultType
    {
      get
      {
        switch ( m_AddressSpace )
        {
          case Medium_T.Flag:
          case Medium_T.Input:
          case Medium_T.Output:
            return typeof( bool );
          case Medium_T.Register:
          case Medium_T.Timer:
          case Medium_T.Counter:
            return typeof( System.Int32 );
          case Medium_T.Text:
            return typeof( string );
        }
        throw new ApplicationException( "addressSpace is out of scope" );
      }
    }
    Type[] IItemDefaultSettings.AvailiableTypes
    {
      get
      {
        switch ( m_AddressSpace )
        {
          case Medium_T.Flag:
          case Medium_T.Input:
          case Medium_T.Output:
            return new Type[] { typeof( object ), typeof( bool ) };
          case Medium_T.Register:
          case Medium_T.Timer:
          case Medium_T.Counter:
            return new Type[]
            {
              typeof(object),
              typeof(System.Int32),
              typeof(System.UInt32),
              typeof(System.Int16),
              typeof(System.UInt16),
              typeof(float)
            };
          case Medium_T.Text:
            return new Type[] { typeof( string ) };
          default:
            return new Type[] { typeof( object ) };
        }
      }
    }
    ItemAccessRights IItemDefaultSettings.AccessRights
    {
      get
      {
        switch ( m_AddressSpace )
        {
          case Medium_T.Input:
            return ItemAccessRights.ReadOnly;
          case Medium_T.Output:
            return ItemAccessRights.WriteOnly;
          default:
            return ItemAccessRights.ReadWrite;
        }
      }
    }
    #endregion
    internal ItemDefaultSettings( Medium_T AddressSpace, ulong Address )
    {
      m_AddressSpace = AddressSpace;
      m_Address = Address;
    }
  }
}
