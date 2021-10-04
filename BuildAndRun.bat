@echo off
echo "Before running this script set ApiKey and SearchEngineID"

SET ApiKey="placeholderKey"
SET SearchEngineID="placeholderId"

pushd %~dp0
cd %~dp0src\HappyQuotes.WebAPI

@echo on

dotnet build -c Release
dotnet run GoogleCustomSearch:ApiKey=%ApiKey% GoogleCustomSearch:SearchEngineID=%SearchEngineID% --urls "http://localhost:80;https://localhost:443"

@echo off

popd