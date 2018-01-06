
rem//  $LastChangedDate: 2009-10-27 13:31:11 +0100 (Wt, 27 paü 2009) $
rem//  $Rev: 4116 $
rem//  $LastChangedBy: mzbrzezny $
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR31-DataProviders/Scripts/create_branch.cmd $
rem//  $Id: create_branch.cmd 4116 2009-10-27 12:31:11Z mzbrzezny $


set branchtype=tags
set TagFolder=rel_3_100_00
set TagPath=svn://svnserver.hq.cas.com.pl/VS/%branchtype%/DataProvidersAndDiagnostic/rel_3_100_00

svn mkdir %TagPath%  -m "created new %TagPath% (in %branchtype% folder)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/CommonBinaries %TagPath%/CommonBinaries -m "created copy in %TagPath% of the project CommonBinaries"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR24-Biblioteka %TagPath%/PR24-Biblioteka -m "created copy in %TagPath% of the PR24-Biblioteka"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR31-DataProviders %TagPath%/PR31-DataProviders -m "created copy in %TagPath% of the project project PR31-DataProviders"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR39-CommonResources %TagPath%/PR39-CommonResources -m "created copy in %TagPath% of the project PR39-CommonResources"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR45-DataProviderDiagnostic %TagPath%/PR45-DataProviderDiagnostic -m "created copy in %TagPath% of the project PR45-DataProviderDiagnostic"

pause ....

