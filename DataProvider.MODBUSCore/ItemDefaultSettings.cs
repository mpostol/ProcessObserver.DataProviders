//_______________________________________________________________
//  Title   : NULL plugin Data Provider Item Default Settings
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.Lib.CommonBus;
using CAS.Lib.RTLib;
using System;

namespace CAS.CommServer.DataProvider.MODBUSCore
{
  public class ItemDefaultSettings : IItemDefaultSettings
  {

    #region private
    private Medium_T m_AddressSpace;
    private ulong m_Address;
    #endregion private

    #region IItemDefaultSettings Members
    string IItemDefaultSettings.Name
    {
      get { return m_AddressSpace.ToString().Substring(0, 1) + "/" + m_Address.ToString("d4"); }
    }
    Type IItemDefaultSettings.DefaultType
    {
      get
      {
        switch (m_AddressSpace)
        {
          case Medium_T.Coil:
          case Medium_T.Discrete_input:
            return typeof(bool);
          case Medium_T.Holding_register:
          case Medium_T.Input_register:
          case Medium_T.Register_MemoryBank_CONTROL:
            return typeof(short);
          case Medium_T.Holding_8bit_register_CONTROL:
            return typeof(byte);
        }
        return typeof(object);
      }
    }
    Type[] IItemDefaultSettings.AvailiableTypes
    {
      get
      {
        switch (m_AddressSpace)
        {
          case Medium_T.Coil:
          case Medium_T.Discrete_input:
            return new Type[] { typeof(object), typeof(bool) };
          case Medium_T.Holding_register:
          case Medium_T.Input_register:
          case Medium_T.Register_MemoryBank_CONTROL:
          case Medium_T.Holding_8bit_register_CONTROL:
            return new Type[] { typeof(object), typeof(short), typeof(ushort), typeof(string) };
          case Medium_T.Holding_32bit_register:
            return new Type[] { typeof(object), typeof(int), typeof(uint), typeof(float) };
        }
        return new Type[] { typeof(object) };
      }
    }
    ItemAccessRights IItemDefaultSettings.AccessRights
    {
      get { return ItemAccessRights.ReadWrite; }
    }
    #endregion

    #region creator
    public ItemDefaultSettings(Medium_T AddressSpace, ulong Address)
    {
      m_AddressSpace = AddressSpace;
      m_Address = Address;
    }
    #endregion
  }
}
