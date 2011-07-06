import System
import System.IO

# Project Information
title            = "Stitch"
company          = ""
version_major    = 0
version_minor    = 1
version_build    = 0
version_revision = 0
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
    call compile
    call package
  except e:
    print e
  
target clean:
  rmdir("Build")
  
target binaries:
  exec("Tools\\NuGet\\NuGet.exe install ${solution_folder}\\Stitch.Core\\packages.config -o Libraries")
  
target compile, (assemblyInfo):
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
  zip(package_dir, String.Format("Build/{4}-{0}.{1}.{2}.{3}.zip", version_major, version_minor, version_build, version_revision, title))
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
    
    revision = 0
    if (int.TryParse(parts[1].Trim(),revision)):
      version_revision = revision
    
  if (version_revision > 0):  
    return
  else:
    raise "Unable to determine repository revision"
    
desc "Create the assembly info"
target assemblyInfo, (getRevision):
  print "Generating SolutionVersion.cs"
  with generate_assembly_info():
    .file = "${solution_folder}/SolutionVersion.cs"
    .version = String.Format("{0}.{1}.{2}.{3}", version_major, version_minor, version_build, version_revision)
    .fileVersion = String.Format("{0}.{1}.{2}.{3}", version_major, version_minor, version_build, version_revision)
    .title = title
    .description  = String.Format("{0} is a product of {1}", title, company)
    .copyright = String.Format("Copyright {0} {1}", DateTime.Now.Year, company)
    .comVisible = false
    .companyName = company
    .productName = title    