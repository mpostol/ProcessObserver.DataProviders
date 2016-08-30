//<summary>
//  Title   : MBUS application layer message extendable data informations
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080813: mzbrzezny: description is added to MBUSApplicationLayerExtendableDataInformation class
//  20080812: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE.MBUSApplicationLayerMessage
{
  internal class MBUSApplicationLayerExtendableDataInformation
  {
    protected bool extension=false;
    protected string description="not set";
    internal bool Extension
    {
      get { return extension; }
    }
    internal string Description
    {
      get { return description; }
    }
  }
}
