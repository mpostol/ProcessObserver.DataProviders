//_______________________________________________________________
//  Title   : ReadDataEntity
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.Lib.CommonBus.ApplicationLayer;
using System;

namespace CAS.CommServer.DataProvider.TextReader.Data
{
  /// <summary>
  /// Class ReadDataEntity - instance of this class captures selected process data gathered from the buffer.
  /// </summary>
  /// <seealso cref="CAS.Lib.CommonBus.ApplicationLayer.IReadValue" />
  /// TODO Edit XML Comment Template for ReadDataEntity
  internal class ReadDataEntity : IReadValue
  {

    #region private
    private string[] Tags;
    #endregion

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadDataEntity"/> class and copies data from the <paramref name="dataBuffer"/> selected by the <paramref name="block"/>. 
    /// </summary>
    /// <param name="dataBuffer">The data buffer holding source data.</param>
    /// <param name="block">The data block description to retrieved from the <paramref name="dataBuffer"/>.</param>
    public ReadDataEntity(IDataEntity dataBuffer, IBlockDescription block)
    {
      startAddress = block.startAddress;
      dataType = block.dataType;
      length = block.length;
      Tags = new string[block.length];
      for (int i = 0; i < Tags.Length; i++)
        Tags[i] = dataBuffer.Tags[i + block.startAddress];
    }
    #endregion

    #region IReadValue
    /// <summary>
    /// Gets the data block starting address.
    /// </summary>
    /// <value>The start address.</value>
    public int startAddress
    {
      get; private set;
    }
    /// <summary>
    /// Gets the length of the data in bytes.
    /// </summary>
    /// <value>The length of data in buffer.</value>
    public int length
    {
      get; private set;
    }
    /// <summary>
    /// Determines the remote unit address space (resource) the data block belongs to. It could also be used to define
    /// data type if it is determined by address space.
    /// </summary>
    /// <value>The type of the data.</value>
    public short dataType
    {
      get; private set;
    }
    /// <summary>
    /// Checks if the buffer is in the pool or otherwise is alone and used by a user.
    /// Used to the state by the governing pool.
    /// </summary>
    /// <value><c>true</c> if the entity is in pool; otherwise, <c>false</c>.</value>
    public bool InPool
    {
      get { return false; }
      set { }
    }
    /// <summary>
    /// Check if address belongs to the block
    /// </summary>
    /// <param name="station">station ro be checked</param>
    /// <param name="address">address to be checked</param>
    /// <param name="type">data type</param>
    /// <returns>true if address belongs to the block</returns>
    public bool IsInBlock(uint station, ushort address, short type)
    {
      return (station == 0) && (type == dataType) && (address >= startAddress) && (address < (startAddress + length));

    }
    /// <summary>
    /// Reads the value and convert it to canonical type if possible.
    /// </summary>
    /// <param name="regAddress">The register address.</param>
    /// <param name="canonicalType">Canonical type of the tag.</param>
    /// <returns>System.Object.</returns>
    /// <exception cref="System.NotImplementedException">Is thrown if the value cannot be converted to the requested canonical value.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">Is thrown if the requested address index is out of range.</exception>
    public object ReadValue(int regAddress, Type canonicalType)
    {
      if (!IsInBlock(0, (ushort)(regAddress + startAddress), 0))
        throw new ArgumentOutOfRangeException($"The register address is out of the expected range");
      string _value = Tags[regAddress];
      if (canonicalType == typeof(string))
        return _value;
      if (canonicalType == typeof(float))
        return float.Parse(_value, System.Globalization.CultureInfo.InvariantCulture);
      if (canonicalType == typeof(long))
        return long.Parse(_value, System.Globalization.CultureInfo.InvariantCulture);
      if (canonicalType == typeof(int))
        return int.Parse(_value, System.Globalization.CultureInfo.InvariantCulture);
      if (canonicalType == typeof(short))
        return short.Parse(_value, System.Globalization.CultureInfo.InvariantCulture);
      throw new NotImplementedException($"The canical type {canonicalType.ToString()} is not supported");
    }
    /// <summary>
    /// Used by a user to return an empty envelope to the common pool. It also resets the message content.
    /// </summary>
    public void ReturnEmptyEnvelope() { }
    #endregion

  }
}
