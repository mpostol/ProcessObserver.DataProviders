//_______________________________________________________________
//  Title   : TextReaderCommunicationLayerId
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

using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.Xml;
using System;
using System.ComponentModel;
using System.Xml;

namespace CAS.CommServer.DataProvider.TextReader.CommunicationLayer
{
  //internal class TextReaderCommunicationLayerId : ICommunicationLayerId
  //{

  //  #region MyRegion
  //  [DisplayName("Text Reader Communication Layer")]
  //  [DescriptionAttribute("Text Reader working parameter settings.")]
  //  [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
  //  public ICommunicationLayerDescription GetCommunicationLayerDescription
  //  {
  //    get; private set;
  //  } = new TextReaderCommunicationLayerDescription();
  //  public string Title
  //  {
  //    get
  //    {
  //      return GetCommunicationLayerDescription.Title;
  //    }
  //  }
  //  public ICommunicationLayer CreateCommunicationLayer(object[] parameter)
  //  {
  //    throw new NotImplementedException();
  //  }
  //  public ICommunicationLayer CreateCommunicationLayer(CommonBusControl cParent)
  //  {
  //    throw new NotImplementedException();
  //  }
  //  private const string m_TextReaderCommunicationLayerIdTag = "TextReaderCommunicationLayerId";
  //  private const string m_RequestTimeoutTag = "RequestTimeout";
  //  private int m_RequestTimeout;

  //  public void GetSettings(XmlWriter pSettings)
  //  {
  //    pSettings.WriteStartElement(m_TextReaderCommunicationLayerIdTag);
  //    XmlHelper.WriteStandardIntegerVale(pSettings, m_RequestTimeoutTag, 500);
  //    pSettings.WriteEndElement();
  //  }
  //  public void SetSettings(XmlReader pSettings)
  //  {
  //    if (!pSettings.IsStartElement(m_TextReaderCommunicationLayerIdTag))
  //      throw new XmlException(string.Format("Expected element {0} has not be found at current position of the configuration file", m_TextReaderCommunicationLayerIdTag));
  //    m_RequestTimeout = XmlHelper.ReadStandardIntegerValue(pSettings, m_RequestTimeoutTag);
  //    pSettings.ReadStartElement(m_TextReaderCommunicationLayerIdTag);
  //  }
  //  #endregion

  //}
}
