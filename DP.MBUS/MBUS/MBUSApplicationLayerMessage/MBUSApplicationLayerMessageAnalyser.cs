//<summary>
//  Title   : MBUS application layer message analyser
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080812: mzbrzezny:  analyser outputs the MBUS address also in full analysis mode,
//                        begin of DIFE and VIFE analysis
//  20080529: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using System.Text;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE.MBUSApplicationLayerMessage
{
  /// <summary>
  /// MBUS application layer message analyser
  /// </summary>
  internal class MBUSApplicationLayerMessageAnalyser
  {
    private class DataList
    {
      private System.Collections.Generic.SortedList<int, object> m_datalist =
        new System.Collections.Generic.SortedList<int, object>();
      internal object this[ int index ]
      {
        get
        {
          if ( m_datalist.ContainsKey( index ) )
            return m_datalist[ index ];
          else
            return null;
        }
        set
        {
          if ( m_datalist.ContainsKey( index ) )
            m_datalist[ index ] = value;
          else
            m_datalist.Add( index, value );
        }
      }
      internal void Clear()
      {
        m_datalist.Clear();
      }
    }

    private DataList m_data = new DataList();
    private bool ready = false;
    StringBuilder sb; //used to prepare more complicated output as string
    MBUSApplicationLayerDIFE dife;//used to prepare information based on dife as string
    //StringBuilder sb_vife;//used to prepare information based on vife as string
    internal ProtocolALMessage.CheckResponseResult AnalyseFrame( MBUS_message MessageToAnalysed, int StartByte, DataAnalysisMode dataAnalysisMode, MBUS_message.TRACE TRACE )
    {
      ready = false;
      sb = null;
      dife = null;
      TRACE( TraceEventType.Verbose, 62, "AnalyseFrame.. start" );
      if ( MessageToAnalysed.CheckFrameType() != MBusFrameTypes.LongFrame )
        return ProtocolALMessage.CheckResponseResult.CR_Invalid;
      int m_data_index = 0; // ta zmienna indeksuje kojene m_data - zostawilismy jeszce wolne miesce na jedna dana
      int m_data_index_quantityofrecord = -1;
      m_data.Clear(); //MZ TODO: przeanalizowac czy ma to jakis wplyw na pamiec i Garbage Collector
      //zaczynamy:
      if ( dataAnalysisMode == DataAnalysisMode.Full )
      {
        // numer licznika:
        m_data[ m_data_index++ ] = MessageToAnalysed[ StartByte + 3 ].ToString( "X2" ) +
          MessageToAnalysed[ StartByte + 2 ].ToString( "X2" ) +
          MessageToAnalysed[ StartByte + 1 ].ToString( "X2" ) +
          MessageToAnalysed[ StartByte + 0 ].ToString( "X2" );
        TRACE( TraceEventType.Verbose, 71, String.Format( "counter no.: {0}", m_data[ 0 ] ) );
        // manufacture ID:
        m_data[ m_data_index++ ] = MessageToAnalysed[ StartByte + 5 ].ToString( "X2" ) +
          MessageToAnalysed[ StartByte + 4 ].ToString( "X2" );
        TRACE( TraceEventType.Verbose, 75, String.Format( "manufacture ID: {0}", m_data[ 1 ] ) );
        // transfer Data:
        sb = new StringBuilder();
        sb.Append( "MBUS address: " );
        sb.Append(MessageToAnalysed.LongFrameStation.ToString( "X2" ));
        sb.Append( "; Ver.:" );
        sb.Append( MessageToAnalysed[ StartByte + 6 ].ToString( "X2" ) );
        sb.Append( "; Medium: " );
        sb.Append( MBUSApplicationLayerMMVS.GetMMVSDescriptionByByte( MessageToAnalysed[ StartByte + 7 ] ).Description );
        sb.Append( "; Access No.:" );
        sb.Append( MessageToAnalysed[ StartByte + 8 ].ToString( "X2" ) );
        sb.Append( "; Status:" );
        sb.Append( MessageToAnalysed[ StartByte + 9 ].ToString( "X2" ) );
        sb.Append( "; Signature:" );
        sb.Append( MessageToAnalysed[ StartByte + 10 ].ToString( "X2" ) );
        sb.Append( MessageToAnalysed[ StartByte + 11 ].ToString( "X2" ) );
        m_data[ m_data_index++ ] = sb.ToString();
        m_data_index_quantityofrecord = m_data_index++;
        TRACE( TraceEventType.Verbose, 90, String.Format( "transfer Data: {0}", m_data[ 2 ] ) );
      }
      MessageToAnalysed.offset = (ushort)( StartByte + 12 ); // ustawiamy offset na poczatek bloku danych
      int NumberOfRecords = 0;// tutaj zliczamy liczbe odczytanych rekordow z przelicznika
      byte[] RecordValue = new byte[ 8 ];
      for ( int VariableIndex = 0; MessageToAnalysed.offset <= MessageToAnalysed.userDataLength - 2; VariableIndex++ )
      {
        if ( MessageToAnalysed.offset >= MessageToAnalysed.userDataLength - 1 )
          return ProtocolALMessage.CheckResponseResult.CR_Incomplete;        // w tej petli czytamy poszczegolne dane
        MBUSApplicationLayerDIF dif = MBUSApplicationLayerDIF.GetDIFDescriptionByByte( MessageToAnalysed.ReadByte() );
        if ( dif.DataField == MBUSApplicationLayerDIF.DataFields.SpecialFunction )
        {
          #region Special Functions Explanation Comment
          //Special Functions (data field = 1111b):
          //
          //DIF	Function
          //0Fh	Start of manufacturer specific data structures to end of user data
          //1Fh	Same meaning as DIF = 0Fh  +  More records follow in next telegram
          //2Fh	Idle Filler (not to be interpreted), following byte = DIF
          //3Fh..6Fh	Reserved
          //7Fh	Global readout request (all storage#, units, tariffs, function fields)
          //
          //If data follows after DIF=$0F or $1F these are manufacturer specific data records. 
          //The number of bytes in these manufacturer specific data can be calculated with the L-Field. 
          //  The DIF 1Fh signals a request from the slave to the master to readout the slave once again. 
          //The master must readout the slave until there is no DIF=1Fh inside the respond telegram (multi telegram readout).
          //
          // przestajemy analizowac ramke
          #endregion Special Functions Explanation Comment
          break;
        }
        if ( dif.Extension )
          dife = new MBUSApplicationLayerDIFE();
        while ( dife != null && dife.Extension ) //TODO: zastanowic sie co robic z rozszerzeniami
        {
          dife.AddNextDIFE( MessageToAnalysed.ReadByte() );
        }
        MBUSApplicationLayerVIFBase vif = MBUSApplicationLayerVIF.GetVIFDescriptionByByte( MessageToAnalysed.ReadByte() );
        if ( vif.Code == (byte)MBUSApplicationLayerVIF.SpecialVIF.Extension_FB )
        {
          vif = MBUSApplicationLayerVIF_Extended_TypeB_FB.GetVIFDescriptionByByte( MessageToAnalysed.ReadByte() );
        }
        else if ( vif.Code == (byte)MBUSApplicationLayerVIF.SpecialVIF.Extension_FD )
        {
          vif = MBUSApplicationLayerVIF_Extended_TypeA_FD.GetVIFDescriptionByByte( MessageToAnalysed.ReadByte() );
        }

        MBUSApplicationLayerExtendableDataInformation vif2 = vif;
        while ( vif2.Extension ) //TODO: zastanowic sie co robic z rozszerzeniami
        {
          vif2 = MBUSApplicationLayerVIF.GetVIFDescriptionByByte( MessageToAnalysed.ReadByte() );
          throw new NotImplementedException( "waiting for VIFE implementation" );
        }
        //teraz odczytujemy konkretna dana:
        sb = new StringBuilder();
        for ( int dataindex = 0; dataindex < dif.DataFieldLengthInBytes; dataindex++ )
          RecordValue[ dataindex ] = MessageToAnalysed.ReadByte();
        m_data[ m_data_index++ ] = vif.ConvertToValue( RecordValue, dif );
        sb.Append( String.Format( "value: {0} ", m_data[ m_data_index - 1 ] ) );
        if ( dataAnalysisMode == DataAnalysisMode.Full )
        {
          m_data[ m_data_index++ ] = dif.ToString();
          if ( dife != null )
            m_data[ m_data_index - 1 ] += dife.ToString();
          sb.Append( String.Format( ";DIF: {0} ", m_data[ m_data_index - 1 ] ) );
          m_data[ m_data_index++ ] = vif.Description;
          sb.Append( String.Format( ";VIF Description: {0} ", m_data[ m_data_index - 1 ] ) );
          sb.Append( String.Format( ";VIF ToString: {0} ", vif.ToString() ) );
          m_data[ m_data_index++ ] = vif.EngUnit;
          sb.Append( String.Format( ";VIF EngUnit: {0} ", m_data[ m_data_index - 1 ] ) );
        }
        sb.AppendLine( "" );
        NumberOfRecords++;
        TRACE( TraceEventType.Verbose, 143, sb.ToString() );
      }
      if ( m_data_index_quantityofrecord >= 0 )
        m_data[ m_data_index_quantityofrecord ] = NumberOfRecords; // tutaj zapisujemy ilosc odczytanych recordow
      TRACE( TraceEventType.Verbose, 146, "AnalyseFrame... end" );
      ready = true;
      return ProtocolALMessage.CheckResponseResult.CR_OK;
    }
    internal object this[ int index ]
    {
      get
      {
        if ( !ready )
          throw new Exception( "Message is not yet ready" );
        return m_data[ index ];
      }
    }
  }
}
