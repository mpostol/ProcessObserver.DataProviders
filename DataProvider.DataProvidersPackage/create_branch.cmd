rem//  $LastChangedDate$
rem//  $Rev$
rem//  $LastChangedBy$
rem//  $URL$
rem//  $Id$

if "%1"=="" goto ERROR
set branchtype=%2
if "%branchtype%"=="" goto setbranch

:dothejob
svn mkdir svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR31-DataProviders/%1  -m "created new %branchtype%  %1 (%branchtype% folder)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/ImageLibrary svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR31-DataProviders/%1/ImageLibrary -m "created new %branchtype% %1 (project ImageLibrary)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/CommonBinaries svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR31-DataProviders/%1/CommonBinaries -m "created new %branchtype% %1 (project CommonBinaries)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR31-DataProviders svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR31-DataProviders/%1/PR31-DataProviders -m "created new %branchtype% %1 (project PR31-DataProviders)"

goto EXIT

:setbranch
set branchtype=branches
goto dothejob
:ERROR
echo Parametr must be set
:EXIT

pause ....
