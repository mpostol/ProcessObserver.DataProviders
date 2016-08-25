//<summary>
//  Title   : NULL implementation of   public class NULL_message: ProtocolALMessage
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MZbrzezny - 2008- 01 poprawki ze static i lockami
//    MZbrzezny - 07-09-2005: dodano typy danych dla symulatora
//    MZbrzezny - 03-06-2005: dodano obsluge pluginow
//    MPostol - 24-10-2003: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.NULL.PRIVATE;

namespace CAS.Lib.CommonBus.ApplicationLayer.NULL
{
  /// <summary>
  /// Summary description for NULL_message.
  /// </summary>
  internal class NULL_message: ProtocolALMessage
  {
    #region PRIVATE
    private static object lockobject = new object();
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
                //TODO wrocic do tego              currVal = DateTime.UtcNow;
                currVal = DateTime.Now;
                break;
              case Medium_T.TimeSpan:
                currVal = TimeSpan.Zero;
                break;
              case Medium_T.String:
                currVal = (string)"CAS OPC Svr";
                break;
              case Medium_T.Object:
                currVal = new object();
                break;
            }
            #endregion
            allCreatedTags.Add( hashindex, currVal );
          }
          else
            try
            {
              currVal = allCreatedTags.GetByIndex( curIdx );
            }
            catch ( Exception ex )
            {
              throw new Exception( "allCreatedTags.GetByIndex( curIdx ):" + ex.Message );
            }
          //tutaj zrobimy podzial na zmienne statyczne i dynamiczne - statyczne beda mialy adresy ponizej 1000
          //dynamiczne - adres powyzej 1000
          try
          {
            buffor[ idx - ( (IBlockDescription)this ).startAddress ] = currVal;
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
        for
          ( int idx = ( (IBlockDescription)this ).startAddress;
          idx < ( (IBlockDescription)this ).startAddress + ( (IBlockDescription)this ).length;
          idx++ )
        {
          int curIdx = allCreatedTags.IndexOfKey( HashIndex( ( (IBlockAddress)this ).station, idx, ( (IBlockDescription)this ).dataType ) );
          if ( curIdx < 0 )
            allCreatedTags.Add
              ( HashIndex( ( (IBlockAddress)this ).station, idx, ( (IBlockDescription)this ).dataType ),
              buffor[ idx - ( (IBlockDescription)this ).startAddress ]
              );
          else
            allCreatedTags.SetByIndex( curIdx, buffor[ idx - ( (IBlockDescription)this ).startAddress ] );
        }
      }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NULL_message"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    internal NULL_message( CommunicationLayer.Generic.IBufferLink homePool )
      : base( 250, homePool, false )
    {
      rnd = new Random( System.DateTime.Now.Millisecond );
    }
    #endregion
  }
}