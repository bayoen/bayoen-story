using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace bayoen.story.Windows
{
    public class BaseWindow : MetroWindow
    {
        public BaseWindow()
        {
            this.TitleCharacterCasing = CharacterCasing.Normal;
            this.ShowIconOnTitleBar = false;
            this.MinHeight = this.MinWidth = 400;
            this.BorderThickness = new Thickness(1);
            this.WindowTitleBrush = Brushes.Transparent;
            this.MouseLeftButtonDown += BaseWindow_MouseLeftButtonDown;
            this.Closing += BaseWindow_Closing;
        }

        private void BaseWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void BaseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
