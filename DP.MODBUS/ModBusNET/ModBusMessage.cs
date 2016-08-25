//<summary>
//  Title   : Modbus RTU message implementation
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080904: mzbrzezny: adaptation for new umessage that supports returning of information about status of write operation
//    20080826: mzbrzezny: cleanup
//    20080620: mzbrzezny: DataLength  is added and mus be overridden by child message to return value 
//                         based on userDataLength (RS) or based on Length field from MBAP header.
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

using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.CommonBus.ApplicationLayer.Modbus;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE
{
  /// <summary>
  /// ModBUS RTU message implementation
  /// </summary>
  internal partial class ModBusMessage: ModBusPDUMessage
  {
    #region PRIVATE
    //Note: values below are based on MODBUS RTU, so:
    //- NET Frame has  longer header than RTU frame: 6 bytes (7 bytes of MBAP header - 1 byte station address), thats why we are adding 6
    //- NET Frame has 0 length CRC 
    //- according to notes above if we want to calculate full frame length in compare to RTU frame we have to add 6 and substract 2
    //NET specific
    private const byte const_TransactionIdentifierOffset = 0;
    private const byte const_ProtocolIdentifierOffset = 2;
    private const byte const_LengthOffset = 4;
    //Common:
    private const byte const_stationAddressPos = 0 + 6;
    private const byte const_dataTypePos = 1 + 6;
    private const byte const_quantityPos = 2 + 6;
    private const byte const_dataAddressPos = 2 + 6;
    private const byte const_datapos = 3 + 6;
    private const byte const_cmdpos = 2 + 6;
    private const byte const_reqQuantityPos = 4 + 6;
    private const byte const_WriteValueOffset = 4 + 6;
    private const byte const_WriteMultipleValueOffset = 4 + 3 + 6; // for 32 bits: const_reqQuantityPos + 3 + 6 as usually
    private const byte const_bankAddressPos = 6 + 6; //CONTROL Micro XL extension
    private const byte const_RequestLength = 8 + 6 - 2;
    private const byte const_CRCLength = 2 - 2;
    private const byte const_MinimumFrameLength = 6 + 6 - 2 - 1;// tylko dlaczego odejmuje jeszcze 1?
    private const byte const_exceptionPos = 2 + 6;
    private const byte const_WriteSingleFrameLength = 8 + 6 - 2;
    private const byte const_coilWritePos = 4 + 6;
    private const byte const_DifferenceBetwwenFrameLengthAndReadObjectQuantity = 5 + 6 - 2;
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
      get
      {
        ushort tempoffset = this.offset;
        this.offset = const_LengthOffset;
        int ret = this.ReadInt16() + 6;
        this.offset = tempoffset;
        return ret;
      }
    }
    #endregion abstract place in frame definition
    private static short TransactionIdentifierCouter = 0;
    internal short TransactionIdentifier
    {
      get
      {
        ushort tempoffset = this.offset;
        this.offset = const_TransactionIdentifierOffset;
        short ret = this.ReadInt16();
        this.offset = tempoffset;
        return ret;
      }
      private set
      {
        ushort tempoffset = this.offset;
        this.offset = const_TransactionIdentifierOffset;
        myAssert.Assert( this.WriteInt16( value ), 106, "ModBUS: Cannot write TransactionIdentifier" );
        this.offset = tempoffset;
      }
    }
    internal short ProtocolIdentifierOffset
    {
      get
      {
        ushort tempoffset = this.offset;
        this.offset = const_ProtocolIdentifierOffset;
        short ret = this.ReadInt16();
        this.offset = tempoffset;
        return ret;
      }
      private set
      {
        ushort tempoffset = this.offset;
        this.offset = const_ProtocolIdentifierOffset;
        myAssert.Assert( this.WriteInt16( value ), 124, "ModBUS: Cannot write ProtocolIdentifierOffset" );
        this.offset = tempoffset;
      }
    }
    #endregion
    #region PUBLIC
    public override void WriteValue( object pValue, int pRegAddress )
    {
      base.WriteValue( pValue, pRegAddress );
    }
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      base.PrepareRequest( station, block );
      //MBAP header:
      TransactionIdentifier = TransactionIdentifierCouter++;
      ProtocolIdentifierOffset = 0;
      this.offset = const_LengthOffset;
      myAssert.Assert( this.WriteInt16( (short)( RequestLength - 6 ) ),
        142, "ModBUS: PrepareRequest: Cannot write RequestLength - 6" );// nie wliczamy do tego poczatku naglowka
    }
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      base.PrepareReqWriteValue( block, station );
      //MBAP header:
      TransactionIdentifier = TransactionIdentifierCouter++;
      ProtocolIdentifierOffset = 0;
      this.offset = const_LengthOffset;
      myAssert.Assert( this.WriteInt16( (short)( RequestLength - 6 ) ),
        151, "ModBUS: PrepareReqWriteValue: Cannot write RequestLength - 6" );// nie wliczamy do tego poczatku naglowka
    }
    public override ProtocolALMessage.CheckResponseResult CheckResponseFrame( ProtocolALMessage tMes )
    {
      ModBusMessage TXModBusMessage = (ModBusMessage)tMes;
      if ( this.TransactionIdentifier != TXModBusMessage.TransactionIdentifier )
        return CheckResponseResult.CR_SynchError;
      if ( this.ProtocolIdentifierOffset != 0 )
        return CheckResponseResult.CR_Invalid;
      return base.CheckResponseFrame( tMes );
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ModBusMessage"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    /// <param name="myProtocolParameters">My protocol parameters.</param>
    internal ModBusMessage( IBufferLink homePool, ModBus_ProtocolParameters myProtocolParameters )
      : base(  homePool , myProtocolParameters)
    {
    }
    #endregion
  }
}
