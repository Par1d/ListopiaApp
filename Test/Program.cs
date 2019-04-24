using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListopiaApp;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ListopiaApp.REST.Service.AddShareInvite(new ListopiaApp.Models.ShareInvite { ListId = 1, InvitedUsername = "michaela.longbottom@gmail.com" });
        }
    }
}
