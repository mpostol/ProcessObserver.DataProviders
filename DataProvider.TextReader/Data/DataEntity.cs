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

namespace CAS.CommServer.DataProvider.TextReader.Data
{
  /// <summary>
  /// Class DataEntity - data holder entity
  /// </summary>
  /// <seealso cref="CAS.Lib.CommonBus.ApplicationLayer.IReadValue" />
  internal class DataEntity
  {

    #region public API
    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    /// <value>The time stamp.</value>
    public DateTime TimeStamp { get; set; }
    /// <summary>
    /// Gets or sets the tags containing process data this instance captures.
    /// </summary>
    /// <value>The tags values in the form as they exist in the source file.</value>
    public string[] Tags { get; set; }
    #endregion

  }

}
