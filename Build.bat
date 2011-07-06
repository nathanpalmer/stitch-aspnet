@echo off

if '%2' NEQ '' goto usage
if '%3' NEQ '' goto usage
if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

SET DIR=%~d0%~p0%
SET PHANTOM="%DIR%Tools\Phantom\Phantom.exe"

%PHANTOM% %1 

if %ERRORLEVEL% NEQ 0 goto errors

goto finish

:usage
echo.
echo Usage: Build.bat
echo.
goto finish

:errors
pause
EXIT /B %ERRORLEVEL%

:finish
pause