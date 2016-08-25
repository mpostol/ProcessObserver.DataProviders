//<summary>
//  Title   : EC2_3SYM plugin Data Provider Item Default Settings
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
using CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM.PRIVATE;
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM
{
  class ItemDefaultSettings: IItemDefaultSettings
  {
    #region private
    private static readonly string[] flagsNames = new string[] {"SYMEC2EC3/ResetAll",
      "SYMEC2EC3/Initialise",
      "SYMEC2EC3/Fu2LowStop"};
    private static readonly string[] registersNames = new string[]{"SYMEC2EC3/P2",
      "SYMEC2EC3/P3",
      "SYMEC2EC3/PKRZ",
      "SYMEC2EC3/PKRP",
      "SYMEC2EC3/P2KPZ",
      "SYMEC2EC3/P2KPP",
      "SYMEC2EC3/P3KPZ",
      "SYMEC2EC3/P3KPP",
      "SYMEC2EC3/F2U",
      "SYMEC2EC3/F3U",
      "SYMEC2EC3/F2",
      "SYMEC2EC3/F3",
      "SYMEC2EC3/AKRZ",
      "SYMEC2EC3/FUS",
      "SYMEC2EC3/CONF/Pu2",
      "SYMEC2EC3/CONF/Pu3",
      "SYMEC2EC3/CONF/A2u",
      "SYMEC2EC3/CONF/Ao",
      "SYMEC2EC3/CONF/Az",
      "SYMEC2EC3/CONF/cycle",
      "SYMEC2EC3/CONF/F2n",
      "SYMEC2EC3/CONF/F3n",
      "SYMEC2EC3/CONF/P2kpzn",
      "SYMEC2EC3/AKRP",
      "SYMEC2EC3/CONF/P2kppn",
      "SYMEC2EC3/CONF/P3kpzn",
      "SYMEC2EC3/CONF/P3kppn",
      "SYMEC2EC3/CONF/P2zn",
      "SYMEC2EC3/CONF/P3zn",
      "SYMEC2EC3/CONF/P3pn",
      "SYMEC2EC3/CONF/P3Stat",
      "SYMEC2EC3/CONF/Pu",
      "SYMEC2EC3/CONF/R2p",
      "SYMEC2EC3/CONF/R2z",
      "SYMEC2EC3/AKPZ",
      "SYMEC2EC3/CONF/R3p",
      "SYMEC2EC3/CONF/R3z",
      "SYMEC2EC3/CONF/R2o",
      "SYMEC2EC3/CONF/R3o",
      "SYMEC2EC3/CONF/R3u",
      "SYMEC2EC3/CONF/Tdz",
      "SYMEC2EC3/CONF/Tdu",
      "SYMEC2EC3/CONF/F2_other_direction",
      "SYMEC2EC3/CONF/F3_other_direction",
      "SYMEC2EC3/EC2_SUMA_F_WODA",
      "SYMEC2EC3/AKPP",
      "SYMEC2EC3/EC3_SUMA_F_WODA",
      "SYMEC2EC3/AKPZO",
      "SYMEC2EC3/AKPPO",
      "SYMEC2EC3/CONF/RKPC",
      "SYMEC2EC3/CONF/RKRC",
      "SYMEC2EC3/CONF/PUWYS",
      "SYMEC2EC3/Rkpp_act",
      "SYMEC2EC3/Rkpz_act",
      "SYMEC2EC3/Rkrp_act",
      "SYMEC2EC3/Rkrz_act",
      "SYMEC2EC3/P2U",
      "SYMEC2EC3/Ru2",
      "SYMEC2EC3/Pu2_act",
      "SYMEC2EC3/PKPStat",
      "SYMEC2EC3/P3U",
      "SYMEC2EC3/P2Z",
      "SYMEC2EC3/P3Z"};
    private Medium_T addressSpace;
    private ulong address;
    #endregion private
    #region IItemDefaultSettings Members
    string IItemDefaultSettings.Name
    {
      get
      {
        switch ( addressSpace )
        {
          case Medium_T.Flag:
            if ( address < (ulong)flagsNames.Length )
              return flagsNames[ address ];
            break;
          case Medium_T.Register:
            if ( address < (ulong)registersNames.Length )
              return registersNames[ address ];
            break;
        }
        return addressSpace.ToString() + "/" + address.ToString( "d4" );
      }
    }
    Type IItemDefaultSettings.DefaultType
    {
      get
      {
        switch ( addressSpace )
        {
          case Medium_T.Flag:
            return typeof( bool );
          case Medium_T.Register:
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
          case Medium_T.Flag:
            return new Type[] { typeof( object ), typeof( bool ) };
          case Medium_T.Register:
            return new Type[] { typeof( object ), typeof( double ) };
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
    static public string[] FlagsNames
    { get { return flagsNames; } }
    static public string[] RegistersNames
    { get { return registersNames; } }
  }
}
