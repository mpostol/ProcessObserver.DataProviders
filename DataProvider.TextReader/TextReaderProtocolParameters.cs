﻿//_______________________________________________________________
//  Title   : TextReaderProtocolParameters
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

using CAS.CommServer.DataProvider.TextReader.Properties;
using System;
using System.ComponentModel;
using System.Xml;

namespace CAS.CommServer.DataProvider.TextReader
{
  /// <summary>
  /// Class TextReaderProtocolParameters - provides editable parameters of the DataProvider behavior
  /// </summary>
  public class TextReaderProtocolParameters : ITextReaderProtocolParameters
  {

    #region ITextReaderProtocolParameters
    /// <summary>
    /// Gets or sets the file modification notification timeout.
    /// </summary>
    /// <value><see cref="double" /> representing the file modification notification timeout.</value>
    [Description("Timeout of File Modification Notification in milliseconds")]
    [DisplayName("Timeout [mS]")]
    [Browsable(true)]
    public double FileModificationNotificationTimeout { get; set; } = Settings.Default.DefaultFileModificationNotificationTimeout;
    /// <summary>
    /// Gets the delay file scan - it is time to postpone the file content read operation after receiving file modification notification.
    /// It is time needed by the remote application to finalize writing to file and release the file for other processes.
    /// </summary>
    /// <value>The delay file scan.</value>
    [Description("Time to postpone the file content read operation after receiving file modification notification. It is time needed by the remote application to finalize writing to file and release the file for other processes.")]
    [DisplayName("Delay File Scan")]
    [Browsable(true)]
    public double DelayFileScan { get; set; } = 1000;
    /// <summary>
    /// Gets the column separator - string used to separate columns in the scanned text.
    /// </summary>
    /// <value>The column separator.</value>
    [Description("String used to separate columns in the text")]
    [DisplayName("Column Separator")]
    [Browsable(true)]
    public string ColumnSeparator { get; set; } = ",";
    #endregion

    #region Objject
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return $"ColumnSeparator: \"{ColumnSeparator}\", DelayFileScann: {TimeSpan.FromMilliseconds(DelayFileScan)}, Timeou: {TimeSpan.FromMilliseconds(FileModificationNotificationTimeout)}";
    }
    #endregion

    #region API
    /// <summary>
    /// Reads the settings form <see cref="XmlReader"/>.
    /// </summary>
    /// <param name="settings">An <see cref="XmlReader"/> instance encapsulating the settings.</param>
    internal void ReadSettings(XmlReader settings)
    {
      settings.ReadStartElement("ColumnSeparator");
      ColumnSeparator = settings.ReadContentAsString();
      settings.ReadEndElement();
      settings.ReadStartElement("FileModificationNotificationTimeout");
      FileModificationNotificationTimeout = settings.ReadContentAsDouble();
      settings.ReadEndElement();
      settings.ReadStartElement("DelayFileScan");
      DelayFileScan = settings.ReadContentAsDouble();
      settings.ReadEndElement();
    }
    /// <summary>
    /// Writes the settings to <see cref="XmlWriter"/>.
    /// </summary>
    /// <param name="settings">An <see cref="XmlReader"/> instance encapsulating the settings.</param>
    internal void WriteSettings(XmlWriter settings)
    {
      settings.WriteStartElement("ColumnSeparator");
      settings.WriteValue(ColumnSeparator);
      settings.WriteEndElement();
      settings.WriteStartElement("FileModificationNotificationTimeout");
      settings.WriteValue(FileModificationNotificationTimeout);
      settings.WriteEndElement();
      settings.WriteStartElement("DelayFileScan");
      settings.WriteValue(DelayFileScan);
      settings.WriteEndElement();
    }
    #endregion


  }

}
