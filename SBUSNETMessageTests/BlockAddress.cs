using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAS.Lib.CommonBus.ApplicationLayer;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS.NET
{

  /// <summary>
  /// Block Address default implementation.
  /// </summary>
  internal class BlockAddress: IBlockAddress
  {

    #region IBlockAddress Members
    public int station
    {
      get { return m_Station; }
    }
    #endregion

    #region IBlockDescription Members
    public short dataType
    {
      get { return m_DataType; }
      set { m_DataType = value; }
    }
    public int length
    {
      get { return m_RegistersCount; }
    }
    public int startAddress
    {
      get { return m_StartAddress; }
    }
    #endregion

    #region ctor
    public BlockAddress( short dataType, int station, ushort startAddress, byte registersCount )
      : this( dataType )
    {
      m_Station = station;
      m_StartAddress = startAddress;
      m_RegistersCount = registersCount;
    }
    public BlockAddress( short dataType )
    {
      m_DataType = dataType;
    }
    #endregion

    #region private
    private int m_Station = 0x4B;
    private ushort m_StartAddress = 360;
    private byte m_RegistersCount = 13;
    private short m_DataType;
    #endregion

  }

}
