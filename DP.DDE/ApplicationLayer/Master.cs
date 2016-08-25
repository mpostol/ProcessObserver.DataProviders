//<summary>
//  Title   : DDE implementation of IApplicationLayerMaster
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47 in write
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.DDE
{
  /// <summary>
  ///  DDE implementation of the IApplicationLayerMaster
  /// </summary>
  internal class Master: ApplicationLayerMaster<Message>
  {
    #region PRIVATE
    /// <summary>
    ///  Title   : NULL implementation of IApplicationLayerMaster
    /// </summary>
    private class DDE_buf_pool: CommunicationLayer.Generic.SesDBufferPool<Message>
    {
      protected override Message CreateISesDBuffer()
      {
        return new Message( this );
      }
    }//NULL_buf_pool
    private DDE_buf_pool m_Pool = new DDE_buf_pool();
    private int m_errorfrequency = 100;
    private ulong m_rPackNum = 0;
    private ulong m_wPackNum = 0;
    private CAS.Lib.RTLib.Management.IProtocolParent myStatistic;
    #endregion

    #region creator
    /// <summary>
    /// ApplicationLayerMaster creator
    /// </summary>
    /// <param name="cPool">Empty data messages pool to be used by the protocol.</param>
    /// <param name="cProt">Protocol to be used.</param>
    internal Master( SesDBufferPool<Message> cPool, Protocol cProt )
      : base( cProt, cPool ) { }

    #endregion
  }
}
