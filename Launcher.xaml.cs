using System;
using System.Windows;
using System.Windows.Controls;

namespace NSSAG
{
    /// <summary>
    /// Launcher.xaml 的交互逻辑
    /// </summary>
    public partial class Launcher : Window
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            int width = 1280, height = 720;
            foreach (var i in ResolutionList.Children)
                if (i is RadioButton)
                {
                    RadioButton button = (RadioButton)i;
                    if (button.IsChecked == true)
                    {
                        int.TryParse(button.Content.ToString().Split('×')[0], out width);
                        int.TryParse(button.Content.ToString().Split('×')[1], out height);
                        break;
                    }
                }
            bool full = IsFullScreen.IsChecked == true;
            new DxManager(false, "zip", "NSSAG ver0.01 | with NSASM & NSGDX", width, height, full);
            this.Hide();

            NSSAGCore core = NSSAGCore.Instance;
            core.Setup();
            do
                core.Loop();
            while (!(core.IsExit | DxManager.Instance.ProcessInfo() == -1));
            core.Dispose();
        }
    }
}
