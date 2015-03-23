**NOTICE**: This project is no longer maintained. If you would like commit access to the repository please open up an issue. If you are still using this in production I would like to hear from you as well. It was fun putting this together but we eventually moved to different framework.

# Stitch

This is a port of Sam Stephenson's [Stich](https://github.com/sstephenson/stitch) and Alex MacCaw's [Stitch-rb](https://github.com/maccman/stitch-rb) to ASP.NET. Stitch allows you to:

> Develop and test your JavaScript applications as CommonJS modules in Node.js. Then __Stitch__ them together to run in the browser.

In other words, this is a [CommonJS](http://dailyjs.com/2010/10/18/modules/) JavaScript package management solution. It's terribly simple, bundling up all your applications JavaScript files without intelligently resolving dependencies. However, unless your application is very modular, it turns out thats all you need.

# Usage

Install the nuget package into a web project.

> Install-Package Stitch

This will modify your Web.config adding a HttpHandler for any files *.stitch.

Add a reference to a stitch file in your application page.

> <script type="text/javascript" src="application.stitch"></script>

This reference will get caught by the HttpHandler and compile the stiched version of your application.

# Configuration

The nuget package will add three things to your Web.config.
  1. Define a new section called stitch
  2. Add the HttpHandler for all *.stitch files
  3. Add the stitch section
  
```xml
<configuration>
  <configSections>
    <section name="stitch" type="Stitch.Web.StitchConfigurationSection, Stitch.Web"/>
  </configSections>

  <system.web>
    <compilation debug="true" targetFramework="4.0" />
    <httpHandlers>
      <add type="Stitch.Web.StitchHttpHandler, Stitch.Web" verb="*" path="*.stitch" />
    </httpHandlers>
  </system.web>

  <stitch>
    <paths>
      <path>scripts/app</path>
    </paths>

    <dependencies>
      <file>lib/dep.js</file>
    </dependencies>

    <compilers>
      <compiler type="Stitch.Compilers.CoffeeScriptCompiler, Stitch.Core"/>
      <compiler type="Stitch.Compilers.JavaScriptCompiler, Stitch.Core"/>
    </compilers>
  </stitch>

</configuration>
```

You can configure several different options here.

The first section <paths> contains a entry for each directory you want compiled into the same application file. Each directory here will compile all sub directories.

The second section <dependencies> will be included in the application file but without encapsulating or compiling. They will simply be appended to the top.

The third section <compilers> lists out the different compilers that are possible. Currently there is a CoffeeScriptCompiler and a JavaScriptCompiler. 

# ASP.NET MVC

In order for this to work in ASP.NET MVC you need to set it up to ignore the stitch files in your routing. Adding this line after the axd ignore should do it.

> routes.IgnoreRoute("{*stitch}", new { stitch = @".*\.stitch(/.*)?" });

# Command Line

Another option to generate your application is through the Stitch.exe command line application. The options are as follows.

```
Stitch Command Line Compiler 0.1.0.14
Copyright (C) 2011 Nathan Palmer
http://github.com/nathanpalmer/stitch-aspnet
 
Stitch [destination] options
 
  -r, --root=VALUE           Root path (default is working directory)
  -p, --paths=VALUE          Comma delimited list of paths that should be compiled
  -d, --dep=VALUE            Comma delimited list of dependencies that should be included
  -i, --identifier=VALUE     Identifier to use for including other files (default is require)
  -c, --compilers=VALUE      Comma delimited list of compilers to use (default is CoffeeScriptCompiler, JavaScriptCompiler)
```
```
