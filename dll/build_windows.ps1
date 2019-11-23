$ErrorActionPreference = "Stop"

$here = (Get-Item $MyInvocation.MyCommand.Path).Directory
# echo $here.FullName

$vswhere = $here.GetFiles("vswhere.exe")[0].FullName
# echo $vswhere

$msbuild = &$vswhere -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | select-object -first 1
# echo $msbuild

$cmake_base = &$vswhere -latest -products * -requires Microsoft.VisualStudio.Component.VC.CMake.Project -property installationPath 
$cmake = (Join-Path -path $cmake_base -child "Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe")
# echo $cmake

$dst = (Join-Path -path $here.FullName -child "build")
if (!(Test-Path -path $dst)) {
    mkdir $dst -Force
}

Push-Location $dst
&$cmake $here.FullName
&$msbuild Sample.sln /p:Configuration=Release
Pop-Location

$dll = (Join-Path -path $here.FullName -child "build/Release/bin/Sample.dll")

copy $dll $here.Parent.FullName
