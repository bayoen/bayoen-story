using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using bayoen.Library.Utilities;

namespace bayoen.story
{
    public class MainWorker : DispatcherTimer
    {
        public MainWorker()
        {
            this.Interval = Config.MainTimeSpan;
            this.Tick += MainWorker_Tick;

            this.BaseAddress = this.Memory.CheckProcess() ? (IntPtr)this.Memory.getBaseAddress : IntPtr.Zero;
            this.ClockAnchor = DateTime.Now;
            this.IsActive = false;
        }

        private ProcessMemory _memory;
        public ProcessMemory Memory => _memory ?? (_memory = new ProcessMemory(Config.PPTName));

        public IntPtr BaseAddress { private set; get; }

        public DateTime ClockAnchor;
        public TimeSpan ClockBuffer;
        public bool IsActive;

        public int Act => Memory.ReadByte(this.BaseAddress + 0x451C54);
        public int Stage => Memory.ReadByte(this.BaseAddress + 0x451C58);
        public int SelectedStage => Memory.ReadByte(this.BaseAddress + 0x598ED0);

        internal void Run()
        {
            if (this.Memory.CheckProcess())
            {
                this.BaseAddress = (IntPtr)this.Memory.getBaseAddress;

                if ((Core.MainWorker.Act == 255 && Core.MainWorker.Stage == 255))
                {
                    Core.Status = StoryStatus.Ready;
                }
                else if ((Core.MainWorker.Act == 0 && Core.MainWorker.Stage == 0))
                {
                    Core.Status = StoryStatus.Ready;
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    Core.Status = StoryStatus.Broken;
                }
            }
            else
            {
                this.BaseAddress = IntPtr.Zero;
                Core.Status = StoryStatus.Missing;
            }

            this.Start();
        }

        public int Star(int act, int stage)
        {
            if (act < 1 || act > 10) return -1;
            if (stage < 1 || stage > 10) return -1;

            byte rawByte = Memory.ReadByte(this.BaseAddress + 0x5992F8 + 8 * (10 * (act - 1) + (stage - 1)));
            return (int)((rawByte - 1) % 8 / 2);
        }

        public int Star(int stage)
        {
            if (stage < 0 || stage > 99) return -1;

            byte rawByte = Memory.ReadByte(this.BaseAddress + 0x5992F8 + 8 * stage);
            return (int)((rawByte - 1) % 8 / 2);
        }

        public void RefreshDisplay()
        {
            this.ClockBuffer = DateTime.Now - this.ClockAnchor;
            Core.MainWindow.Clock.Time = this.ClockBuffer;
            Core.MainWindow.StageBlock.Text = $"{((this.Act == 255) ? "-" : this.Act.ToString())} - {((this.Stage == 255) ? "-" : this.Stage.ToString())}";
        }

        public void ResetAdvanture()
        {
            if (this.Act == 255 && this.Stage == 255)
            {
                // Pass
            }
            else if (Core.MainWorker.Act == 0 && Core.MainWorker.Stage == 0)
            {
                // Pass
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            // Remove advanture records
            for (int actIndex = 0; actIndex < 10; actIndex++)
            {
                for (int stageIndex = 0; stageIndex < 10; stageIndex++)
                {
                    // Remove stars to 0x0
                    this.Memory.WriteByte(this.BaseAddress + 0x5992F8 + 8 * (10 * actIndex + stageIndex), 0);

                    // Remove records
                    this.Memory.WriteInt32(this.BaseAddress + 0x5992FC + 8 * (10 * actIndex + stageIndex), 0);
                }
            }

            // Set selectedStage at 1-1
            this.Memory.WriteByte(this.BaseAddress + 0x598ED0, 0);

            // Reset
            Core.Status = StoryStatus.Ready;
            Core.MainWindow.Clock.Time = new TimeSpan(0);

            // Deactivate
            this.IsActive = false;
        }

        //public void UnlockAdvanture()
        //{
        //    // Remove advanture records
        //    for (int actIndex = 0; actIndex < 10; actIndex++)
        //    {
        //        for (int stageIndex = 0; stageIndex < 10; stageIndex++)
        //        {
        //            // Reset stars to 0x1 if it is invalid
        //            int star = this.Memory.ReadByte(this.BaseAddress + 0x5992F8 + 8 * (10 * actIndex + stageIndex));
        //            if (star == 0) this.Memory.WriteByte(this.BaseAddress + 0x5992F8 + 8 * (10 * actIndex + stageIndex), 1);
        //        }
        //    }

        //    // Set selectedStage at 10-10
        //    this.Memory.WriteByte(this.BaseAddress + 0x598ED0, 99);
        //}

        private void MainWorker_Tick(object sender, EventArgs e)
        {            
            if (this.Memory.CheckProcess())
            {
                if (Core.Status == StoryStatus.Missing)
                {
                    Core.Status = StoryStatus.Ready;
                }
            }
            else
            {
                if (Core.Status != StoryStatus.Missing)
                {
                    System.Media.SystemSounds.Beep.Play();
                    Core.Status = StoryStatus.Missing;
                }                
            }

            // Main transition
            if (Core.Status == StoryStatus.Ready)
            {
                if (this.IsActive)
                {
                    if (Core.Status != StoryStatus.Recording) Core.Status = StoryStatus.Recording;
                    this.RefreshDisplay();
                }
                else
                {
                    // Waiting                    
                }

                if (this.Act != 255 && this.Stage != 255)
                {
                    this.IsActive = true;
                    Core.Status = StoryStatus.Recording;
                    this.ClockAnchor = DateTime.Now;
                }
            }
            else if (Core.Status == StoryStatus.Recording)
            {
                this.RefreshDisplay();
            }
            else if (Core.Status == StoryStatus.Broken)
            {
                // Do nothing
            }
            else if (Core.Status == StoryStatus.Missing)
            {
                if (this.IsActive) this.RefreshDisplay();
                if (this.BaseAddress != IntPtr.Zero) this.BaseAddress = IntPtr.Zero;
            }

#if DEBUG
            // TextOut1
            Core.DebugWindow.TextOut1.Text = "- Overall stars along advanture\n";
            for (int actIndex = 0; actIndex < 10; actIndex++)
            {
                Core.DebugWindow.TextOut1.Text += $"Act {actIndex + 1}:\n";
                Core.DebugWindow.TextOut1.Text += "\t" + string.Join(", ", Enumerable.Range(1, 10).ToList().ConvertAll(x => this.Star(actIndex + 1, x))) + "\n\n";
            }

            // TextOut2
            Core.DebugWindow.TextOut2.Text = "- Dashboard\n";
            Core.DebugWindow.TextOut2.Text += $"Core.Status: {Core.Status.ToString()}\n";
            Core.DebugWindow.TextOut2.Text += $"MainWorker.IsActive: {this.IsActive}\n";

            Core.DebugWindow.TextOut2.Text += "\n";
            Core.DebugWindow.TextOut2.Text += "- Stopwatch\n";
            Core.DebugWindow.TextOut2.Text += $"MainWorker.ClockAnchor: {this.ClockAnchor}\n";
            Core.DebugWindow.TextOut2.Text += $"DateTime.Now: {DateTime.Now}\n";
            Core.DebugWindow.TextOut2.Text += $"MainWorker.ClockBuffer: {this.ClockBuffer}\n";

            // TextOut3
            Core.DebugWindow.TextOut3.Text = "- Scope\n";
            Core.DebugWindow.TextOut3.Text += $"MainWorker.Act: {((this.Act == 255) ? "-" : this.Act.ToString())}\n";
            Core.DebugWindow.TextOut3.Text += $"MainWorker.Stage: {((this.Stage == 255) ? "-" : this.Stage.ToString())}\n";
            Core.DebugWindow.TextOut3.Text += $"MainWorker.Selected Stage Index: {this.SelectedStage}-index\n";
#endif

        }
    }
}
