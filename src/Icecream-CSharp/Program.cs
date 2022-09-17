namespace IcecreamLogTest;

using IcDebugger;

class Program
{
    static void Main(string[] args)
    {
        var a = 1;
        // var logger = new Logger();
        IceCream.Ic(a == 1);
        IceCream.Ic();
    }
}

