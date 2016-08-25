//<summary>
//  Title   : ApplicationLayer.EC2_3SYM.NullMessage
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2004: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.EC2_3SYM
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
    private object ConvertFromCanonicalType( object source, Type type )
    {
      //TODO:Define of other conversion
      return source;
    }
    private object ConvertToCanonicalType( object source )
    {
      //TODO:Define of other conversion
      return source;
    }
    #endregion
    #region ProtocolALMessage
    public override ProtocolALMessage.CheckResponseResult CheckResponseFrame( ProtocolALMessage txmsg )
    {
      return CheckResponseResult.CR_OK;
    }
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      throw new Exception( "The method or operation is not implemented." );
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
        int curIdx = HashIndex(
          ( (IBlockAddress)this ).station,
          idx,
          (Medium_T)( (IBlockDescription)this ).dataType
                                     );
        object currVal = null;
        if ( curIdx < mSym.values.Length )
        {
          switch ( (Medium_T)( (IBlockDescription)this ).dataType )
          {
            case Medium_T.Flag:
              {
                currVal = mSym.valuesCMD[ curIdx ];
                break;
              }
            case Medium_T.Register:
              {
                //              double limVal = Math.Max( mSym.values[curIdx], Int32.MinValue);
                //              limVal = Math.Min(limVal, Int32.MaxValue);
                //              currVal = Convert.ToInt32( limVal );
                currVal = mSym.values[ curIdx ];
                break;
              }
          }
        }
        else
          if ( curIdx < mSym.values.Length )
          {
            switch ( (Medium_T)( (IBlockDescription)this ).dataType )
            {
              case Medium_T.Flag:
                {
                  currVal = false;
                  break;
                }
              case Medium_T.Register:
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
        int curIdx = HashIndex( ( (IBlockAddress)this ).station, idx, (Medium_T)( (IBlockDescription)this ).dataType );
        if ( curIdx < mSym.values.Length )
        {
          switch ( (Medium_T)( (IBlockDescription)this ).dataType )
          {
            case Medium_T.Flag:
              {
                mSym.valuesCMD[ curIdx ] = Convert.ToBoolean( buffor[ idx - ( (IBlockDescription)this ).startAddress ] );
                break;
              }
            case Medium_T.Register:
              {
                mSym.values[ curIdx ] = Convert.ToDouble( buffor[ idx - ( (IBlockDescription)this ).startAddress ] );
                break;
              }
          }

        }
      }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NULL_message"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    public NULL_message( IBufferLink homePool )
      : base( 30, homePool, false )
    { }
    #endregion

  }
}