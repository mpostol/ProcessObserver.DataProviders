//<summary>
//  Title   : Modbus Exceptions and Enums
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20080826: mzbrzezny: not suppoorted data types are commented
//  20080825: mzbrzezny: enums for CONTROL Micro XL added
//  2002: mpostol: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE
{
  internal enum Medium_T: short
  {
    Coil = 0,
    Discrete_input = 1,
    Holding_register = 10,
    Holding_32bit_register=100,
    Input_register,
    //Exception_status,
    //Diagnostic,
    //Com_event_counter,
    //Com_event_log,
    //Fifo_queue,
    //Device_identification,
    //File_record,
    //DataBlock_CONTROL, //CONTROL Micro XL extension
    Register_MemoryBank_CONTROL = 200, //CONTROL Micro XL extension
    Holding_8bit_register_CONTROL = 201 //CONTROL Micro XL extension
  }
  internal enum Modbus_Exceptions: byte
  {
    ILLEGAL_FUNCTION = 0x01,
    ILLEGAL_DATA_ADDRESS = 0x02,
    ILLEGAL_DATA_VALUE = 0x03,
    SLAVE_DEVICE_FAILURE = 0x04,
    ACKNOWLEDGE = 0x05,
    SLAVE_DEVICE_BUSY = 0x06,
    MEMORY_PARITY_ERROR = 0x08,
    GATEWAY_PATH_UNAVAILABLE = 0x0A,
    GATEWAY_TARGET_DEVICE_FAILED_TO_RESPOND = 0x0B,
    ILLEGAL_CRC = 0x0C,
    OK = 0x0F,
  };
  internal enum Modbus_Functions: byte
  {
    ERORR = 0x00,
    READ_COILS = 0x01,
    READ_DISCRETE_INPUT = 0x02,
    READ_HOLDING_REGISTERS = 0x03,
    READ_INPUT_REGISTER = 0x04,
    WRITE_SINGLE_COIL = 0x05,
    WRITE_SINGLE_REGISTER = 0x06,
    READ_EXCEPTION_STATUS = 0x07,
    DIAGNOSTIC = 0x08,
    GET_COM_EVENT_COUNTER = 0x0B,
    GET_COM_EVENT_LOG = 0x0C,
    WRITE_MULTIPLE_COILS = 0x0F,
    WRITE_MULTIPLE_REGISTERS = 0x10,
    REPORT_SLAVEID = 0x11,
    READ_FILE_RECORD = 0x14,
    WRITE_FILE_RECORD_AND_WRITE_DATABLOCK_CONTROL = 0x15,
    MASK_WRITE_REGISTER = 0x16,
    READ_WRITE_MULTIPLE_REGISTERS = 0x17,
    READ_FIFO_QUEUE = 0x18,
    READ_HOLDING_REGISTERS_8BIT_CONTROL = 0x1C, //CONTROL Micro XL extension
    WRITE_SINGLE_HOLDING_REGISTERS_8BIT_CONTROL = 0x1D, //CONTROL Micro XL extension
    WRITE_MULTIBPLE_HOLDING_REGISTERS_8BIT_CONTROL = 0x1E, //CONTROL Micro XL extension
    READ_DATABLOCK_CONTROL = 0x1F, //CONTROL Micro XL extension
    READ_MEMORYBANK_CONTROL = 0x20, //CONTROL Micro XL extension
    WRITE_MEMORYBANK_CONTROL = 0x21, //CONTROL Micro XL extension
    READ_DEVICE_IDENTIFICATION = 0x2B
  };
  internal enum Diagnostics_Sub_Functions: byte
  {
    RETURN_QUERY_DATA = 0x00,
    RESTART_COMMUNICATIONS_OPTION = 0x01,
    RETURN_DIAGNOSTIC_REGISTER = 0x02,
    CHANGE_ASCII_INPUT_DELIMITER = 0x03,
    FORCE_LISTEN_ONLY_MODE = 0x04,
    /*05..09 NOT USED*/
    CLEAR_COUNTERS_AND_DIAGNOSTIC_REGISTER = 0x0A,
    RETURN_BUS_MESSAGE_COUNT = 0x0B,
    RETURN_BUS_COMMUNICATION_ERROR_COUNT = 0x0C,
    RETURN_BUS_EXCEPTION_ERROR_COUNT = 0x0D,
    RETURN_SLAVE_MESSAGE_COUNT = 0x0E,
    RETURN_SLAVE_NO_RESPONSE_COUNT = 0x0F,
    RETURN_SLAVE_NAK_COUNT = 0x10,
    RETURN_SLAVE_BUSY_COUNT = 0x11,
    RETURN_BUS_CHARACTER_OVERRUN_COUNT = 0x12,
    PRIVATE1 = 0x13,
    PRIVATE2 = 0x14,
  };
  internal enum RX_BP_RetCode
  {
    RX_BP_OK,
    RX_BP_NAK,
    RX_BP_INVALID,
    RX_BP_BCCERR,
    RX_BP_Uncompl,
    RX_BP_NotAwaiting
  };
}//namespace BaseStation.ModBus.PRIVATE
