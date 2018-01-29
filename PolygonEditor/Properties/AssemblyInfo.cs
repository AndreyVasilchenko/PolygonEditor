using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("\"PolygonEditor\"")]
[assembly: AssemblyDescription("  \"ПОСТРОЕНИЕ и РАЗБИЕНИЕ ПОЛИГОНОВ\"                               Языки: C# + XAML                        " +
                                " 1.Можно рисовать в окне приложения полиго- ны с произвольным количеством точек.               "+
	                            " 2.Можно отредактировать полигоны – доба- вить или удалить точки.                                           "+
	                            " 3.Полигоны могут строятся без самопересе- чений, приложение может отслеживать эту ситуацию.                                                                  "+
                                " 4.В приложении можно сделать разбиение одного полигона другим. Разбиение полигонов можно выделить, как отдельный набор поли- гонов.                                                                         " +
	                            " 5.Полигоны и разбиения можно вывести на печать.                                                                       "+
	                            " 6.Есть возможность сохранять и загружать исходные полигоны и разбиения в БазеДанных или в файле(по выбору пользователя).")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]//"for \"Software Technologies\"")]
[assembly: AssemblyProduct("\"PolygonEditor\"")]
[assembly: AssemblyCopyright("Copyright © 2018 Vasilchenko A.V.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
