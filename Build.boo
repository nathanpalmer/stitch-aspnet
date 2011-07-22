import System
import System.IO

# Project Information
title            = "Stitch"
company          = ""
version_major    = 0
version_minor    = 2
version_build    = 0
version_revision = 0
version          = "0.2.0.0"
repository_url   = ""
repository_type  = "git"

# Build Information
solution_folder  = "Source"
solution_file    = "Stitch.sln"
configuration    = "Release"
dotnet_version   = "4.0"
build_directory  = DirectoryInfo("Build/${configuration}")

# Targets
desc "Default target"
target default:
  try:
    call clean
    call binaries
    call assemblyInfo
    call compile
    call package
    call nuget
  except e:
    print e
  
target clean:
  rmdir("Build")
  
target binaries:
  with FileList(solution_folder):
    .Include("*")
    .ForEach def(file):
        packages_config = Path.Combine(file.FullName,"packages.config")
        if Directory.Exists(file.FullName) and File.Exists(packages_config):
            exec("Tools\\NuGet\\NuGet.exe install ${packages_config} -o Libraries") 
  
target compile:
  print "Compiling ${solution_file}"
  with msbuild():
    .file = Path.Combine(solution_folder, solution_file)
    .configuration = configuration
    .version = dotnet_version
    .properties = { 'OutDir': build_directory.FullName+"/" }

target package:
  package_dir = "Build/Package"
  mkdir package_dir
  cp("Build/${configuration}/Stitch.Core.dll", package_dir + "/Stitch.Core.dll")
  cp("Build/${configuration}/Stitch.Web.dll", package_dir + "/Stitch.Web.dll")
  cp("Build/${configuration}/Stitch.exe", package_dir + "/Stitch.exe")
  zip(package_dir, String.Format("Build/{4}.{0}.{1}.{2}.{3}.zip", version_major, version_minor, version_build, version_revision, title))
  rmdir package_dir
  
target getRevision:
  int.TryParse(env("build.vcs.number"), version_revision)
  if (version_revision > 0):
    return
    
  if (repository_type == "git"):
    exec("git describe --tags --long", { 'Output': '_summary.txt' })
    
    file = StreamReader("_summary.txt")
    line = file.ReadLine()
    file.Close()
    file.Dispose()
  
    rm("_summary.txt")
    
    parts = line.Split(char('-'))  
    
    print parts[1]
    
    revision = -1
    if (int.TryParse(parts[1].Trim(),revision)):
      version_revision = revision
    
  if (version_revision >= 0):  
    return
  else:
    raise "Unable to determine repository revision"
    
target assemblyInfo, (getRevision):
  print "Generating SolutionVersion.cs"
  version = String.Format("{0}.{1}.{2}.{3}", version_major, version_minor, version_build, version_revision)
  with generate_assembly_info():
    .file = "${solution_folder}/SolutionVersion.cs"
    .version = version
    .fileVersion = version
    .title = title
    .description  = String.Format("{0} is a product of {1}", title, company)
    .copyright = String.Format("Copyright {0} {1}", DateTime.Now.Year, company)
    .comVisible = false
    .companyName = company
    .productName = title 
    
def createNuget(name as string, nuspec as string):
  rm(name) if File.Exists(name)
  file = StreamWriter(name)
  file.Write(nuspec)
  file.Close()
  exec(".\\Tools\\NuGet\\NuGet.exe pack .\\${name} -OutputDirectory Build")
  rm(name)
    
target nuget, (assemblyInfo):
  createNuget("Stitch.nuspec", """<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
    <metadata>
        <id>Stitch</id>
        <version>${version}</version>
        <authors>Nathan Palmer</authors>
        <description>Develop and test your JavaScript applications as CommonJS modules in Node.js. Then __Stitch__ them together to run in the browser. Port of Sam Stephenson's Stitch.</description>
        <language>en-GB</language>
        <projectUrl>https://github.com/nathanpalmer/stitch-aspnet</projectUrl>
        <licenseUrl>https://github.com/nathanpalmer/stitch-aspnet/blob/master/LICENSE.txt</licenseUrl>
    </metadata>
    <files>
        <file src="Build\Release\Stitch.Core.dll"
              target="lib" />
        <file src="Build\Release\Stitch.exe"
              target="bin" />
        <file src="LICENSE.txt"
              target="" />
    </files>
</package>
  """)

  createNuget("Stitch.AspNet.nuspec", """<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
    <metadata>
        <id>Stitch.AspNet</id>
        <version>${version}</version>
        <authors>Nathan Palmer</authors>
        <description>Develop and test your JavaScript applications as CommonJS modules in Node.js. Then __Stitch__ them together to run in the browser. Port of Sam Stephenson's Stitch.</description>
        <language>en-GB</language>
        <projectUrl>https://github.com/nathanpalmer/stitch-aspnet</projectUrl>
        <licenseUrl>https://github.com/nathanpalmer/stitch-aspnet/blob/master/LICENSE.txt</licenseUrl>
        <dependencies>
          <dependency id="Stitch" version="${version_major}.${version_minor}" />
        </dependencies>
    </metadata>
    <files>
        <file src="Build\Release\Stitch.Web.dll"
              target="lib" />
        <file src="LICENSE.txt"
              target="" />
        <file src="Web.config.transform"
              target="content" />
    </files>
</package>
  """)
  
  createNuget("Stitch.Compilers.CoffeeScript.nuspec", """<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
    <metadata>
        <id>Stitch.Compilers.CoffeeScript</id>
        <version>${version}</version>
        <authors>Nathan Palmer</authors>
        <description>CoffeeScript compiler for use with Stitch.</description>
        <language>en-GB</language>
        <projectUrl>https://github.com/nathanpalmer/stitch-aspnet</projectUrl>
        <licenseUrl>https://github.com/nathanpalmer/stitch-aspnet/blob/master/LICENSE.txt</licenseUrl>
        <dependencies>
            <dependency id="SassAndCoffee.Core" version="1.0" />
            <dependency id="Stitch" version="${version_major}.${version_minor}" />
        </dependencies>
    </metadata>
    <files>
        <file src="Build\Release\Stitch.Compilers.CoffeeScript.dll"
              target="lib" />
        <file src="LICENSE.txt"
              target="" />
    </files>
</package>
  """)
  