//<summary>
//  Title   : DP Diagnostics starting point
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//   20090119: mschabowski: try catch - in licence installlation is added
//    mzbrzezny - 2007:
//    created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http://www.cas.eu
//</summary>

using System;
using System.Deployment.Application;
using System.Windows.Forms;
using CAS.Lib.CodeProtect;

namespace CAS.DPDiagnostics
{
  class StartingPoint
  {
    [STAThread]
    static void Main()
    {
      string m_cmmdLine = Environment.CommandLine;
      if ( ( ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun ) || m_cmmdLine.ToLower().Contains( "installic" ) )
        try
        {
          LibInstaller.InstalLicense( true );
        }
        catch ( Exception ex )
        {
          MessageBox.Show( "Unable to install license: " + ex.Message );
        }
      Application.Run( new Program() );
    }
  }
}
