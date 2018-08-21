################################################################################
##  File:  Install-Firefox.ps1
##  Team:  Automated Testing
##  Desc:  Install Mozilla Firefox
################################################################################

function Install-EXE
{
    Param
    (
        [String]$Url,
        [String]$Name,
        [String[]]$ArgumentList
    )

    $exitCode = -1

    try
    {
        Write-Host "Downloading $Name..."
        $FilePath = "${env:Temp}\$Name"

        Invoke-WebRequest -Uri $Url -OutFile $FilePath

        Write-Host "Starting Install $Name..."
        $process = Start-Process -FilePath $FilePath -ArgumentList $ArgumentList -Wait -PassThru
        $exitCode = $process.ExitCode

        if ($exitCode -eq 0 -or $exitCode -eq 3010)
        {
            Write-Host -Object 'Installation successful'
            return $exitCode
        }
        else
        {
            Write-Host -Object "Non zero exit code returned by the installation process : $exitCode."
            return $exitCode
        }
    }
    catch
    {
        Write-Host -Object "Failed to install the Executable $Name"
        Write-Host -Object $_.Exception.Message
        return -1
    }
}

Install-EXE -Url "https://seleniumwebdrivers.blob.core.windows.net/knownfirefoxversion/Firefox%20Installer.exe" -Name "Firefox"

$path = '{0}\Program Files\Mozilla Firefox\' -f $env:SystemDrive;
New-Item -path $path -Name 'mozilla.cfg' -Value '//
pref("browser.shell.checkDefaultBrowser", false);
pref("app.update.enabled", false);' -ItemType file -force

$path = '{0}\Program Files\Mozilla Firefox\defaults\pref\' -f $env:SystemDrive;
New-Item -path $path -Name 'local-settings.js' -Value 'pref("general.config.obscure_value", 0);
pref("general.config.filename", "mozilla.cfg");' -ItemType file -force