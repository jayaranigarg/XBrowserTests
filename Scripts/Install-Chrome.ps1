################################################################################
##  File:  Install-Chrome.ps1
##  Team:  Automated Testing
##  Desc:  Install Google Chrome
################################################################################

function Install-MSI
{
    Param
    (
        [String]$MsiUrl,
        [String]$MsiName
    )

    $exitCode = -1

    try
    {
        Write-Host "Downloading $MsiName..."
        $FilePath = "${env:Temp}\$MsiName"

        Invoke-WebRequest -Uri $MsiUrl -OutFile $FilePath

        $Arguments = ('/i', $FilePath, '/QN', '/norestart' )

        Write-Host "Starting Install $MsiName..."
        $process = Start-Process -FilePath msiexec.exe -ArgumentList $Arguments -Wait -PassThru
        $exitCode = $process.ExitCode

        if ($exitCode -eq 0 -or $exitCode -eq 3010)
        {
            Write-Host -Object 'Installation successful'
            return $exitCode
        }
        else
        {
            Write-Host -Object "Non zero exit code returned by the installation process : $exitCode."
            exit $exitCode
        }
    }
    catch
    {
        Write-Host -Object "Failed to install the MSI $MsiName"
        Write-Host -Object $_.Exception.Message
        exit -1
    }
}

$temp_install_dir = 'C:\Windows\Installer'
New-Item -Path $temp_install_dir -ItemType Directory -Force

Install-MSI -MsiUrl "https://seleniumwebdrivers.blob.core.windows.net/knownchromeversion/googlechromestandaloneenterprise64.msi" -MsiName "googlechromestandaloneenterprise64.msi"

New-NetFirewallRule -DisplayName "BlockGoogleUpdate" -Direction Outbound -Action Block -Program "C:\Program Files (x86)\Google\Update\GoogleUpdate.exe"

Stop-Service -Name gupdate -Force
Set-Service -Name gupdate -StartupType "Disabled"
Stop-Service -Name gupdatem -Force
Set-Service -Name gupdatem -StartupType "Disabled"

New-Item -Path "HKLM:\SOFTWARE\Policies\Google\Update" -Force
New-ItemProperty "HKLM:\SOFTWARE\Policies\Google\Update" -Name "AutoUpdateCheckPeriodMinutes" -Value 00000000 -Force
New-ItemProperty "HKLM:\SOFTWARE\Policies\Google\Update" -Name "UpdateDefault" -Value 00000000 -Force
New-ItemProperty "HKLM:\SOFTWARE\Policies\Google\Update" -Name "DisableAutoUpdateChecksCheckboxValue" -Value 00000001 -Force
New-ItemProperty "HKLM:\SOFTWARE\Policies\Google\Update" -Name "Update{8A69D345-D564-463C-AFF1-A69D9E530F96}" -Value 00000000 -Force
New-Item -Path "HKLM:\SOFTWARE\Policies\Google\Chrome" -Force
New-ItemProperty "HKLM:\SOFTWARE\Policies\Google\Chrome" -Name "DefaultBrowserSettingEnabled" -Value 00000000 -Force