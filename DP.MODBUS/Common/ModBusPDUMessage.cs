//<summary>
//  Title   : Modbus PDU message implementation
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20081008: mzbrzezny: conversion to unsigned is added
//    20080904: mzbrzezny: adaptation for new umessage that supports returning of information about status of write operation
//    20080826: mzbrzezny: support for CONTROL MicroXL extension is added
//    20080704: mzbrzezny: String in HoldingRegister (16 bits are two character) support is added
//    20080623: mzbrzezny: INPUT_REGISTER support is added
//    20080620: mzbrzezny: DataLength  is added and must be overridden by child message to return value 
//                         based on userDataLength (RS) or based on Length field from MBAP header.
//    20080619: mzbrzezny: Read Discrtet Input is added , tostring is added
//    20080618: mzbrzezny: created and some implementation is imported from ModBusRTUMessage
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using System.Text;
using CAS.Lib.CommonBus.ApplicationLayer.ModBus.Common;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE
{
  /// <summary>
  /// ModBUS PDU message implementation
  /// </summary>
  internal abstract class ModBusPDUMessage: ProtocolALMessage
  {
    private bool registers32AreUsed = false;
    protected ModBus_CommonProtocolParameters MyProtocolParameters { private set; get; }
    protected bool RegisterInFrame;
    protected static CAS.Lib.RTLib.Processes.Assertion myAssert =
      new CAS.Lib.RTLib.Processes.Assertion( "myAssert.Assert error in ModBus_message", 200, false );
    #region private.conversion
    private ushort ConvertShort2UnsigedShort( short i )
    {
      if ( i >= 0 )
        return (ushort)i;
      ushort j = (ushort)( (short)0 - (short)i );
      j = (ushort)~j;
      return (ushort)( j + 1 );
    }
    private short ConvertUnsignedShort2Short( ushort i )
    {
      if ( i <= 0x7FFF )
        return (short)i;
      ushort j = (ushort)~(ushort)( i - 1 );
      return (short)( 0 - (short)j );
    }
    #endregion
    private Single ConvertInt2Float( Int32 i )
    {
      Single ret;
      byte[] bytes = BitConverter.GetBytes( i );
      if ( MyProtocolParameters.FloatingPoint == ModBus_CommonProtocolParameters.FloatingPointType.Standard_Modicon )
        ret = BitConverter.ToSingle( bytes, 0 );
      else
        ret = BitConverter.ToSingle( new byte[] { bytes[ 2 ], bytes[ 3 ], bytes[ 0 ], bytes[ 1 ] }, 0 );
      return ret;
    }
    private Int32 ConvertFloat2int( Single i )
    {
      Int32 ret;
      byte[] bytes = BitConverter.GetBytes( i );
      if ( MyProtocolParameters.FloatingPoint == ModBus_CommonProtocolParameters.FloatingPointType.Standard_Modicon )
        ret = BitConverter.ToInt32( bytes, 0 );
      else
        ret = BitConverter.ToInt32( new byte[] { bytes[ 2 ], bytes[ 3 ], bytes[ 0 ], bytes[ 1 ] }, 0 );
      return ret;
    }
    private object ConvertFromCanonicalType( object source, Type type )
    {
      //TODO:Define of other conversion
      if ( type.Equals( typeof( string ) ) )
      {
        string result = "";
        byte[] bytes = BitConverter.GetBytes( System.Convert.ToInt64( source ) );
        for ( int idx = bytes.Length - 1; idx >= 0; idx-- )
        {
          string newchar = char.ConvertFromUtf32( bytes[ idx ] );
          if ( newchar != "" && newchar != "\0" )
            result += newchar;
        }
        return result;
      }
      if ( type.Equals( typeof( ushort ) ) )
      {
        return this.ConvertShort2UnsigedShort( System.Convert.ToInt16( source ) );
      }
      if ( type.Equals( typeof( float ) ) || type.Equals( typeof( double ) ) )
      {
        return this.ConvertInt2Float( System.Convert.ToInt32( source ) );
      }
      if ( source is short ) //function Read Registers (Input or Holding)
      {
      }
      return source;
    }
    private object ConvertToCanonicalType( object source, Modbus_Functions RequestedDataType )
    {
      switch ( RequestedDataType )
      {
        case Modbus_Functions.WRITE_SINGLE_REGISTER:
          if ( source is string )
          {
            string source_string = source as string;
            //jesli string zbyt dlugi, to go obcinamy:
            if ( source_string.Length > 2 )
              source_string = source_string.Substring( 0, 2 );
            byte[] source_byte_array = ( new ASCIIEncoding() ).GetBytes( source_string );
            //zmieniamy kolejnosc znakow
            byte temp_byte = source_byte_array[ 0 ];
            source_byte_array[ 0 ] = source_byte_array[ 1 ];
            source_byte_array[ 1 ] = temp_byte;
            return BitConverter.ToInt16( source_byte_array, 0 );
          }
          if ( source is ushort )
          {
            return this.ConvertUnsignedShort2Short( System.Convert.ToUInt16( source ) );
          }
          break;
        case Modbus_Functions.WRITE_SINGLE_COIL:
          break;
        case Modbus_Functions.WRITE_MULTIPLE_REGISTERS: // zapis liczb rzeczywistych wymaga zapisu wielu rejestrow
          if ( source.GetType().Equals( typeof( float ) ) || source.GetType().Equals( typeof( double ) ) )
          {
            return this.ConvertFloat2int( Convert.ToSingle( source ) );
          }
          break;
        default:
          break;
      }
      return source;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ModBusPDUMessage"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    /// <param name="myProtocolParameters">My protocol parameters.</param>
    internal ModBusPDUMessage( IBufferLink homePool, ModBus_CommonProtocolParameters myProtocolParameters )
      : base( 300, homePool, true )
    {
      MyProtocolParameters = myProtocolParameters;
    }
    #region abstract place in frame definition
    protected abstract byte stationAddressPos { get; }
    protected abstract byte bankAddressPos { get; } //CONTROL Micro XL extension
    protected abstract byte dataAddressPos { get; }
    protected abstract byte dataTypePos { get; }
    protected abstract byte quantityPos { get; }
    protected abstract byte dataPos { get; }
    protected abstract byte cmdPos { get; }
    protected abstract byte coilWritePos { get; }
    protected abstract byte exceptionPos { get; }
    protected abstract byte reqQuantityPos { get; }
    protected abstract byte WriteSingleValueOffset { get; }
    protected abstract byte WriteMultipleValueOffset { get; }
    protected abstract byte RequestLength { get; }
    protected abstract byte MinimumFrameLength { get; }
    protected abstract byte WriteSingleOrMultipleFrameLength { get; }
    protected abstract byte CRCLengthInBytes { get; }
    protected abstract byte DifferenceBetwwenFrameLengthAndReadObjectQuantity { get; }
    protected abstract int DataLength { get; }
    #endregion abstract place in frame definition
    /// <summary>
    /// Performs the CRC check.
    /// this method should be overriden to perform specifics checks
    /// </summary>
    /// <returns>true - if CRC is ok </returns>
    protected virtual bool PerformCRCCheck()
    {
      if ( CRCLengthInBytes > 0 )
        return false;
      else
        return true; // nie ma CRC
    }
    #region ProtocolALMessage implementation
    /// <summary>
    /// Reads the value.
    /// </summary>
    /// <param name="regAddressOffset">The reg address offset.</param>
    /// <param name="pCanonicalType">the canonical type.</param>
    /// <returns></returns>
    public override object ReadValue( int regAddressOffset, Type pCanonicalType )
    {
      object result = null;
      string info = String.Format( " ReadValue({0})", dataType.ToString() );
      switch ( dataType )
      {
        //2-bytes and 4-bytes variables
        case Modbus_Functions.READ_MEMORYBANK_CONTROL:
        case Modbus_Functions.READ_INPUT_REGISTER:
        case Modbus_Functions.READ_HOLDING_REGISTERS:
          if ( registers32AreUsed )
          {
            // extension allows to read 32bits registers as two 16bits registers
            this.offset = (ushort)( regAddressOffset * 4 + dataPos );
            myAssert.Assert
              ( ( this.offset + 2 ) <= this.userDataLength, (short)( 180 + this.userDataLength * 10 + this.offset ), info );
            byte reg1_1 = this.ReadByte();
            byte reg1_2 = this.ReadByte();
            byte reg2_1 = this.ReadByte();
            byte reg2_2 = this.ReadByte();
            if ( MyProtocolParameters.RegisterOrderIn32mode == ModBus_CommonProtocolParameters.RegisterOrderEnum.Standard_FirstRegisterIsMoreSignificant )
            {
              result = BitConverter.ToInt32( new byte[] { reg1_2, reg1_1, reg2_2, reg2_1 }, 0 );
            }
            else
            {
              result = BitConverter.ToInt32( new byte[] { reg2_2, reg2_1, reg1_2, reg1_1 }, 0 );
            }
          }
          else
          {
            // by default 2-bytes (16bits) registers are used.
            this.offset = (ushort)( regAddressOffset * 2 + dataPos );
            myAssert.Assert
              ( ( this.offset + 2 ) <= this.userDataLength, (short)( 100 + this.userDataLength * 10 + this.offset ), info );
            result = this.ReadInt16();
          }
          break;
        //1-byte variables
        case Modbus_Functions.READ_HOLDING_REGISTERS_8BIT_CONTROL:
          this.offset = (ushort)( regAddressOffset * 1 + dataPos );
          myAssert.Assert
            ( ( this.offset + 1 ) <= this.userDataLength, (short)( 140 + this.userDataLength * 10 + this.offset ), info );
          result = this.ReadByte();
          break;
        //bits variables
        case Modbus_Functions.READ_DISCRETE_INPUT:
        case Modbus_Functions.READ_COILS:
          if ( ( this[ regAddressOffset / 8 + dataPos ] & ( 0x01 << ( regAddressOffset % 8 ) ) ) == 0 )
            result = false;
          else
            result = true;
          break;
        default:
          myAssert.Assert( false, 105, info );
          break;
      }
      return ConvertFromCanonicalType( result, pCanonicalType );
    }
    protected override object ReadCMD( int regAddressOffset ) //odpowiednik getvalue i taką powinien miec nazwę
    {
      object result = null;
      offset = (ushort)( regAddressOffset + cmdPos );
      short val = ReadInt16();
      if ( this.RegisterInFrame )
        result = val;
      else
      {
        if ( val == 0x00FF )
          result = true;
        else
          result = false;
      }
      return result;
    }
    public override int GetCommand
    {
      get { throw ( new NotImplementedException( "The method or operation is not implemented: Modbus: GetCommand." ) ); }
    }
    /// <summary>
    /// Writes the value to the message in the requested type.
    /// If the address space cannot contain values in the type of pValue no conversion is done.
    /// </summary>
    /// <param name="pValue">Value to write.</param>
    /// <param name="pRegAddress">Address</param>
    public override void WriteValue( object pValue, int pRegAddress )
    {
      pValue = ConvertToCanonicalType( pValue, dataType );
      this.offset = WriteSingleValueOffset;
      //TODO wstawic inne funkcje
      switch ( dataType )
      {
        case Modbus_Functions.WRITE_MULTIPLE_REGISTERS:
          this.offset = WriteMultipleValueOffset;
          if ( registers32AreUsed )
          {
            myAssert.Assert( pRegAddress == 0, 296, "In  Modbus_Functions.WRITE_MULTIPLE_REGISTERS reg address must be 0" );
            byte[] bytes = BitConverter.GetBytes( Convert.ToInt32( pValue ) );
            if ( MyProtocolParameters.RegisterOrderIn32mode == ModBus_CommonProtocolParameters.RegisterOrderEnum.Standard_FirstRegisterIsMoreSignificant )
            {
              myAssert.Assert( this.WriteByte( bytes[ 1 ] )
              , 295, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
              myAssert.Assert( this.WriteByte( bytes[ 0 ] )
              , 297, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
              myAssert.Assert( this.WriteByte( bytes[ 3 ] )
              , 299, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
              myAssert.Assert( this.WriteByte( bytes[ 2 ] )
              , 301, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
            }
            else
            {
              myAssert.Assert( this.WriteByte( bytes[ 3 ] )
              , 306, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
              myAssert.Assert( this.WriteByte( bytes[ 2 ] )
              , 308, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
              myAssert.Assert( this.WriteByte( bytes[ 1 ] )
              , 310, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
              myAssert.Assert( this.WriteByte( bytes[ 0 ] )
              , 312, String.Format( "Cannot Write Value WRITE_MULTIPLE_REGISTERS on address: {0}", pRegAddress ) );
            }
          }
          else
          {
            myAssert.Assert( false, 130, "Cannot WRITE_MULTIPLE_REGISTERS in 16 bit mode" );
          }
          break;
        case Modbus_Functions.WRITE_SINGLE_REGISTER:
          if ( registers32AreUsed )
          {
            myAssert.Assert( false, 130, "Cannot WRITE_SINGLE_REGISTER in 32 bit mode" );
            myAssert.Assert( pRegAddress == 0, 296, "In  Modbus_Functions.WRITE_SINGLE_REGISTER reg address must be 0" );
            myAssert.Assert( this.WriteInt32( System.Convert.ToInt32( pValue ) )
              , 198, String.Format( "Cannot Write Value WRITE_SINGLE_REGISTER on address: {0}", pRegAddress ) );
          }
          else
          {
            myAssert.Assert( pRegAddress == 0, 197, "In  Modbus_Functions.WRITE_SINGLE_REGISTER reg address must be 0" );
            myAssert.Assert( this.WriteInt16( System.Convert.ToInt16( pValue ) )
              , 198, String.Format( "Cannot Write Value WRITE_SINGLE_REGISTER on address: {0}", pRegAddress ) );
          }
          break;
        case Modbus_Functions.WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL:
          myAssert.Assert( pRegAddress == 0, 188, "In  Modbus_Functions.WRITE_SINGLE_COIL reg address must be 0" );
          myAssert.Assert( this.WriteByte( System.Convert.ToByte( pValue ) )
            , 204, String.Format( "Cannot Write Value WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL on address: {0}", pRegAddress ) );
          break;
        case Modbus_Functions.WRITE_MEMORYBANK_CONTROL:
          myAssert.Assert( pRegAddress == 0, 110, "In  Modbus_Functions.WRITE_SINGLE_COIL reg address must be 0" );
          this.offset += 2;  //  writing to bank offset is greater than standard request (2 bytes - bank number + byte count ) 
          //TODO: tymczasowo zakladam, ze ilosc jest przesylana w polu 8bit powinno to byc wyjasnione z CONTROL'em
          myAssert.Assert( this.WriteInt16( System.Convert.ToInt16( pValue ) )
            , 211, String.Format( "Cannot Write Value WRITE_MEMORYBANK_CONTROL on address: {0}", pRegAddress ) );
          break;
        case Modbus_Functions.WRITE_SINGLE_COIL:
          myAssert.Assert( pRegAddress == 0, 120, "In  Modbus_Functions.WRITE_SINGLE_COIL reg address must be 0" );
          if ( System.Convert.ToBoolean( pValue ) )
            unchecked
            {
              myAssert.Assert( this.WriteInt16( (short)0xFF00 )
                , 219, String.Format( "Cannot Write Value WRITE_SINGLE_COIL = true on address: {0}", pRegAddress ) );
            }
          else
          {
            myAssert.Assert( this.WriteInt16( (short)0x0000 )
              , 224, String.Format( "Cannot Write Value WRITE_SINGLE_COIL = false on address: {0}", pRegAddress ) );
          }
          break;
        default:
          myAssert.Assert( false, 130, "This write value is not implemented " + dataType.ToString() );
          break;
      }
    }
    protected override void SetValue( object regValue, int regAddressOffset )
    {
      if ( this.RegisterInFrame )
      {
        ushort oldoffset = offset; //MZ: I am not sure whether it is necessary to keep old(previous) offset
        offset = (ushort)( regAddressOffset * 2 + dataPos );
        myAssert.Assert( WriteInt16( (short)regValue ), 238, "ModBus: Cannot set value" );
        offset = (ushort)( oldoffset + 2 );
      }
      else
      {
        ushort oldoffset = offset; //MZ: I am not sure whether it is necessary  to keep old(previous) offset
        offset = coilWritePos;
        if ( (bool)regValue )
          myAssert.Assert( WriteInt16( System.Convert.ToInt16( 0xFF00 ) ), 246, "ModBus: Cannot set value" );
        else
          myAssert.Assert( WriteInt16( System.Convert.ToInt16( 0x0000 ) ), 246, "ModBus: Cannot set value" );
        offset = oldoffset;
      }
    }
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      userDataLength = RequestLength;	// stała długośc zapytania
      this.station = (byte)station;
      registers32AreUsed = false; //by default 16 bit registers are used, this variable as true is used only as extension
      switch ( (Medium_T)block.dataType )
      {
        case Medium_T.Holding_32bit_register:
          dataType = Modbus_Functions.READ_HOLDING_REGISTERS;
          RegisterInFrame = true;
          registers32AreUsed = true;
          break;
        case Medium_T.Holding_register:
          dataType = Modbus_Functions.READ_HOLDING_REGISTERS;
          RegisterInFrame = true;
          break;
        case Medium_T.Input_register:
          dataType = Modbus_Functions.READ_INPUT_REGISTER;
          RegisterInFrame = true;
          break;
        case Medium_T.Holding_8bit_register_CONTROL: //CONTROL Micro XL extension
          dataType = Modbus_Functions.READ_HOLDING_REGISTERS_8BIT_CONTROL;
          RegisterInFrame = true;
          break;
        case Medium_T.Register_MemoryBank_CONTROL: //CONTROL Micro XL extension
          dataType = Modbus_Functions.READ_MEMORYBANK_CONTROL;
          bank = (byte)( ( block.startAddress & 0xFF0000 ) >> 16 );
          RegisterInFrame = true;
          userDataLength++; // request for bank is longer than standard request (1 byte - bank number)
          break;
        case Medium_T.Coil:
          dataType = Modbus_Functions.READ_COILS;
          RegisterInFrame = false;
          break;
        case Medium_T.Discrete_input:
          dataType = Modbus_Functions.READ_DISCRETE_INPUT;
          RegisterInFrame = false;
          break;
        //TODO: dodac inne mozliwosci
        default:
          throw new NotImplementedException( "Not yet implemented" );
      }
      address = (short)( block.startAddress & 0xFFFF ); //MODBUS uses two byte addressing
      //in case of 32bit register the request quatity is different
      if ( (Medium_T)block.dataType == Medium_T.Holding_32bit_register )
        reqQuantity = (short)( ( block.length * 2 ) & 0xFFFF );
      else
        reqQuantity = (short)( block.length & 0xFFFF ); //MODBUS uses two byte length
    }
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      userDataLength = RequestLength; //stala dlugosc zapytania
      this.station = (byte)station;
      registers32AreUsed = false; //by default 16 bit registers are used, this variable as true is used only as extension
      switch ( (Medium_T)block.dataType )
      {
        case Medium_T.Holding_32bit_register:
          dataType = Modbus_Functions.WRITE_MULTIPLE_REGISTERS;
          RegisterInFrame = true;
          registers32AreUsed = true;
          userDataLength += 5;
          // request for writing to multiple registers is longer than standard request 
          // assuming that we want to write 1 32 bit register we have to add 6 bytes:
          // 2 bytes for quantity of registers
          // 2 bytes for byte count
          // 2 bytes because 32 bit register contains 2 more bytes than 16 bit register
          // Request write (multiple): 
          //  Function code 1 Byte 0x10
          //  Starting Address 2 Bytes 0x0000 to 0xFFFF
          //  Quantity of Registers 2 Bytes 0x0001 to 0x0078
          //  Byte Count 1 Byte 2 x N*
          //  Registers Value N* x 2 Bytes value
          // where N - number of registers to be written
          // Request write single:
          //  Function code 1 Byte 0x06
          //  Register Address 2 Bytes 0x0000 to 0xFFFF
          //  Register Value 2 Bytes 0x0000 or 0xFFFF
          //
          // additional settings:
          this.offset = (ushort)( reqQuantityPos ); // Quantity of Registers 2 Bytes 0x0001 to 0x0078
          myAssert.Assert( WriteInt16( 2 ), 449, "ModBus: Cannot write Quantity of Registers" ); // we are writting 2 registers 16 bit
          this.offset = (ushort)( dataAddressPos + 4 ); // Byte Count 1 Byte 2 x N*
          myAssert.Assert( WriteByte( 4 ), 452, "ModBus: Cannot write Quantity of Registers" );// we are writting 2 register 16 bits = 4 bytes
          break;
        case Medium_T.Holding_register:
          dataType = Modbus_Functions.WRITE_SINGLE_REGISTER;
          RegisterInFrame = true;
          break;
        case Medium_T.Holding_8bit_register_CONTROL: //CONTROL Micro XL extension
          dataType = Modbus_Functions.WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL;
          RegisterInFrame = true;
          userDataLength--; // writing 1 byte frame is shorter than standard write
          break;
        case Medium_T.Register_MemoryBank_CONTROL: //CONTROL Micro XL extension
          dataType = Modbus_Functions.WRITE_MEMORYBANK_CONTROL;
          RegisterInFrame = true;
          userDataLength += 2;  // request for writing to bank is longer than standard request (2 bytes - bank number + byte count ) 
          //MZTODO: tymczasowo zakladam, ze ilosc jest przesylana w polu 8bit powinno to byc wyjasnione z CONTROL'em
          bank = (byte)( ( block.startAddress & 0xFF0000 ) >> 16 );
          this[ bankAddressPos + 1 ] = 1;//MZTODO: tymczasowo zakladam, ze ilosc jest przesylana w polu 8bit powinno to byc wyjasnione z CONTROL'em
          break;
        case Medium_T.Coil:
          dataType = Modbus_Functions.WRITE_SINGLE_COIL;
          RegisterInFrame = false;
          break;
        //TODO: dodac inne mozliwosci
        default:
          throw new NotImplementedException( "Not yet implemented" );
      }
      address = (short)( block.startAddress & 0xFFFF ); //MODBUS uses two byte addressing
    }
    #endregion
    #region PUBLIC
    internal bool Registers32AreUsed { get { return registers32AreUsed; } }
    internal Modbus_Functions dataType // function code
    {
      set
      {
        this[ dataTypePos ] = (byte)value;
      }
      get
      {
        return (Modbus_Functions)this[ dataTypePos ];
      }
    }
    internal byte station // slave address
    {
      set
      {
        this[ stationAddressPos ] = value;
      }
      get
      {
        return this[ stationAddressPos ];
      }
    }
    internal byte bank  //CONTROL Micro XL extension
    {
      set
      {
        this[ bankAddressPos ] = value;
      }
      get
      {
        return this[ bankAddressPos ];
      }
    }
    internal short address
    {
      set
      {
        offset = dataAddressPos;
        myAssert.Assert( WriteInt16( value ), 364, "ModBus: Cannot write address" );
      }
      get
      {
        offset = dataAddressPos;
        return ReadInt16();
      }
    }
    internal short reqQuantity
    {
      set
      {
        offset = reqQuantityPos;
        myAssert.Assert( WriteInt16( value ), 377, "ModBus: Cannot write reqQuantity" );
      }
      get
      {
        offset = reqQuantityPos;
        return ReadInt16();
      }
    }
    public override CheckResponseResult CheckResponseFrame( ProtocolALMessage tMes )
    {
      ModBusMessage txmsg = (ModBusMessage)tMes;
      this.registers32AreUsed = txmsg.Registers32AreUsed;
      //out 
      Modbus_Exceptions exception = Modbus_Exceptions.OK;
      if ( this.userDataLength < MinimumFrameLength )
        return CheckResponseResult.CR_Incomplete;
      switch ( this.dataType )
      {
        case Modbus_Functions.READ_COILS:                             //0x01
        case Modbus_Functions.READ_DISCRETE_INPUT:                    //0x02
        case Modbus_Functions.READ_HOLDING_REGISTERS:                 //0x03
        case Modbus_Functions.READ_INPUT_REGISTER:                    //0x04
        case Modbus_Functions.READ_HOLDING_REGISTERS_8BIT_CONTROL:
        case Modbus_Functions.READ_MEMORYBANK_CONTROL:
          if ( this[ quantityPos ] != this.DataLength - DifferenceBetwwenFrameLengthAndReadObjectQuantity )
            return CheckResponseResult.CR_Incomplete;
          break;
        case Modbus_Functions.WRITE_SINGLE_COIL:                      //0x05
        case Modbus_Functions.WRITE_SINGLE_REGISTER:                  //0x06
          if ( this.DataLength != WriteSingleOrMultipleFrameLength )
            return CheckResponseResult.CR_Incomplete;
          break;
        case Modbus_Functions.WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL:
          if ( this.DataLength != WriteSingleOrMultipleFrameLength - 1 ) // we substract 1 because response for writing single register is shorter han standard response
            return CheckResponseResult.CR_Incomplete;
          break;
        case Modbus_Functions.WRITE_MULTIPLE_REGISTERS:
          if ( this.DataLength != WriteSingleOrMultipleFrameLength )
            return CheckResponseResult.CR_Incomplete;
          break;
        case Modbus_Functions.WRITE_MEMORYBANK_CONTROL:
          if ( this.DataLength != WriteSingleOrMultipleFrameLength + 1 ) // we add 1 because of bank field
            return CheckResponseResult.CR_Incomplete;
          break;
        default:
          break;
      }
      if ( !this.PerformCRCCheck() )
        return CheckResponseResult.CR_CRCError;
      this.userDataLength -= CRCLengthInBytes;
      CheckResponseResult res = CheckResponseResult.CR_OK;
      offset = 0;
      if ( (uint)( this.dataType ) <= 0x7F )
      {
        exception = Modbus_Exceptions.OK;
        if ( ( txmsg.station != station ) || ( txmsg.dataType != dataType ) )
          return CheckResponseResult.CR_SynchError;
        switch ( this.dataType )
        {
          case Modbus_Functions.READ_DISCRETE_INPUT:                     //0x02
          case Modbus_Functions.READ_COILS:                              //0x01
            offset = dataPos;
            if ( this[ quantityPos ] != txmsg.reqQuantity / 8 + 1 )
              return CheckResponseResult.CR_SynchError;
            RegisterInFrame = false;
            break;
          case Modbus_Functions.READ_HOLDING_REGISTERS: //0x03
          case Modbus_Functions.READ_INPUT_REGISTER:    //0x04
          case Modbus_Functions.READ_MEMORYBANK_CONTROL:
            if ( this[ quantityPos ] != txmsg.reqQuantity * 2 )
            {
              //             ^
              //w tym polu porównywana jest liczba bajtow, ktora jest dwa razy wieksza od liczby rejsetrzow:
              //Modbus_Application_Protocol_V1_1a.pdf page 15
              return CheckResponseResult.CR_SynchError;
            }
            offset = dataPos;
            RegisterInFrame = true;
            break;
          case Modbus_Functions.READ_HOLDING_REGISTERS_8BIT_CONTROL:
            if ( this[ quantityPos ] != txmsg.reqQuantity )
            {
              //             ^
              //w tym polu porównywana jest liczba bajtow, ktora jest dwa razy wieksza od liczby rejsetrzow:
              //Modbus_Application_Protocol_V1_1a.pdf page 15
              return CheckResponseResult.CR_SynchError;
            }
            offset = dataPos;
            RegisterInFrame = true;
            break;

          case Modbus_Functions.WRITE_SINGLE_COIL:                       //0x05
            if ( ( this[ coilWritePos + 1 ] != 0x0 ) || ( ( this[ coilWritePos ] != 0x00 ) & ( this[ coilWritePos ] != 0xFF ) ) )
              res = CheckResponseResult.CR_Invalid;
            if ( this.DataLength != WriteSingleOrMultipleFrameLength - CRCLengthInBytes )
              res = CheckResponseResult.CR_Invalid;
            break;
          case Modbus_Functions.WRITE_SINGLE_REGISTER:  //0x06
            if ( this.DataLength != WriteSingleOrMultipleFrameLength - CRCLengthInBytes )
              res = CheckResponseResult.CR_Invalid;
            break;
          case Modbus_Functions.WRITE_MULTIPLE_REGISTERS:  //0x10
            if ( this.DataLength != WriteSingleOrMultipleFrameLength - CRCLengthInBytes )
              res = CheckResponseResult.CR_Invalid;
            if ( this.address != txmsg.address || this.reqQuantity != txmsg.reqQuantity )
              res = CheckResponseResult.CR_Invalid;
            break;
          case Modbus_Functions.WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL:
            if ( this.DataLength != WriteSingleOrMultipleFrameLength - CRCLengthInBytes - 1 )//writing sigle 8bit is shorter than standard write resp
              res = CheckResponseResult.CR_Invalid;
            break;
          case Modbus_Functions.WRITE_MEMORYBANK_CONTROL:
            if ( this.DataLength != WriteSingleOrMultipleFrameLength - CRCLengthInBytes + 1 )
              res = CheckResponseResult.CR_Invalid;
            break;
          default:
            res = CheckResponseResult.CR_Invalid;
            break;
        }
      }
      else
      {
        exception = (Modbus_Exceptions)this[ exceptionPos ];
        TraceEvent( TraceEventType.Warning, 328, "ModBUSmessage: the following exception has occured: " + exception.ToString() );
        res = CheckResponseResult.CR_NAK;
      }
      return res;
    }//CheckResponseFrame
    internal Modbus_Exceptions CheckRequestFrame()
    {
      Modbus_Exceptions res;
      byte byte1 = this.ReadByte();
      UInt32 twobytes;
      if ( byte1 >= 0 && byte1 <= 0x7F )
      {
        byte1 = this.ReadByte();
        switch ( byte1 )
        {
          case 0x01: //RC
            twobytes = (UInt16)this.ReadInt16();//adres
            twobytes = (UInt16)this.ReadInt16();//ilosc flag
            if ( twobytes > 0 && twobytes < 2000 )
              res = Modbus_Exceptions.OK;
            else
              res = Modbus_Exceptions.ILLEGAL_DATA_VALUE;
            break;
          case 0x03: //RMR
            twobytes = (UInt16)this.ReadInt16();//adres
            twobytes = (UInt16)this.ReadInt16();//ilosc rejestrow
            if ( twobytes > 0 && twobytes < 126 )
              res = Modbus_Exceptions.OK;
            else
              res = Modbus_Exceptions.ILLEGAL_DATA_VALUE;
            break;
          case 0x05: //WSC
            twobytes = (UInt16)this.ReadInt16();//adres
            twobytes = (UInt16)this.ReadInt16();//wartosc
            if ( twobytes == 0x0000 || twobytes == 0xFF00 )
              res = Modbus_Exceptions.OK;
            else
              res = Modbus_Exceptions.ILLEGAL_DATA_VALUE;
            break;
          case 0x06:
            res = Modbus_Exceptions.OK;
            break;
          default:
            res = Modbus_Exceptions.OK;
            break;
        }
      }
      else
        res = Modbus_Exceptions.ILLEGAL_FUNCTION;
      // ostateczny test czy jest dobre CRC
      if ( res == Modbus_Exceptions.OK )
        if ( !this.PerformCRCCheck() )
          res = Modbus_Exceptions.ILLEGAL_CRC;
      this.offset = 0;
      return res;
    }//CheckRequestFrame
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append( "ModbusPDUMessage:" );
      for ( int i = 0; i < this.userDataLength; i++ )
      {
        sb.Append( this[ i ].ToString( "X" ) );
        sb.Append( " " );
      }
      sb.AppendLine();
      sb.AppendLine( base.ToString() );
      return sb.ToString();
    }
    #endregion PUBLIC
  }
}