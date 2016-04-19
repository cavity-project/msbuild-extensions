using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Cavity")]
[assembly: AssemblyProduct("https://github.com/cavity-project/msbuild-extensions")]
[assembly: AssemblyCopyright("Copyright © 2010 - 2016 Alan Dean")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-GB")]
[assembly: ComVisible(false)]

#if (DEBUG)

[assembly: AssemblyConfiguration("Debug Build")]

#else

[assembly: AssemblyConfiguration("Release Build")]

#endif