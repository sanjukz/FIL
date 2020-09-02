param (
    [switch] $force
)

if ($force -ne $true -and (Get-Command "choco" -errorAction SilentlyContinue))
{
    "Chocolatey already installed."
} else {
	iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
}
