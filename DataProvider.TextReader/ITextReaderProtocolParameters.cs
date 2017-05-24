﻿//_______________________________________________________________
//  Title   : ITextReaderProtocolParameters
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

namespace CAS.CommServer.DataProvider.TextReader
{
  /// <summary>
  /// Interface ITextReaderProtocolParameters - provides parameters of the DataProvider behaviour.
  /// </summary>

  public interface ITextReaderProtocolParameters
  {
    /// <summary>
    /// Gets the column separator - string used to separate columns in the scanned text.
    /// </summary>
    /// <value>The column separator.</value>
    string ColumnSeparator { get; }
    /// <summary>
    /// Gets the delay file scann - it is time to postpone the file content read operation after receiving file modification notification. 
    /// It is time needed by the remote application to finalize writing to file and release the file for other processes.
    /// </summary>
    /// <value>The delay file scann.</value>
    double DelayFileScann { get; }
    /// <summary>
    /// Gets or sets the file modification notification timeout.
    /// </summary>
    /// <value><see cref="double"/> representing the file modification notification timeout.</value>
    double FileModificationNotificationTimeout { get; }
  }
}