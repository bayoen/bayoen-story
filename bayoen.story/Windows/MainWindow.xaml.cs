using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace bayoen.story.Windows
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.Title += $" - v{Config.ProjectAssemply.Version}";

            this.TitlebarHeight = 0;
            this.TopGrid.Margin = new Thickness(0, 30, 0, 0);
        }

        private void MainWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            this.TitlebarHeight = 30;
            this.TopGrid.Margin = new Thickness(0, 0, 0, 0);
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            this.TitlebarHeight = 0;            
            this.TopGrid.Margin = new Thickness(0, 30, 0, 0);
        }
    }
}
