using System;

namespace Local.Http.Email.Server.Common
{
    public static class ConsoleOutPut
    {
        public static void ShowOnConsole(this string url, string server)
        {
            Console.WriteLine($"**************************************************************************************");
            Console.WriteLine($"***                                                                                ***");
            Console.WriteLine($"                   {server} Server running on : {url}...                              ");
            Console.WriteLine($"***                                                                                ***");
            Console.WriteLine($"**************************************************************************************");
        }
    }
}