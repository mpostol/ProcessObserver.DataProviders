//<summary>
//  Title   : DDE implementation of   public class Message: ProtocolALMessage
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

namespace CAS.Lib.CommonBus.ApplicationLayer.DDE
{
  /// <summary>
  /// DDE implementation of   public class Message: ProtocolALMessage
  /// </summary>
  internal class Message: ProtocolALMessage
  {
    #region PRIVATE
    private string excelDDEItemBlock = Resources.Excel_DDE_Item_Block_English;
    string[] responses;
    private object ConvertFromCanonicalType(object source, Type type)
    {
      if (type == null || source == null)
        return source;
      object ret = null;
      if (type == typeof(System.Object))
      {
        return source;
      }
      else
        if (type == typeof(System.String))
        {
          return source.ToString();
        }
        else
          if (type == typeof(System.UInt16))
          {
            System.UInt16 outvalue;
            if (System.UInt16.TryParse(source.ToString(), out outvalue))
            {
              return outvalue;
            }
          }
          else
            if (type == typeof(System.Int16))
            {
              System.Int16 outvalue;
              if (System.Int16.TryParse(source.ToString(), out outvalue))
              {
                return outvalue;
              }
            }
            else
              if (type == typeof(System.UInt32))
              {
                System.UInt32 outvalue;
                if (System.UInt32.TryParse(source.ToString(), out outvalue))
                {
                  return outvalue;
                }
              }
              else
                if (type == typeof(System.Int32))
                {
                  System.Int32 outvalue;
                  if (System.Int32.TryParse(source.ToString(), out outvalue))
                  {
                    ret = outvalue;
                  }
                }
                else
                  if (type == typeof(System.Boolean))
                  {
                    System.Boolean outvalue;
                    if (System.Boolean.TryParse(source.ToString(), out outvalue))
                    {
                      return outvalue;
                    }
                  }
                  else
                    if (type == typeof(System.DateTime))
                    {
                      System.DateTime outvalue;
                      if (System.DateTime.TryParse(source.ToString(), out outvalue))
                      {
                        return outvalue;
                      }
                    }
      if (ret == null)
      {
        System.Double outvalue;
        if (System.Double.TryParse(source.ToString(), out outvalue))
        {
          ret = outvalue;
        }
        else
          ret = outvalue = System.Double.NaN;
        if (type == typeof(System.Double))
        {
          return ret;
        }
        if (outvalue != Double.NaN)
        {
          try
          {
            ret = System.Convert.ChangeType(outvalue, type);
          }
          catch (Exception)
          {
            ret = null;
          }
        }
      }
      return ret;
    }
    private object ConvertToCanonicalType( object source )
    {
      return source.ToString();
    }
    #endregion
    #region ProtocolALMessage
    public override void SetBlockDescription( int station, IBlockDescription block )
    {
      SetBlockDescription( station, block.startAddress, block.dataType, block.length );
    }
    public override void SetBlockDescription( int station, int address, short myDataType, int length )
    {
      base.SetBlockDescription( station, address, myDataType, length );
    }
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      string request = String.Format( "SND;" + excelDDEItemBlock, block.startAddress, block.dataType,
        block.startAddress + block.length - 1, block.dataType );
      userDataLength = (ushort)request.Length;
      this.WriteString( request );
      this.SetBlockDescription( station, block );
    }
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      string request = String.Format( "REQ;" + excelDDEItemBlock, block.startAddress, block.dataType,
        block.startAddress + block.length - 1, block.dataType );
      userDataLength = (ushort)request.Length;
      this.WriteString( request );
      this.SetBlockDescription( station, block );
    }

    public override ProtocolALMessage.CheckResponseResult CheckResponseFrame( ProtocolALMessage txmsg )
    {
      string wholeresponse = System.Text.ASCIIEncoding.ASCII.GetString( GetManagedBuffer() ).Trim(
        new char[] { '\n', '\r', '\0', ' ', '\t' } );
      responses = wholeresponse.Split( new char[] { '\n' } );
      for ( int idx = 0; idx < responses.Length; idx++ )
        responses[ idx ] = responses[ idx ].Trim();
      if ( ( (IBlockDescription)txmsg ).length > responses.Length )
        return CheckResponseResult.CR_Incomplete;
      if ( ( (IBlockDescription)txmsg ).length < responses.Length )
        return CheckResponseResult.CR_Invalid;
      return CheckResponseResult.CR_OK;
    }
    public override object ReadValue( int regAddress, Type pCanonicalType )
    {
      return ConvertFromCanonicalType(
        responses[ regAddress ]
        , pCanonicalType );
    }
    protected override object ReadCMD( int regAddress )
    {
      throw new NotImplementedException( "The method or operation is not implemented." );
      //return buffor[ regAddress ];
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
      if ( pRegAddress > 0 )
        throw new NotImplementedException( "writing to not 0 register in DDE block is not implemented." );
      this.offset = (ushort)( this.userDataLength );
      string pValAsString = pValue.ToString();
      this.userDataLength += (ushort)pValAsString.Length;
      this.WriteString( pValAsString );
    }
    protected override void SetValue( object regValue, int regAddress )
    {
      throw new NotImplementedException( "The method or operation is not implemented." );
      //buffor[ regAddress ] = regValue;
    }
    internal void SetData( string DataToBeSet, int offset )
    {
      throw new NotImplementedException();
    }
    #endregion
    #region PUBLIC
    /// <summary>
    /// Initializes a new instance of the <see cref="Message"/> class.
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    internal Message( CommunicationLayer.Generic.IBufferLink homePool )
      : base( 2048, homePool, false )
    {
      switch ( DS_DataProviderID.GetExcelLanguage() )
      {
        case DS_DataProviderID.ExcelLanguageEnum.English:
          excelDDEItemBlock = Resources.Excel_DDE_Item_Block_English;
          break;
        case DS_DataProviderID.ExcelLanguageEnum.Polish:
          excelDDEItemBlock = Resources.Excel_DDE_Item_Block_Polish;
          break;
        default:
          TraceEvent( System.Diagnostics.TraceEventType.Warning, 114, "Not supported localisation, English localisation is used instead" );
          break;
      }
    }
    #endregion
  }
}