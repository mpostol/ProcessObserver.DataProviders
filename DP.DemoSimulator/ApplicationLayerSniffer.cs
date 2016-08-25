//<summary>
//  Title   : DemoSimulator implementation - sniffer side
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{
  /// <summary>
  /// DemoSimulator implementation - sniffer side
  /// </summary>
  internal class ApplicationLayerSniffer: ApplicationLayerCommon, IApplicationLayerSniffer
  {
    #region PRIVATE
    /// <summary>
    ///  Title   : NULL implementation of NULL_buf_pool: CommunicationLayer.SesDBufferPool
    /// </summary>
    private class NULL_buf_pool: CommunicationLayer.Generic.SesDBufferPool<Message>
    {
      protected override Message CreateISesDBuffer()
      {
        return new Message( this );
      }
    }//NULL_buf_pool
    private NULL_buf_pool pool = new NULL_buf_pool();
    CAS.Lib.RTLib.Management.IProtocolParent myStatistic;
    private void init()
    {
      System.Random rand = new Random( (int)( System.DateTime.Now.Ticks % 10000 ) );
      IReadValue data;
      object _value = null;
      //stacja 1
      data = (IReadValue)pool.GetEmptyISesDBuffer();
      ( (Message)data ).SetBlockDescription( 1, 0, (short)Medium_T.Flag, 4 );
      for ( int i = 0; i < 4; i++ )
      {
        _value = false;
        if ( rand.Next( 10 ) > 4 )
          _value = true;
        ((Message)data).setDBTag(1, 0, (short)Medium_T.Flag, _value);
      }
      ( (Message)data ).ReadFromDB();
      data.ReturnEmptyEnvelope();
      data = (IReadValue)pool.GetEmptyISesDBuffer();
      ((Message)data).SetBlockDescription(1, 0, (short)Medium_T.Register, 4);
      ( (Message)data ).ReadFromDB();
      data.ReturnEmptyEnvelope();
      for ( int i = 0; i < 4; i++ )
      {
        _value = (int)rand.Next( 1000 );
        ((Message)data).setDBTag(1, 0, (short)Medium_T.Register, _value);
      }
      //stacja 2
      data = (IReadValue)pool.GetEmptyISesDBuffer();
      for ( int i = 0; i < 4; i++ )
      {
        _value = false;
        if ( rand.Next( 10 ) > 4 )
          _value = true;
        ((Message)data).setDBTag(1, 0, (short)Medium_T.Flag, _value);
      }
      ((Message)data).SetBlockDescription(2, 0, (short)Medium_T.Flag, 4);
      ( (Message)data ).ReadFromDB();
      data.ReturnEmptyEnvelope();
      data = (IReadValue)pool.GetEmptyISesDBuffer();
      for ( int i = 0; i < 25; i++ )
      {
        _value = (int)rand.Next( 1000 );
        ((Message)data).setDBTag(1, 0, (short)Medium_T.Register, _value);
      }
      ((Message)data).SetBlockDescription(2, 0, (short)Medium_T.Register, 4);
      ( (Message)data ).ReadFromDB();
      data.ReturnEmptyEnvelope();
    }
    #endregion
    #region IApplicationLayerSniffer
    AL_ReadData_Result IApplicationLayerSniffer.ReadData( out IBlockDescription block, out IReadValue data )
    {
      lock ( this )
      {
        System.Threading.Thread.Sleep( TimeSpan.FromSeconds( 1 ) );
        data = null;
        System.Random rand = new Random( (int)( System.DateTime.Now.Ticks % 10000 ) );
        int station = 1;
        if ( rand.Next( 10 ) > 5 )
          station = 2; //tylko 2 stacje - 1 i 2
        data = (IReadValue)pool.GetEmptyISesDBuffer();
        Medium_T dt = Medium_T.Flag;
        if ( rand.Next( 10 ) > 5 )
          dt = Medium_T.Register;  // 50% szans ze to rejestry
        int address = 0;  // adres startowy 0-5
        int len = 5;      //maksymalna dlugosc bloku 5
        object _value = false;
        for ( int i = address; i < address + len; i++ )
        {
          if ( station == 1 )
          {
            switch ( dt )
            {
              case Medium_T.Flag:
              case Medium_T.Input:
              case Medium_T.Output:
                {
                  _value = false;
                  if ( rand.Next( 10 ) > 4 )
                    _value = true;
                  break;
                }
              case Medium_T.Counter:
              case Medium_T.Register:
              case Medium_T.Timer:
                {
                  _value = (int)rand.Next( 1000 );
                  break;
                }
            }
          }
          else
          {
            switch ( dt )
            {
              case Medium_T.Flag:
              case Medium_T.Input:
              case Medium_T.Output:
                {
                  _value = false;
                  break;
                }
              case Medium_T.Counter:
              case Medium_T.Register:
              case Medium_T.Timer:
                {
                  _value = 0;
                  break;
                }
            }
          }

          ((Message)data).setDBTag(station, address, (short)dt, _value);
        }
        ((Message)data).SetBlockDescription(station, address, (short)dt, len);
        ( (Message)data ).ReadFromDB();
        block = ( (Message)data );
        myStatistic.IncStTxFrameCounter();
        myStatistic.IncStRxFrameCounter();
        myStatistic.RxDataBlock( true );
        return AL_ReadData_Result.ALRes_Success;
      }
    }//ReadData
    #endregion
    #region INIT
    internal ApplicationLayerSniffer
      ( CAS.Lib.RTLib.Management.IProtocolParent pStatistic, ICommunicationLayer pComm )
      : base( pComm )
    {
      this.myStatistic = pStatistic;
      init();
    }
    #endregion
  }
}
