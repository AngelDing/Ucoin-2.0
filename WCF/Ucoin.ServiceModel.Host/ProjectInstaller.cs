using System.ComponentModel;
using System.Configuration.Install;

namespace Ucoin.Service.Host
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
