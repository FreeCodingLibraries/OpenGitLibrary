@ECHO OFF

set releaseVersion=%1
set progetRepo=%2
set ecrRepo=%3
set rid=win-x64
set project=App.Service\App.Service.csproj
set buildType=Release
set output=.\dist

:: Restore & Publish
dotnet restore %project% -r %rid%
dotnet publish %project% --nologo -c %buildType% -o %output% -r %rid% --self-contained true /p:PublishTrimmed=false /p:PublishSingleFile=false /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true /p:Version=%releaseVersion%

:: Build the container.
docker build --platform windows --rm -t %progetRepo%:%releaseVersion% -t %progetRepo%:latest -t %ecrRepo%:%releaseVersion% -t %ecrRepo%:latest -f App.Service\Dockerfile .

:: Publish the container.
docker push %ecrRepo%:%releaseVersion% & docker push %ecrRepo%:latest
