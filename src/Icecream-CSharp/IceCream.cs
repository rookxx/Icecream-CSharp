namespace IcDebugger;

using System;
using System.Runtime.CompilerServices;

public static class IceCream
{
    private record struct Configure
    {
        public string Prefix;
        public bool IncludeContext;
    }

    private static Configure configure = new()
    {
        Prefix = "Ic |",
        IncludeContext = false,
    };

    public static void Ic(object? arg = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0,
        [CallerArgumentExpression("arg")] string argExpressions = "")
    {
        Console.WriteLine(Format(arg, memberName, sourceFilePath, sourceLineNumber, argExpressions));
    }

    public static string Format(object? arg = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0,
        [CallerArgumentExpression("arg")] string argExpressions = "")
    {
        var context = configure.IncludeContext || arg is null ?
            $"{sourceFilePath} in {memberName} line {sourceLineNumber} {(arg is not null ? ":" : string.Empty)}" :
            string.Empty;

        var log = $"{configure.Prefix} {context}";
        log += arg != null ? $"{argExpressions}:{arg}" : string.Empty;

        return log;
    }

    public static void ConfigureOutput(
        bool includeContext = false)
    {
        configure = configure with { IncludeContext = includeContext };
    }
}
