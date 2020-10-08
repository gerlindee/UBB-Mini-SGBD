using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var Server = new Server();
            Server.StartServer();
        }
    }
}
