//<summary>
//  Title   : MBUS_ApplicationLayerSlave
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{

  /// <summary>
  /// Modbus Application Layer slave implementation
  /// </summary>
  internal class ModBus_ApplicationLayerSlave: ApplicationLayerCommon, IApplicationLayerSlave
  {
    //private IProtocolParent myStatistic = null;
    //    private class MODBUS_buf_pool: CAPI.Session.SesDBufferPool
    //    {
    //      protected override CAPI.Session.ISesDBuffer CreateISesDBuffer()
    //      {
    //        MBUS_message newMess = new MBUS_message(this);
    //        return newMess; 
    //      }
    //    }//MODBUS_buf_pool
    //    private MODBUS_buf_pool pool;
    //    //private RS_to_Serial r_t_s = new RS_to_Serial.RS_to_Serial();
    //
    //    private bool TX_TELEGRAM(MBUS_message Txmsg, object PcdPort)
    //    {
    //      bool res;
    //      if(r_t_s.FrameEndSignal(PcdPort,Txmsg)==BaseStation.TResult.Success) res = true;
    //      else res = false;
    //      return res;																					 
    //    }
    //    private RX_Res_T RX_TELEGRAM(object PcdPort, out MBUS_message Rxmsg, bool GetResponse)
    //    {
    //      BaseStation.TResult getcharres = 0, getcharres2 = 0;
    //      BaseStation.RX_Res_T res;
    //      byte lastChar;
    //      Rxmsg = (MBUS_message)pool.GetEmptyISesDBuffer();
    //      getcharres = r_t_s.GetChar(out lastChar,PcdPort,GetResponse);
    //      if(getcharres == BaseStation.TResult.Success)
    //      {
    //        while(getcharres2!=BaseStation.TResult.Timeout35)
    //        {
    //          Rxmsg.WriteByte(lastChar);
    //          getcharres2 = r_t_s.GetChar(out lastChar,PcdPort);
    //          if(getcharres2 == BaseStation.TResult.Timeout15) 
    //          {
    //            Rxmsg = null;
    //            return BaseStation.RX_Res_T.RX_INVALID;
    //          }
    //        }//end while
    //        if(Rxmsg.CheckRequestFrame()== Modbus_Exceptions.OK) res = BaseStation.RX_Res_T.RX_OK;
    //        else res = BaseStation.RX_Res_T.RX_INVALID;
    //      }//end if
    //      res = BaseStation.RX_Res_T.RX_INVALID;
    //      return res;
    //
    //    }//RX_TELEGRAM
    //
    internal ModBus_ApplicationLayerSlave( ICommunicationLayer cCommChannel )
      : base( cCommChannel )
    {
      //      pool = new MODBUS_buf_pool();
    }
    #region IApplicationLayerSlave Members
    /// <summary>
    /// Read command from a master station.
    /// </summary>
    /// <param name="frame">Received frame</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result IApplicationLayerSlave.ReadCMD( out IReadCMDValue frame )
    {
      frame = null;
      return AL_ReadData_Result.ALRes_DatTransferErrr; //not implemented
      //      bool ret;
      //      MBUS_message modbus_frame;
      //      frame=null;
      //      RX_Res_T res = RX_TELEGRAM(port, out modbus_frame, false);
      //      if(res==RX_Res_T.RX_OK)
      //      {
      //        switch(modbus_frame.dataType)
      //        {
      //          case 0:
      //            cmd = (BaseStation.ProtocolCmd)BaseStation.Modbus_Functions.ERORR;
      //            break;
      //          case 1:
      //            cmd = (BaseStation.ProtocolCmd)BaseStation.Modbus_Functions.READ_COILS;
      //            break;
      //          case 3:
      //            cmd = (BaseStation.ProtocolCmd)BaseStation.Modbus_Functions.READ_MULTIPLE_REGISTERS;
      //            break;
      //          case 5:
      //            cmd = (BaseStation.ProtocolCmd)BaseStation.Modbus_Functions.WRITE_SINGLE_COIL;
      //            break;
      //          case 6:
      //            cmd = (BaseStation.ProtocolCmd)BaseStation.Modbus_Functions.WRITE_SINGLE_REGISTER;
      //            break;
      //          default:
      //            cmd = ProtocolCmd.coDoNothing; 
      //            ret = false;
      //            break;
      //        }
      //        frame = modbus_frame;
      //        command = (IBlockDescription)frame;
      //        ret = true;
      //      }
      //      else
      //      {
      //        cmd = ProtocolCmd.coDoNothing;
      //        command = null;
      //        return false;
      //      }
      //      return ret;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result IApplicationLayerSlave.SendNAKRes()
    {
      //      MBUS_message frame = (MBUS_message)pool.GetEmptyISesDBuffer();
      //      frame.station = oldframe.station;
      //      frame.dataType = (byte)(oldframe.dataType + 0x80);
      //      frame.fioCount = (byte)exc; // fioCount bo wstawia w dobre miejsce ramki
      //			
      //      bool res = TX_TELEGRAM(frame, port);
      //      frame.ReturnEmptyEnvelope();
      //      return res;
      return AL_ReadData_Result.ALRes_DatTransferErrr; //not implemented
    }
    //potrzebujê prze³adowanej wersji patrz wy¿ej
    //    public bool SendNAKRes(object port)
    //    {
    //      return false;
    //    }
    //dla modbusa do tworzenia ramki odpowiedzi na ¿¹danie pisania
    internal bool SendACKRes( ModBusMessage oldframe )
    {
      //      bool res = TX_TELEGRAM(oldframe, port);
      //      oldframe.ReturnEmptyEnvelope();
      //      return res;
      return false;
    }
    //potrzebujê prze³adowanej wersji patrz wy¿ej
    AL_ReadData_Result IApplicationLayerSlave.SendACKRes()
    {
      return AL_ReadData_Result.ALRes_DatTransferErrr; //not implemented
    }

    IResponseValue IApplicationLayerSlave.GetEmptySendDataBuffor( IBlockDescription block, int address )
    {
      return null;
      //      MBUS_message frame = (MBUS_message)pool.GetEmptyISesDBuffer();
      //      frame.dataType = (byte)block.dataType;
      //      frame.address = System.Convert.ToInt16(block.startAddress);			
      //      if (((int)block.dataType >= 10) && ((int)block.dataType <= 15)) 
      //        frame.regCount = System.Convert.ToInt16(((block.length * 4) + 4));
      //      else frame.regCount = System.Convert.ToInt16( block.length);
      //      return frame;
    }
    AL_ReadData_Result IApplicationLayerSlave.SendData( IResponseValue data )
    {
      return AL_ReadData_Result.ALRes_DatTransferErrr; //not implemented
      //      return TX_TELEGRAM((MBUS_message) data, port);
    }
    #endregion
  }
  internal enum recStateType
  {
    WaitFS_FE,
    WaitFS_TO,
    SkipFrame,
    RecFrame
  };
}
