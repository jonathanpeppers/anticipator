# Change these if your paths are different
param
(
    [string] $adb = 'C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe',
    [string] $msbuild = 'C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
    [string] $project = '.\AnticipatorTest\AnticipatorTest.csproj',
    [string] $package = 'com.xamarin.anticipatortest',
    [string] $activity = 'com.xamarin.anticipatortest.MainActivity',
    [string] $configuration = 'Debug',
    [bool] $anticipator = $true,
    [int] $iterations = 10
)

$ErrorActionPreference = 'Stop'

if ($anticipator)
{
    $define = 'ANTICIPATOR'
}

# We need a huge logcat buffer
& $adb logcat -G 15M
& $adb logcat -c
& $msbuild $project /r /v:minimal /nologo /t:Clean,Install /p:Configuration=$configuration /p:DefineConstants=$define

for ($i = 0; $i -le $iterations; $i++)
{
    Write-Host "Launching: $package $activity"
    & $adb shell am force-stop $package
    & $msbuild $project /v:minimal /nologo /t:_Run /p:Configuration=$configuration /p:DefineConstants=$define
    Start-Sleep -Seconds 5
}

$log = & $adb logcat -d | Select-String GREPME
$total = 0;
foreach ($line in $log)
{
    if ($line -match "\s(\d+)$")
    {
        $total += [int]$Matches.1
        Write-Host $line
    }
    else
    {
        Write-Error "No timing found for line: $line"
    }
}
$average = $total / $log.Count
Write-Host "Average: $average"
