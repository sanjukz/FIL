if (Get-WmiObject -Class Win32_Service -Filter "Name='Redis'") {
    Write-Host "Redis is already installed"
}
else {
    Write-Host "Installing Redis..."

    pushd C:\temp

    # Download
    if (!(Test-Path "Redis-x64-2.8.2400.msi")) {
        Invoke-WebRequest https://github.com/MSOpenTech/redis/releases/download/win-2.8.2400/Redis-x64-2.8.2400.msi -OutFile Redis-x64-2.8.2400.msi
    }

    # Install
    cmd /c start /wait msiexec.exe /q /i Redis-x64-2.8.2400.msi ADDLOCAL=ALL

    pushd "C:\Program Files\Redis"
    .\redis-server.exe --version

    popd
}

# If Redis is running, stop it
if (Get-WmiObject -Class Win32_Service -Filter "Name='Redis' and State='Running'") {
    Write-Host "Stopping Redis..."
    net stop redis
}

# Set up the config file
rm "C:\Program Files\Redis\redis.windows-service.conf"

cp "$PSScriptRoot\..\redis.windows-service.conf" "C:\Program Files\Redis\redis.windows-service.conf"

# Start Redis
Write-Host "Starting Redis..."
net start redis