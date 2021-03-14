using System.Threading.Tasks;

namespace Local.Http.Email.Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await ServerManager.StartAsync();
        }
    }
}