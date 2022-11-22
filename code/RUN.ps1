# partly based on github.com/ondfisk/BDSA2021/blob/main/Start-Application.ps1
$database = "GitInsight"

$secretsProject = "Models"
$startupProject = "REST"

$localPath = "./.local/secrets/"

# create new database container or use existing
if (Test-Path $localPath) {
	Write-Host "Starting exsisting SQL Server"
	# retrieve secrets to reuse server
	$password = Get-Content $localPath/password
	$databaseId = Get-Content $localPath/databaseId
	docker container start $databaseId
} else {
	Write-Host "Creating new SQL Server"
	$password = New-Guid
	Write-Host "Database password is: $password"
	$databaseId = docker run -e "ACCEPT_EULA=y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
	#save secrets to reuse server
	New-Item -Path $localPath -Name "password" -ItemType "file" -Value $password -Force
	New-Item -Path $localPath -Name "databaseId" -ItemType "file" -Value $databaseId -Force
}

# connection string
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password;Trusted_Connection=False;Encrypt=False"

# user secrets
Write-Host "Configuring Connection String"
dotnet user-secrets init --project $secretsProject
dotnet user-secrets set "ConnectionStrings:$database" "$connectionString" --project $secretsProject

# wait to make sure docker is running
Write-Host "Waiting to make sure container is ready"

Function Sleep-Progress($seconds) { # credit to https://lazyadmin.nl/powershell/start-sleep/ for this function
	$s = 0;
	Do {
		$p = [math]::Round(100 - ((($seconds * 10) - $s) / ($seconds * 10) * 100));
		Write-Progress -Activity "Waiting..." -Status "$p% Complete:" -SecondsRemaining ($seconds - ($s / 10)) -PercentComplete $p;
		[System.Threading.Thread]::Sleep(100)
		$s++;
	}
	While ($s -lt ($seconds * 10));
    Write-Progress -Activity "Waiting..." -Status "100% Complete:" -SecondsRemaining (0) -PercentComplete 100;
}

Sleep-Progress 5

# start application
Write-Host "Starting App"
dotnet run -nowarn --project $startupProject