using System;
using System.Reflection;

[assembly: CLSCompliant(false)]
[assembly: AssemblyDefaultAlias("Cavity.MSBuild.Facts.dll")]
[assembly: AssemblyTitle("Cavity.MSBuild.Facts.dll")]

#if (DEBUG)

[assembly: AssemblyDescription("Cavity : MSBuild Facts Library (Debug)")]

#else

[assembly: AssemblyDescription("Cavity : MSBuild Facts Library (Release)")]

#endif