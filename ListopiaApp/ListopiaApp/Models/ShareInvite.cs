using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ListopiaApp.Models
{
    public class ShareInvite
    {
        public ICommand AcceptCommand { get; set; }
        public ICommand DeclineCommand { get; set; }

        public ShareInvite()
        {
            AcceptCommand = new Command(async () => await REST.Service.AcceptInvite(this));
            DeclineCommand = new Command(async () => await REST.Service.DeclineInvite(this));
        }

        public int Id { get; set; }
        public string InviterUsername { get; set; }
        public string InvitedUsername { get; set; }
        public int ListId { get; set; }
        public string ListName { get; set; }
        public bool Accepted { get; set; }
    }
}
