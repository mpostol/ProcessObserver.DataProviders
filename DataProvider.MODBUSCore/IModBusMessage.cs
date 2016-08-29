//_______________________________________________________________
//  Title   : IModBusMessage
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

namespace CAS.CommServer.DataProvider.MODBUSCore
{
  /// <summary>
  /// Interface IModBusMessage
  /// </summary>
  public interface IModBusMessage
  {
    short address { get; }
    Modbus_Functions dataType { get; }
    bool Registers32AreUsed { get; set; }
    int reqQuantity { get; }
    byte station { get; set; }
  }
}