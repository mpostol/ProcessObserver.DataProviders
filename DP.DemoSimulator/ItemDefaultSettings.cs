//<summary>
//  Title   : DemoSimulator plugin Data Provider Item Default Settings
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20081007: mzbrzezny: Arrays support is added
//    20081002 mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator.PRIVATE;
using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{
  class ItemDefaultSettings: IItemDefaultSettings
  {
    #region private
    private static readonly string[] flagsNames = new string[] {
      "Simulator/Commands/ResetAll",
      "Simulator/Commands/ResetToCurrent",
      "Simulator/Model/Alarm/SwitchON",
      "Simulator/Model/Alarm/SwitchOFF",
      "Simulator/Routes/1/setActive",
      "Simulator/Routes/2/setActive",
      "Simulator/Commands/ResetTime",
      "Simulator/Routes/1/TrasmitON",
      "Simulator/Routes/2/TrasmitON"};
    private static readonly string[] registersNames = new string[]{
      "Simulator/Model/Tanks/1/h/cv",
      "Simulator/Model/Tanks/2/h/cv",
      "Simulator/Model/Tanks/3/h/cv",
      "Simulator/Model/Tanks/4/h/cv",
      "Simulator/Model/Pumps/1/V/set",
      "Simulator/Model/Pumps/2/V/set",
      "Simulator/Model/Tanks/1/Config/nozzle/area",
      "Simulator/Model/Tanks/2/Config/nozzle/area",
      "Simulator/Model/Tanks/3/Config/nozzle/area",
      "Simulator/Model/Tanks/4/Config/nozzle/area",
      "Simulator/Model/Tanks/1/Config/area",
      "Simulator/Model/Tanks/2/Config/area",
      "Simulator/Model/Tanks/3/Config/area",
      "Simulator/Model/Tanks/4/Config/area",
      "Simulator/Model/Valves/1/r/set",
      "Simulator/Model/Valves/2/r/set",
      "Simulator/Model/Pumps/1/Config/k",
      "Simulator/Model/Pumps/2/Config/k",
      "Simulator/Model/Tanks/1/h/previuos",
      "Simulator/Model/Tanks/2/h/previuos",
      "Simulator/Model/Tanks/3/h/previuos",
      "Simulator/Model/Tanks/4/h/previuos",
      "Simulator/Model/Config/g",
      "Simulator/Config/cycle",
      "Simulator/Signals/Wawes/Saw-Toothed/y",
      "Simulator/Signals/Wawes/Sinus/y",
      "Simulator/Signals/Wawes/Sinus/Config/A",
      "Simulator/Signals/Wawes/Saw-Toothed/Config/A",
      "Simulator/Internals/t",
      "Simulator/Signals/Config/w",
      "Simulator/Signals/Config/b",
      "Simulator/Model/Tanks/1/h/delta",
      "Simulator/Model/Tanks/2/h/delta",
      "Simulator/Model/Tanks/3/h/delta",
      "Simulator/Model/Tanks/4/h/delta",
      "Simulator/Signals/Wawes/Saw-Toothed/Config/mn",
      "Simulator/Model/Alarm/delta/set",
      "Simulator/Internals/DateTime/Year",
      "Simulator/Internals/DateTime/Month",
      "Simulator/Internals/DateTime/Day",
      "Simulator/Internals/DateTime/Hour",
      "Simulator/Internals/DateTime/Minute",
      "Simulator/Internals/DateTime/Second",
      "Simulator/Internals/TrasmissionDelay/set"
};
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
          case Medium_T.SimulationFlags:
            if ( address < (ulong)flagsNames.Length )
              return flagsNames[ address ];
            break;
          case Medium_T.SimulationRegister:
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
          case Medium_T.Bool:
          case Medium_T.Flag:
          case Medium_T.SimulationFlags:
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
          case Medium_T.TimeSpan:
            return typeof( TimeSpan );
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
          case Medium_T.SimulationRegister:
            return typeof( double );
          case Medium_T.ArrayOfInts:
            return typeof( int[] );
          case Medium_T.ArrayOfStrings:
            return typeof( string[] );
          case Medium_T.ArrayOfDoulbes:
            return typeof( double[] );
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
          case Medium_T.SimulationFlags:
          case Medium_T.Input:
          case Medium_T.Output:
            return new Type[] { typeof( object ), typeof( bool ), typeof( string ) };
          case Medium_T.SimulationRegister:
            return new Type[] { typeof( object ), typeof( double ) };
          case Medium_T.TimeSpan:
            return new Type[] { typeof( object ), typeof( TimeSpan ), typeof( string ) };
          case Medium_T.DateTime:
            return new Type[] { typeof( object ), typeof( DateTime ), typeof( string ) };
          case Medium_T.ArrayOfInts:
            return new Type[] { typeof( int[] ) };
          case Medium_T.ArrayOfStrings:
            return new Type[] { typeof( string[] ) };
          case Medium_T.ArrayOfDoulbes:
            return new Type[] { typeof( double[] ) };
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
      get
      {
        switch ( addressSpace )
        {
          case Medium_T.SimulationFlags:
            if ( address < (ulong)flagsNames.Length &&
              ( flagsNames[ address ].ToLower().Contains( "config" ) ||
              flagsNames[ address ].ToLower().Contains( "set" ) ||
              flagsNames[ address ].ToLower().Contains( "commands" ) ) )
              return ItemAccessRights.ReadWrite;
            return ItemAccessRights.ReadOnly;
          case Medium_T.SimulationRegister:
            if ( address < (ulong)registersNames.Length &&
              ( registersNames[ address ].ToLower().Contains( "config" ) ||
              registersNames[ address ].ToLower().Contains( "set" ) ||
              registersNames[ address ].ToLower().Contains( "commands" ) ) )
              return ItemAccessRights.ReadWrite;
            return ItemAccessRights.ReadOnly;
        }
        return ItemAccessRights.ReadWrite;
      }
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
