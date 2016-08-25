echo off
rem//  $LastChangedDate$
rem//  $Rev$
rem//  $LastChangedBy$
rem//  $URL$
rem//  $Id$


rem DEL [/P] [/F] [/S] [/Q] [/A[[:]attributes]] names
rem   names         Specifies a list of one or more files or directories.
rem                 Wildcards may be used to delete multiple files. If a
rem                 directory is specified, all files within the directory
rem                 will be deleted.
rem 
rem   /P            Prompts for confirmation before deleting each file.
rem   /F            Force deleting of read-only files.
rem   /S            Delete specified files from all subdirectories.
rem   /Q            Quiet mode, do not ask if ok to delete on global wildcard
rem   /A            Selects files to delete based on attributes
rem   attributes    R  Read-only files            S  System files
rem                 H  Hidden files               A  Files ready for archiving
rem                 -  Prefix meaning not
REM RMDIR [/S] [/Q] [drive:]path
REM RD [/S] [/Q] [drive:]path

REM     /S      Removes all directories and files in the specified directory
REM             in addition to the directory itself.  Used to remove a directory
REM             tree.

REM     /Q      Quiet mode, do not ask if ok to remove a directory tree with /S
rem all cleanup should be performed in the following directories:
rem EX01-OPCFoundation_NETApi
rem IN12XX-SampleHTTPSoapOPCClient
rem PR24-Biblioteka
rem PR30-OPCViever
echo "$URL$"
cd ..\..\

if "%1"=="local" goto LOCAL
del /Q  .\CommonBinaries\PR31-DataProviders\*.*
:LOCAL

RD /S /Q .\PR31-DataProviders\DP.DDE\obj 
RD /S /Q .\PR31-DataProviders\DP.DDE\bin 
RD /S /Q .\PR31-DataProviders\DP.DemoSimulator\obj 
RD /S /Q .\PR31-DataProviders\DP.DemoSimulator\bin 
RD /S /Q .\PR31-DataProviders\DP.EC2-3SYM2\obj 
RD /S /Q .\PR31-DataProviders\DP.EC2-3SYM2\bin 
RD /S /Q .\PR31-DataProviders\DP.MBUS\obj 
RD /S /Q .\PR31-DataProviders\DP.MBUS\bin 
RD /S /Q .\PR31-DataProviders\DP.MODBUS\obj 
RD /S /Q .\PR31-DataProviders\DP.MODBUS\bin 
RD /S /Q .\PR31-DataProviders\DP.NULLbus\obj 
RD /S /Q .\PR31-DataProviders\DP.NULLbus\bin 
RD /S /Q .\PR31-DataProviders\DP.SBUS\bin 
RD /S /Q .\PR31-DataProviders\DP.SBUS\bin 
RD /S /Q .\PR31-DataProviders\DP.SymSiec\obj 
RD /S /Q .\PR31-DataProviders\DP.SymSiec\bin 
RD /S /Q .\PR31-DataProviders\DP.UT\NET\obj 
RD /S /Q .\PR31-DataProviders\DP.UT\NET\bin 
RD /S /Q .\PR31-DataProviders\DP.UT\RS\obj 
RD /S /Q .\PR31-DataProviders\DP.UT\RS\bin 
RD /S /Q .\PR31-DataProviders\PR31-DataProvidersPackage\obj 
RD /S /Q .\PR31-DataProviders\PR31-DataProvidersPackage\bin 
RD /S /Q .\PR31-DataProviders\DP.SBUS\SBUSonNET\obj 
RD /S /Q .\PR31-DataProviders\DP.SBUS\SBUSonRS\obj 
rem deleting project user files
del /F /S /Q /A:H .\PR31-DataProviders\*.suo
del /F /S /Q /A:H .\PR31-DataProviders\*.user
del /F /S /Q  .\PR31-DataProviders\*.user
rem deleting objects
del /F /S /Q  .\PR31-DataProviders\*.obj
rem deleting intellisence
del /F /S /Q  .\PR31-DataProviders\*.ncb
rem deleting debuger informations
del /F /S /Q  .\PR31-DataProviders\*.pdb
rem deletind desktop.ini
del /F /S /Q /A:H .\PR31-DataProviders\*.ini
rem returning to base directory
cd .\PR31-DataProviders\Scripts