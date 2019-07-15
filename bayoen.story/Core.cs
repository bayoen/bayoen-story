using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
            Core.MainWorker.Run();
        }

        public static void Show()
        {
            Core.MainWindow.Show();
            Core.MainWindow.Activate();
        }

        public static void SetChromaKey(Brush brush)
        {
            Core.MainWindow.Background = brush;
        }

#if DEBUG
        public static void ShowDebug()
        {
            Core.DebugWindow.Show();
            Core.DebugWindow.Activate();
        }
#endif

        public static void Reset()
        {            
            Core.MainWorker.ResetAdvanture();            
        }

        //public static void Unlock()
        //{
        //    Core.MainWorker.UnlockAdvanture();
        //}

        //public static void Save()
        //{

        //}

        public static void Terminate()
        {
            Core.TrayIcon.Terminate();
            //Core.Save();
            Environment.Exit(0);
        }

        private static StoryStatus _status;
        public static StoryStatus Status
        {
            get => _status;
            set
            {
                if (_status == value) return;

                string status = "";

                switch (value)
                {
                    case StoryStatus.Broken:
                        status = "Broken!: Try 'Reset' in title";
                        break;
                    case StoryStatus.Missing:
                        status = "Missing!: Puyo Puyo Tetris Off";
                        break;
                    case StoryStatus.Ready:
                        status = "Ready";
                        break;
                    case StoryStatus.Recording:
                        status = "Recording!";
                        break;
                    default:
                        throw new InvalidOperationException("Wrong status is placed");
                }

                Core.MainWindow.StatusBlock.Foreground = (value >= StoryStatus.Broken) ? Brushes.Yellow : Brushes.White;
                Core.MainWindow.StatusBlock.FontSize = (value >= StoryStatus.Broken) ? 24 : 32;
                Core.MainWindow.StatusBlock.Text = status;

                _status = value;
            }
        }
    }
}
