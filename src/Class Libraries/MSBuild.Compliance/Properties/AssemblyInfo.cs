using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: CLSCompliant(true)]
[assembly: AssemblyDefaultAlias("Cavity.MSBuild.dll")]
[assembly: AssemblyTitle("Cavity.MSBuild.dll")]

#if (DEBUG)

[assembly: AssemblyDescription("Cavity : MSBuild Library (Debug)")]

#else

[assembly: AssemblyDescription("Cavity : MSBuild Library (Release)")]

#endif

[assembly: SuppressMessage("Microsoft.Naming", "CA1701:ResourceStringCompoundWordsShouldBeCasedCorrectly", MessageId = "XPath", Scope = "resource", Target = "Cavity.Properties.Resources.resources", Justification = "This is the correct spelling.")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "csproj", Scope = "resource", Target = "Cavity.Properties.Resources.resources", Justification = "This is the correct spelling.")]