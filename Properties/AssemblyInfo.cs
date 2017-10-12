using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("Jiaparts.LocalStorage")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("09f0aa01-3431-409f-9211-72dfb25883ba")]


[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("北京嘉配科技")]
[assembly: AssemblyProduct("PAP系统")]
[assembly: AssemblyCopyright("版权所有 (C) 2016-2018 北京嘉配科技")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyDescription("警告:本软件受著作权法和国际公约的保护,未经授权擅自复制或传播本软件的部分或全部,可能受到严厉的民事或则刑事制裁,并在法律的允许范围内受到最大的可能起诉.")]


// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(RevisionClass.FullVersion)]
[assembly: AssemblyFileVersion(RevisionClass.FullVersion)]

internal static class RevisionClass
{
    public const string Major = "1";
    public const string Minor = "0";
    public const string Build = "0";


    public const string MainVersion = Major + "." + Minor;
    public const string FullVersion = Major + "." + Minor + "." + Build + ".16";
}
