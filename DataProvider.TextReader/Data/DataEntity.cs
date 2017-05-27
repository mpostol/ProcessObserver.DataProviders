//_______________________________________________________________
//  Title   : DataEntity
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

using System;
using System.IO;

namespace CAS.CommServer.DataProvider.TextReader.Data
{
  /// <summary>
  /// Class DataEntity - data holder entity
  /// </summary>
  internal class DataEntity : IDataEntity
  {
    #region private 
    private DataEntity() { }
    #endregion

    #region public API
    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    /// <value>The time stamp.</value>
    public DateTime TimeStamp { get; private set; }
    /// <summary>
    /// Gets or sets the tags containing process data this instance captures.
    /// </summary>
    /// <value>The tags values in the form as they exist in the source file.</value>
    public string[] Tags { get; private set; }

    /// <summary>
    /// Reads the file and the analysis result provide as an instance of <see cref="IDataEntity"/>.
    /// </summary>
    /// <param name="fullPath">The full path.</param>
    /// <param name="timeStamp">The time stamp.</param>
    /// <param name="columnSeparator">The column separator.</param>
    /// <returns>IDataEntity.</returns>
    internal static IDataEntity ReadFile(string fullPath, DateTime timeStamp, string columnSeparator)
    {
      DataEntity _ret = null;
      string[] _content = File.ReadAllLines(fullPath);
      int _line2Read = Int32.Parse(_content[0].Trim());
      _ret = new DataEntity() { TimeStamp = timeStamp, Tags = _content[_line2Read].Split(new string[] { columnSeparator }, StringSplitOptions.None) };
      return _ret;
    }
    #endregion

  }

}
