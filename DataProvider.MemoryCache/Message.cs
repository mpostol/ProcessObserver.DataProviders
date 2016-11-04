//<summary>
//  Title   : DemoSimulator implementation of   public class Message: ProtocolALMessage
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081007: mzbrzezny: Arrays support is added
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator.PRIVATE;
using System;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{
  /// <summary>
  /// DemoSimulator implementation of   public class Message: ProtocolALMessage
  /// </summary>
  internal class Message: ProtocolALMessage
  {
    #region PRIVATE
    private static Simulator mSym = new Simulator();
    private static object lockobject = new object();
    private static string[] text_array =
          {
            "CAS", 
            "CommServer",
            "DataPorter",
            "OPC Viewer",
            "Unified Architecture",
            "Universe Communication Server",
            "Redundancy",
            "Multiprotocol",
            "Optimal transfer algorithm (OTA)",
            "Adaptive sampling algorithm (ASA)",
            "Multi-DataProviders",
            "Process Observer",
            "OPC",
            "Simulator DataProvider",
            "Hello World",
            "http://www.cas.eu",
            "http://www.commsvr.com",
            "mailto:techsupp@cas.eu",
            "TEL: +48 42' 686 25 47",
            "Copyright (C) 2008, CAS LODZ POLAND."
          };
    private object[] buffor = new object[ 1000 ];
    private static System.Collections.SortedList allCreatedTags = new System.Collections.SortedList();
    private static long HashIndex( int station, int myAddress, short myDataType )
    {
      return ( 256 * myAddress + myDataType ) * 256 + station; // czy to na pewno wystasrczajaca przestrzen danych??
    }
    private System.Random rnd;
    private object ConvertFromCanonicalType( object source, Type type )
    {
      //if ( type == null || source == null )
      //  return source;
      //object ret = null;
      //try
      //{
      //  ret= System.Convert.ChangeType( source, type );
      //}
      //catch (InvalidCastException)
      //{
      //  ret=source;
      //}
      //return ret;
      return source;
    }
    private object ConvertToCanonicalType( object source )
    {
      //TODO:Define of other conversion
      return source;
    }
    #endregion

    #region ProtocolALMessage
    public override void SetBlockDescription( int station, IBlockDescription block )
    {
      base.SetBlockDescription( station, block );
    }
    public override void SetBlockDescription( int station, int address, short myDataType, int length )
    {
      base.SetBlockDescription( station, address, myDataType, length );
    }
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    public override ProtocolALMessage.CheckResponseResult CheckResponseFrame( ProtocolALMessage txmsg )
    {
      return CheckResponseResult.CR_OK;
    }
    /// <summary>
    /// Read the selected contend (value) from the message in the requested type. 
    /// If the address space cannot contain values in this type no conversion is done. 
    /// </summary>
    /// <param name="regAddress">Address</param>
    /// <param name="pCanonicalType">Requested canonical type.</param>
    /// <returns>Converted value.</returns>
    public override object ReadValue( int regAddress, Type pCanonicalType )
    {
      return ConvertFromCanonicalType( buffor[ regAddress ], pCanonicalType );
    }
    protected override object ReadCMD( int regAddress )
    {
      return buffor[ regAddress ];
    }
    public override int GetCommand
    {
      get { throw new Exception( "The method or operation is not implemented." ); }
    }
    /// <summary>
    /// Writes the value to the message in the requested type. 
    /// If the address space cannot contain values in the type of pValue no conversion is done.
    /// </summary>
    /// <param name="pValue">Value to write.</param>
    /// <param name="pRegAddress">Address</param>
    public override void WriteValue( object pValue, int pRegAddress )
    {
      buffor[ pRegAddress ] = ConvertToCanonicalType( pValue );
    }
    protected override void SetValue( object regValue, int regAddress )
    {
      buffor[ regAddress ] = regValue;
    }
    #endregion

    #region PUBLIC
    internal void setDBTag( int _station, int _address, short _datatype, object _value )
    {
      lock ( lockobject )
      {
        int curIdx = allCreatedTags.IndexOfKey( HashIndex( _station, _address, _datatype ) );
        if ( curIdx < 0 )
          allCreatedTags.Add
            ( HashIndex( _station, _address, _datatype ),
            _value
            );
        else
          allCreatedTags.SetByIndex( curIdx, _value );
      }
    }
    internal void ReadFromDB()
    {
      lock ( lockobject )
      {
        //dodatkowe oczekiwanie
        int timetosleep = Convert.ToInt32( mSym.values[ (int)Simulator.signalsIdx.TransmissionDelayInMs ] / 2 ) +
          ( rnd.Next( Convert.ToInt32( mSym.values[ (int)Simulator.signalsIdx.TransmissionDelayInMs ] ) ) );
        System.Threading.Thread.Sleep( timetosleep );
        for
          ( int idx = ( (IBlockDescription)this ).startAddress;
          idx < ( (IBlockDescription)this ).startAddress + ( (IBlockDescription)this ).length; idx++
          )
        {
          long hashindex = HashIndex( ( (IBlockAddress)this ).station, idx, ( (IBlockDescription)this ).dataType );
          int curIdx = allCreatedTags.IndexOfKey( hashindex );
          object currVal = null;
          if ( curIdx < 0 )
          {
            #region switch(((IBlockDescription)this).dataType)
            switch ( (Medium_T)( (IBlockDescription)this ).dataType )
            {
              case Medium_T.SimulationRegister:
                if ( idx < mSym.values.Length )
                  currVal = mSym.values[ idx ];
                else
                  currVal = (double)0;
                break;
              case Medium_T.SimulationFlags:
                if ( idx < mSym.valuesCMD.Length )
                  currVal = mSym.valuesCMD[ idx ];
                else
                  currVal = false;
                break;
              case Medium_T.Flag:
              case Medium_T.Input:
              case Medium_T.Output:
                {
                  currVal = false;
                  break;
                }
              case Medium_T.Counter:
              case Medium_T.Register:
              case Medium_T.Timer:
                {
                  currVal = (int)0;
                  break;
                }
              case Medium_T.SByte:
                currVal = (sbyte)0;
                break;
              case Medium_T.Byte:
                currVal = (byte)0;
                break;
              case Medium_T.UShort:
                currVal = (ushort)0;
                break;
              case Medium_T.Int:
                currVal = (int)0;
                break;
              case Medium_T.Uint:
                currVal = (uint)0;
                break;
              case Medium_T.Long:
                currVal = (long)0;
                break;
              case Medium_T.Ulong:
                currVal = (ulong)0;
                break;
              case Medium_T.Float:
                currVal = (float)0;
                break;
              case Medium_T.Double:
                currVal = (double)0;
                break;
              case Medium_T.Decimal:
                currVal = (decimal)0;
                break;
              case Medium_T.Bool:
                currVal = (bool)false;
                break;
              case Medium_T.DateTime:
                currVal = DateTime.Now;
                break;
              case Medium_T.TimeSpan:
                currVal = TimeSpan.Zero;
                break;
              case Medium_T.String:
                currVal = (string)"";
                break;
              case Medium_T.Object:
                currVal = new object();
                break;
              case Medium_T.ArrayOfDoulbes:
                currVal = new double[] { (double)0.0, (double)0.0, (double)0.0 };
                break;
              case Medium_T.ArrayOfInts:
                currVal = new int[] { 0, 0, 0 };
                break;
              case Medium_T.ArrayOfStrings:
                currVal = new string[] { "", "", "" };
                break;
            }
            #endregion
            allCreatedTags.Add( hashindex, currVal );
          }
          else
            try
            {
              switch ( (Medium_T)( (IBlockDescription)this ).dataType )
              {
                case Medium_T.SimulationRegister:
                  if ( idx < mSym.values.Length )
                    currVal = mSym.values[ idx ];
                  else
                    currVal = (double)0;
                  break;
                  break;
                case Medium_T.SimulationFlags:
                  if ( idx < mSym.valuesCMD.Length )
                    currVal = mSym.valuesCMD[ idx ];
                  else
                    currVal = false;
                  break;
                  break;
                default:
                  currVal = allCreatedTags.GetByIndex( curIdx );
                  break;
              }
            }
            catch ( Exception ex )
            {
              throw new Exception( "allCreatedTags.GetByIndex( curIdx ):" + ex.Message );
            }
          //tutaj zrobimy podzial na zmienne statyczne i dynamiczne - statyczne beda mialy adresy ponizej 1000
          //dynamiczne - adres powyzej 1000
          try
          {
            if ( ( (IBlockDescription)this ).startAddress < 1000 )
            {
              buffor[ idx - ( (IBlockDescription)this ).startAddress ] = currVal;
            }
            else
            {
              //bedziemy losowac
              #region switch(((IBlockDescription)this).dataType)
              switch ( (Medium_T)( (IBlockDescription)this ).dataType )
              {
                case Medium_T.Flag:
                case Medium_T.Input:
                case Medium_T.Output:
                  {
                    currVal = !(bool)currVal;
                    break;
                  }
                case Medium_T.Counter:
                case Medium_T.Register:
                case Medium_T.Timer:
                  {
                    currVal = (int)rnd.Next( Int16.MaxValue );
                    break;
                  }
                case Medium_T.SByte:
                  currVal = (sbyte)rnd.Next( SByte.MaxValue );
                  break;
                case Medium_T.Byte:
                  currVal = (byte)rnd.Next( Byte.MaxValue );
                  break;
                case Medium_T.UShort:
                  currVal = (ushort)rnd.Next( UInt16.MaxValue );
                  break;
                case Medium_T.Int:
                  currVal = (int)rnd.Next( Int16.MaxValue );
                  break;
                case Medium_T.Uint:
                  currVal = (uint)rnd.Next( UInt16.MaxValue );
                  break;
                case Medium_T.Long:
                  currVal = (long)rnd.Next( Int32.MaxValue );
                  break;
                case Medium_T.Ulong:
                  currVal = (ulong)rnd.Next( UInt16.MaxValue );
                  break;
                case Medium_T.Float:
                  currVal = (float)( rnd.NextDouble() * 100.0 );
                  break;
                case Medium_T.Double:
                  currVal = (double)( rnd.NextDouble() * 100.0 );
                  break;
                case Medium_T.Decimal:
                  currVal = (decimal)0;
                  break;
                case Medium_T.Bool:
                  currVal = (bool)false;
                  break;
                case Medium_T.DateTime:
                  currVal = new DateTime( rnd.Next( 200 ) + 1900, rnd.Next( 11 ) + 1, rnd.Next( 27 ) + 1, rnd.Next( 24 ), rnd.Next( 60 ), rnd.Next( 60 ) );
                  break;
                case Medium_T.TimeSpan:
                  currVal = new TimeSpan( rnd.Next( 24 ), rnd.Next( 60 ), rnd.Next( 60 ) );
                  break;
                case Medium_T.String:
                  currVal = (string)text_array[ rnd.Next( text_array.Length ) ];
                  break;
                case Medium_T.Object:
                  currVal = new object();
                  break;
                case Medium_T.ArrayOfDoulbes:
                  currVal = new double[ rnd.Next( 10 ) + 1 ];
                  for ( int myidx = 0; myidx < ( (double[])currVal ).Length; myidx++ )
                    ( (double[])currVal )[ myidx ] = (double)( rnd.NextDouble() * 100.0 );
                  break;
                case Medium_T.ArrayOfInts:
                  int[] values_ints = new int[ rnd.Next( 10 ) + 1 ];
                  for ( int myidx = 0; myidx < values_ints.Length; myidx++ )
                    values_ints[ myidx ] = (int)rnd.Next( Int16.MaxValue );
                  currVal = values_ints;
                  break;
                case Medium_T.ArrayOfStrings:
                  string[] values_strings = new string[ rnd.Next( 10 ) + 1 ];
                  for ( int myidx = 0; myidx < values_strings.Length; myidx++ )
                    values_strings[ myidx ] = (string)text_array[ rnd.Next( text_array.Length ) ];
                  currVal = values_strings;
                  break;
              }
              #endregion
              buffor[ idx - ( (IBlockDescription)this ).startAddress ] = currVal;
            }
          }
          catch ( Exception ex )
          {
            throw new System.Exception( "buffor[ idx - ( (IBlockDescription)this ).startAddress ]:" + ex.Message );
          }

        }
      }
    }
    internal void WriteToDB()
    {
      lock ( lockobject )
      {
        System.Threading.Thread.Sleep( Convert.ToInt32( mSym.values[ (int)Simulator.signalsIdx.TransmissionDelayInMs ] ) );
        for
          ( int idx = ( (IBlockDescription)this ).startAddress;
          idx < ( (IBlockDescription)this ).startAddress + ( (IBlockDescription)this ).length;
          idx++ )
        {
          switch ( (Medium_T)( (IBlockDescription)this ).dataType )
          {
            case Medium_T.SimulationRegister:
              if ( idx < mSym.values.Length )
                mSym.values[ idx ] = (double)buffor[ idx - ( (IBlockDescription)this ).startAddress ];
              break;
            case Medium_T.SimulationFlags:
              if ( idx < mSym.valuesCMD.Length )
                mSym.valuesCMD[ idx ] = (bool)buffor[ idx - ( (IBlockDescription)this ).startAddress ];
              break;
            default:
              int curIdx = allCreatedTags.IndexOfKey( HashIndex( ( (IBlockAddress)this ).station, idx, ( (IBlockDescription)this ).dataType ) );
              if ( curIdx < 0 )
                allCreatedTags.Add
                  ( HashIndex( ( (IBlockAddress)this ).station, idx, ( (IBlockDescription)this ).dataType ),
                  buffor[ idx - ( (IBlockDescription)this ).startAddress ]
                  );
              else
                allCreatedTags.SetByIndex( curIdx, buffor[ idx - ( (IBlockDescription)this ).startAddress ] );
              break;
          }

        }
      }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="Message"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    internal Message( CommunicationLayer.Generic.IBufferLink homePool )
      : base( 30, homePool, false )
    {
      rnd = new Random( System.DateTime.Now.Millisecond );
    }
    #endregion
    internal bool TestCommunication( int station )
    {
      return mSym.TestCommunication( station );
    }
    internal void TransmitterON( int station )
    {
      mSym.TransmitterON( station );
    }
    internal void TransmitterOFF( int station )
    {
      mSym.TransmitterOFF( station );
    }
  }
}