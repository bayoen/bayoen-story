using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using bayoen.Library.Utilities;
using bayoen.story.Windows;

namespace bayoen.story
{ 
    public static class Core
    {
        private static TrayIcon _trayIcon;
        public static TrayIcon TrayIcon => _trayIcon ?? (_trayIcon = new TrayIcon());

        private static MainWindow _mainWindow;
        public static MainWindow MainWindow => _mainWindow ?? (_mainWindow = new MainWindow());

        private static MainWorker _mainWorker;
        public static MainWorker MainWorker => _mainWorker ?? (_mainWorker = new MainWorker());

#if DEBUG
        private static DebugWindow _debugWindow;
        public static DebugWindow DebugWindow => _debugWindow ?? (_debugWindow = new DebugWindow());
#endif

        public static void Initiate()
        {
            Core.TrayIcon.NotifyIcon.Visible = true;
            Core.MainWindow.Show();
#if DEBUG
            Core.DebugWindow.Show();
#endif
            Core.MainWorker.Start();
        }

        public static void Show()
        {
            Core.MainWindow.Show();
            Core.MainWindow.Activate();
        }

#if DEBUG
        public static void ShowDebug()
        {
            Core.DebugWindow.Show();
            Core.DebugWindow.Activate();
        }
#endif

        public static void Start()
        {

        }

        public static void Reset()
        {
            Core.MainWorker.ResetAdvanture();
        }

        public static void Unlock()
        {
            Core.MainWorker.UnlockAdvanture();
        }

        public static void Save()
        {

        }

        public static void Terminate()
        {
            Core.TrayIcon.Terminate();
            Core.Save();
            Environment.Exit(0);
        }
    }
}
