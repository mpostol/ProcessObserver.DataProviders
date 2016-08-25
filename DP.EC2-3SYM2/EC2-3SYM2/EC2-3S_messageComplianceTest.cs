//<summary>
//  Title   : ApplicationLayer.EC2_3SYM.NullMessage
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM2.PRIVATE;

namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM2
{
  /// <summary>
  /// Summary description for NULL_message.
  /// </summary>
  internal class NULL_message: ProtocolALMessage
  {
    #region PRIVATE
    private static Symulator mSym = new Symulator();
    private object[] buffor = new object[ 100 ];
    private static int HashIndex( int station, int myAddress, Medium_T myDataType )
    {
      return myAddress;
    }
    private object ConvertToCanonicalType( object p, Type pCanonicalType )
    {
      return p;
    }
    #endregion
    #region ProtocolALMessage
    public override ProtocolALMessage.CheckResponseResult CheckResponseFrame( ProtocolALMessage txmsg )
    {
      return CheckResponseResult.CR_OK;
    }
    protected override void PrepareRequest( int station, IBlockDescription block ){}
    protected override void PrepareReqWriteValue( IBlockDescription block, int station ){}
    /// <summary>
    /// Read the selected contend (value) from the message in the requested type. 
    /// If the address space cannot contain values in this type no conversion is done. 
    /// </summary>
    /// <param name="regAddress">Address</param>
    /// <param name="pCanonicalType">Requested canonical type.</param>
    /// <returns>Converted value.</returns>
    public override object ReadValue( int regAddress, Type pCanonicalType )
    {
      return ConvertToCanonicalType( buffor[ regAddress ], pCanonicalType );
    }
    protected override object ReadCMD( int regAddress )
    {
      return buffor[ regAddress ];
    }
    public override int GetCommand
    {
      get { throw new Exception( "The method or operation is not implemented." ); }
    }
    public override void WriteValue( object regValue, int regAddress )
    {
      buffor[ regAddress ] = regValue;
    }
    protected override void SetValue( object regValue, int regAddress )
    {
      buffor[ regAddress ] = regValue;
    }
    #endregion
    #region PUBLIC
    internal void ReadFromDB()
    {
      for
        ( int idx = ( (IBlockDescription)this ).startAddress;
        idx < ( (IBlockDescription)this ).startAddress + ( (IBlockDescription)this ).length; idx++
        )
      {
        int curIdx = HashIndex( ( (IBlockAddress)this ).station, idx, (Medium_T)( (IBlockDescription)this ).dataType );
        object currVal = null;
        if ( curIdx < mSym.values.Length )
        {
          switch ((Medium_T)((IBlockDescription)this).dataType)
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
                currVal = Convert.ToInt32( mSym.values[ curIdx ] );
                break;
              }
          }
        }
        else
          if ( curIdx < mSym.values.Length )
          {
            switch ((Medium_T)((IBlockDescription)this).dataType)
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
                  currVal = Int32.MinValue;
                  break;
                }
            }
          }
        buffor[ idx - ( (IBlockDescription)this ).startAddress ] = currVal;
      }
    }
    internal void WriteToDB()
    {
      for
        ( int idx = ( (IBlockDescription)this ).startAddress;
        idx < ( (IBlockDescription)this ).startAddress + ( (IBlockDescription)this ).length;
        idx++ )
      {
        int curIdx = HashIndex(((IBlockAddress)this).station, idx, (Medium_T)((IBlockDescription)this).dataType);
        if ( curIdx < mSym.values.Length )
          mSym.values[ curIdx ] = Convert.ToDouble( buffor[ idx - ( (IBlockDescription)this ).startAddress ] );
      }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NULL_message"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    internal NULL_message( CommunicationLayer.Generic.IBufferLink homePool )
      : base( 30, homePool, false ) { }
    #endregion
  }
}