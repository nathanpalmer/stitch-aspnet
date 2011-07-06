import System
import System.IO

# Project Information
title            = "Stitch-AspNet"
company          = ""
version_major    = 0
version_minor    = 1
version_build    = 0
version_revision = 0
repository_url   = ""
repository_type  = "git"

# Build Information
solution_folder  = "Source"
solution_file    = "Stitch-AspNet.sln"
configuration    = "Release"
dotnet_version   = "4.0"
build_directory  = DirectoryInfo("Build/${configuration}")
package_dir      = "Build/${configuration}"

# Targets
desc "Default target"
target default, (clean, binaries, compile, package):
  pass
  
target clean:
  rmdir("Build")
  
target binaries:
  exec("Tools\\NuGet\\NuGet.exe install ${solution_folder}\\Stitch-AspNet\\packages.config -o Libraries")
  
target compile, (assemblyInfo):
  print "Compiling ${solution_file}"
  with msbuild():
    .file = Path.Combine(solution_folder, solution_file)
    .configuration = configuration
    .version = dotnet_version
    .properties = { 'OutDir': build_directory.FullName+"/" }

target package:
  zip(package_dir, String.Format("Build/{4}-{0}.{1}.{2}.{3}.zip", version_major, version_minor, version_build, version_revision, title))
  
target getRevision:
  int.TryParse(env("build.vcs.number"), version_revision)
  if (version_revision > 0):
    return
    
  if (repository_type == "git"):
    version_revision = getRevisionMercurial(repository_url)
    
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