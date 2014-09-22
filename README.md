Ref-Restrict
============

Allows you to restrict what references can be added to projects in Visual Studio. See my blog post [here](http://www.benibinson.com/blog/2014/8/31/restrict-project-references-with-ref-restrict) on how Ref-Restrict can help you maintain a clean project structure.

Installation guide
------------------
The easiest way to install Ref-Restrict is to use the Nuget package which can be found [here](https://www.nuget.org/packages/RefRestrict/). Installing the nuget the package to a project will do the following...

- Ensures that the `RefRestrict.exe` is copied into the solution folder.
- If this is the first project the package is installed to, it will create a blank `RefRestrict.config.xml` config file in the solution folder.
- Ensures that there is a reference to the `RefRestrict.config.xml` config file in the `Solution Items` folder available in the solution of the project.
- Adds an empty `<rules project="PROJECTNAME"></rules>` node to the config file ready to add your custom restrictions.
- Adds an addition pre-build step for the project which will check your custom restrictions and stop the build if any of them are violated.


Using Ref-Restrict Manually
---------------------------
The reference checks can be execute manually by running the RefRestrict binary in the command line for an individual project. Any violations will be reported to the console
```bash
RefRestrict.exe RefRestrict.config.xml YourProjectFile.csproj
```

Configuring Ref-Restrict for your Project
---------------------------------------------
The reference restrictions are defined in the `RefRestrict.config.xml` config file. There is one config file for all the projects in your solution and should be located in the Solution Folder. 
```xml
<rrconfig>
  <rules project="ProjectA">
    <!-- Add all restrictions for Project A here --->
  </rules>
  <rules project="ProjectB">
    <!-- Add all restrictions for Project B here --->
  </rules>
  <!-- etc... -->
</rrconfig>
```

Ref Restrict Rules
------------------

**No local references** - Check that the project has no references to any other projects in the solution.
```xml
<nolocalrefs/>
```

**Only local references** - Check that the project only references the specified projects, and no other projects in the solutions.
```xml
<onlylocalrefs>
  <project>Project A</project>
  <project>Project B</project>
</onlylocalrefs>
```

**Include** - The project **must** include the specified reference
```xml
<include>System.Threading</include>
```

**Exclude** - The project **must not** include the specified reference
```xml
<exclude>System.Timer</exclude>
```

Configuration Examples
----------------------
Imagine a Visual studio solution containing the following projects, which are used for a Calculator app.

- **CalcGraphics** - A graphics driver used to render the calculator graphics
- **CalcMaths** - A low level custom maths library used to perform some of the calculator operations and graphics calculations
- **CalcUI** - Contains the UI for the application
 
The following restrictions need to be applied
- The graphics driver should only reference the maths library and not the UI project. It should also be the only project that uses the Graphics.API third party library
- The maths library must not be referenced by any other projects in the solution (have no local dependacies so it can be used in other solutions).
- The UI project must include the graphics driver to render the UI, but may or may not reference maths library.

This config file will ensure these restrictions are enforced every time the projects are built.
```xml
<rrconfig>
  <rules project="CalcGraphics">
    <onlylocalrefs>
      <project>CalcMaths</project>
    </onlylocalrefs>
    <include>Graphics.API</include>
  </rules>
  <rules project="CalcMaths">
    <nolocalrefs/>
    <exclude>Graphics.API</exclude>
  </rules>
  <rules project="CalcUI">
    <include>CalcGraphics</include>
    <exclude>Graphics.API</exclude>
  </rules>
</rrconfig>
```
For a more advanced example and to see how the restriction violations are displayed in Visual Studio, please download the RefRestrictDemo solution.
