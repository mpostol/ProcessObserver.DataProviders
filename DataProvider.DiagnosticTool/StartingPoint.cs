//_______________________________________________________________
//  Title   : DP Diagnostics starting point
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.Lib.CodeProtect;
using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace CAS.DPDiagnostics
{
  class StartingPoint
  {
    [STAThread]
    static void Main()
    {
      string _commandLine = Environment.CommandLine;
      if ( ( ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun ) || _commandLine.ToLower().Contains( "installic" ) )
        try
        {
          LibInstaller.InstallLicense( true );
        }
        catch ( Exception ex )
        {
          MessageBox.Show( "Unable to install license: " + ex.Message );
        }
      Application.Run( new Program() );
    }
  }
}
