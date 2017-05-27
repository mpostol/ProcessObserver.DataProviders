//_______________________________________________________________
//  Title   : Interface IDataEntity - data holder entity
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

namespace CAS.CommServer.DataProvider.TextReader.Data
{
  /// <summary>
  /// Interface IDataEntity - data holder entity
  /// </summary>
  internal interface IDataEntity
  {
    
    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    /// <value>The time stamp.</value>
    string[] Tags { get; }
    /// <summary>
    /// Gets or sets the tags containing process data this instance captures.
    /// </summary>
    /// <value>The tags values in the form as they exist in the source file.</value>
    DateTime TimeStamp { get; }

  }
}