@ECHO off

FOR /f "tokens=1,2*" %%a IN ('date /T') DO SET date=%%a %%b

SET y=%date:~6,4%
SET m=%date:~0,2%
SET d=%date:~3,2%

FOR /f "tokens=1-2 delims=/:" %%a IN ("%TIME%") DO (SET t=%%a%%b)

SET utc=%y%-%m%-%d%+%t%

MSBUILD /m build.xml /l:FileLogger,Microsoft.Build.Engine;logfile="%utc% build.log";append=true;verbosity=diagnostic;encoding=utf-8

:END