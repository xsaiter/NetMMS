using System.Windows;

namespace NetMMS {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            var shell = new ShellView();            
            shell.Show();
        }
    }
}
