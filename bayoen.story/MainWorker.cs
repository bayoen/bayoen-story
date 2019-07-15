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
        private ProcessMemory _memory;
        public ProcessMemory Memory => _memory ?? (_memory = new ProcessMemory(Config.PPTName));

        public IntPtr BaseAddress { private set; get; }

        public int Act => Memory.ReadByte(this.BaseAddress + 0x451C54);
        public int Stage => Memory.ReadByte(this.BaseAddress + 0x451C58);
        public int SelectedStage => Memory.ReadByte(this.BaseAddress + 0x598ED0);
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

        public MainWorker()
        {
            this.Interval = Config.MainTimeSpan;
            this.Tick += MainWorker_Tick;

            this.BaseAddress = this.Memory.CheckProcess() ? (IntPtr)this.Memory.getBaseAddress : IntPtr.Zero;
        }

        public void ResetAdvanture()
        {
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
        }

        public void UnlockAdvanture()
        {
            // Remove advanture records
            for (int actIndex = 0; actIndex < 10; actIndex++)
            {
                for (int stageIndex = 0; stageIndex < 10; stageIndex++)
                {
                    // Reset stars to 0x1 if it is invalid
                    int star = this.Memory.ReadByte(this.BaseAddress + 0x5992F8 + 8 * (10 * actIndex + stageIndex));
                    if (star == 0) this.Memory.WriteByte(this.BaseAddress + 0x5992F8 + 8 * (10 * actIndex + stageIndex), 1);
                }
            }

            // Set selectedStage at 10-10
            this.Memory.WriteByte(this.BaseAddress + 0x598ED0, 99);
        }

        private void MainWorker_Tick(object sender, EventArgs e)
        {
            if (this.Memory.CheckProcess())
            {
                if (this.BaseAddress == IntPtr.Zero) this.BaseAddress = (IntPtr)this.Memory.getBaseAddress;

#if DEBUG
                // TextOut1
                Core.DebugWindow.TextOut1.Text = "Overall stars along advanture\n\n";
                for (int actIndex = 0; actIndex < 10; actIndex++)
                {
                    Core.DebugWindow.TextOut1.Text += $"Act {actIndex + 1}:\n";
                    Core.DebugWindow.TextOut1.Text += "\t" + string.Join(", ", Enumerable.Range(1, 10).ToList().ConvertAll(x => this.Star(actIndex + 1, x))) + "\n\n";
                }

                // TextOut2
                Core.DebugWindow.TextOut2.Text = "Scope\n\n";
                Core.DebugWindow.TextOut2.Text += $"Act: {((this.Act == 255) ? "-" : this.Act.ToString())}\n";
                Core.DebugWindow.TextOut2.Text += $"Stage: {((this.Stage == 255) ? "-" : this.Stage.ToString())}\n";
                Core.DebugWindow.TextOut2.Text += $"Selected Stage Index: {this.SelectedStage}-index\n";

                // TextOut3

#endif        
            }
            else
            {
                if (this.BaseAddress != IntPtr.Zero) this.BaseAddress = IntPtr.Zero;
            }               
        }
    }
}
