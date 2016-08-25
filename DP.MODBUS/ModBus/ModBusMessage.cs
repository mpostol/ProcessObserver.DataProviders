//<summary>
//  Title   : Modbus RTU message implementation
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080826: mzbrzezny: support for CONTROL MicroXL extension is added
//    20080620: mzbrzezny: DataLength  is added and mus be overridden by child message to return value 
//              based on userDataLength (RS) or based on Length field from MBAP header.
//    20080618: mzbrzezny: some implementation is extracted to ModBusPDUMessage
//    20080618: mzbrzezny: CRC is extracted
//    MPostol - 18-03-2005
//      new was added to definition of station - the same ident was used in ProtocolALMessage
//    MPostol - 04-11-2003
//      Assert ertrors cause system reboot 
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.Modbus;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE
{
  /// <summary>
  /// ModBUS RTU message implementation
  /// </summary>
  internal partial class ModBusMessage: ModBusPDUMessage
  {
    #region PRIVATE
    private const byte const_stationAddressPos = 0;
    private const byte const_dataTypePos = 1;
    private const byte const_quantityPos = 2;
    private const byte const_dataAddressPos = 2;
    private const byte const_datapos = 3;
    private const byte const_cmdpos = 2;
    private const byte const_reqQuantityPos = 4;
    private const byte const_WriteValueOffset = 4;
    private const byte const_WriteMultipleValueOffset = 4 + 3; // for 32 bits: const_reqQuantityPos + 3
    private const byte const_bankAddressPos = 6; //CONTROL Micro XL extension
    private const byte const_RequestLength = 8;
    private const byte const_CRCLength = 2;
    private const byte const_MinimumFrameLength = 6;
    private const byte const_exceptionPos = 2;
    private const byte const_WriteSingleFrameLength = 8;
    private const byte const_coilWritePos = 4;
    private const byte const_DifferenceBetwwenFrameLengthAndReadObjectQuantity = 5;
    #region abstract place in frame definition
    protected override byte WriteMultipleValueOffset { get { return const_WriteMultipleValueOffset; } }
    protected override byte stationAddressPos { get { return const_stationAddressPos; } }
    protected override byte bankAddressPos { get { return const_bankAddressPos; } } //CONTROL Micro XL extension
    protected override byte dataAddressPos { get { return const_dataAddressPos; } }
    protected override byte dataTypePos { get { return const_dataTypePos; } }
    protected override byte quantityPos { get { return const_quantityPos; } }
    protected override byte dataPos { get { return const_datapos; } }
    protected override byte cmdPos { get { return const_cmdpos; } }
    protected override byte exceptionPos { get { return const_exceptionPos; } }
    protected override byte reqQuantityPos { get { return const_reqQuantityPos; } }
    protected override byte WriteSingleValueOffset { get { return const_WriteValueOffset; } }
    protected override byte RequestLength { get { return const_RequestLength; } }
    protected override byte CRCLengthInBytes { get { return const_CRCLength; } }
    protected override byte MinimumFrameLength { get { return const_MinimumFrameLength; } }
    protected override byte WriteSingleOrMultipleFrameLength { get { return const_WriteSingleFrameLength; } }
    protected override byte coilWritePos { get { return const_coilWritePos; } }
    protected override byte DifferenceBetwwenFrameLengthAndReadObjectQuantity
    { get { return const_DifferenceBetwwenFrameLengthAndReadObjectQuantity; } }
    protected override int DataLength
    {
      get { return this.userDataLength; }
    }
    protected override bool PerformCRCCheck()
    {
      return IsCRCGood();
    }
    #endregion abstract place in frame definition
    #endregion
    #region PUBLIC
    public override void WriteValue( object pValue, int pRegAddress )
    {
      base.WriteValue( pValue, pRegAddress );
      this.PutCRC16();
    }
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      base.PrepareRequest( station, block );
      offset = 6;
      if ( dataType == Modbus_Functions.READ_MEMORYBANK_CONTROL ) //CONTROL Micro XL extension
        offset++; //in READ_MEMORYBANK_CONTROL there is one additional field (Bank) before CRC
      PutCRC16();
    }
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      base.PrepareReqWriteValue( block, station );
      offset = 6;
      if ( dataType == Modbus_Functions.WRITE_MULTIPLE_REGISTERS )
        offset += 5; // write two registers (for 32bit) is longer 5 bytes
      if ( dataType == Modbus_Functions.WRITE_MEMORYBANK_CONTROL ) //CONTROL Micro XL extension
        offset += 2; //in WRITE_MEMORYBANK_CONTROL there are additional fields: (Bank) and (byte count) before CRC
      if ( dataType == Modbus_Functions.WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL ) //CONTROL Micro XL extension
        offset -= 1; //in WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL the frame is shorter because we are sending 1 data byte instead of 16 bit (two bytes) before CRC

      //MZTODO: tymczasowo zakladam, ze ilosc jest przesylana w polu 8bit powinno to byc wyjasnione z CONTROL'em
      PutCRC16();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ModBusMessage"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    /// <param name="myProtocolParameters">protocol parameters <see cref="ModBus_ProtocolParameters"/>.</param>
    internal ModBusMessage( IBufferLink homePool, ModBus_ProtocolParameters myProtocolParameters )
      : base( homePool, myProtocolParameters )
    {
    }
    #endregion
  }
}
