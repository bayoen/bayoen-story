using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bayoen.story
{
    public class TrayIcon
    {
        public TrayIcon()
        {
            this.NotifyIcon = new NotifyIcon
            {
                ContextMenu = new ContextMenu(),
                Icon = bayoen.story.Properties.Resources.longcarbIcon,
                Text = Config.ProjectAssemply.Name,
            };

            this.NotifyIcon.MouseDoubleClick += (sender, e) => Core.Show();
            this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem($"{Config.ProjectAssemply.Name} - v{Config.ProjectAssemply.Version}") { Enabled = false });
            this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("-"));

#if DEBUG
            MenuItem ShowMenuItem = new MenuItem("Show", (sender, e) => Core.Show());           
            ShowMenuItem.MenuItems.Add(new MenuItem("MainWindow", (sender, e) => Core.Show()));
            ShowMenuItem.MenuItems.Add(new MenuItem("DebugWindow", (sender, e) => Core.ShowDebug()));
            this.NotifyIcon.ContextMenu.MenuItems.Add(ShowMenuItem);
#else
            this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Show", (sender, e) => Core.Show()));
#endif
            //this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Settings", (sender, e) => Core.ShowSetting()));

            //            MenuItem AdvancedMenuItem = new MenuItem("Advanced");
            //            AdvancedMenuItem.MenuItems.Add(new MenuItem("Restart", (sender, e) => Core.Restart(RestartingModes.RestartOnly)));
            //            AdvancedMenuItem.MenuItems.Add(new MenuItem("Folder", (sender, e) => Core.Folder()));
            //#if DEBUG
            //            AdvancedMenuItem.MenuItems.Add(new MenuItem("Debug", (sender, e) => Core.ShowDebug()));
            //#endif
            //            this.NotifyIcon.ContextMenu.MenuItems.Add(AdvancedMenuItem);
            this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Reset", (sender, e) => Core.Reset()));
            this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Unlock", (sender, e) => Core.Unlock()));
            this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("-"));
            this.NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", (sender, e) => Core.Terminate()));

        }

        public NotifyIcon NotifyIcon { get; set; }

        public void Terminate()
        {
            this.NotifyIcon.Visible = false;
            this.NotifyIcon.Dispose();
        }
    }
}
