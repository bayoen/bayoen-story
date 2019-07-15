using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using bayoen.Library.Utilities.ExtendedMethods;

namespace bayoen.story.Windows
{
    public class Clock : Grid
    {
        public Clock()
        {
            this.ClockPanelImage = new Image()
            {
                Height = 68,
                Width = 406,
                Margin = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            this.ClockPanelImage.SetBitmap(bayoen.story.Properties.Resources.Score2FitStrongPanel);
            this.Children.Add(this.ClockPanelImage);

            this.ClockMainTextBlock = new TextBlock()
            {
                Foreground = Brushes.White, //new SolidColorBrush(Color.FromRgb(0xFF, 0xEA, 0x00)), // #ffea00
                FontFamily = new FontFamily("Arial"),
                FontSize = 40,
                FontWeight = FontWeights.ExtraBold,
                //Text = "00 : 00 : 00.0",
                Margin = new Thickness(0, 0, 30, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            this.Children.Add(this.ClockMainTextBlock);

            this.ClockMilliTextBlock = new TextBlock()
            {
                Foreground = Brushes.White, //new SolidColorBrush(Color.FromRgb(0xFF, 0xEE, 0x00)), // #ffea00
                FontFamily = new FontFamily("Arial"),
                FontSize = 25,
                FontWeight = FontWeights.Bold,
                //Text = "00",
                Margin = new Thickness(285, 8, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            this.Children.Add(this.ClockMilliTextBlock);

            this._time = new TimeSpan(-1);
            this.Time = new TimeSpan(0);
        }

        private Image ClockPanelImage;
        private TextBlock ClockMainTextBlock;
        private TextBlock ClockMilliTextBlock;

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => this._time;
            set
            {
                if (this._time == value) return;

                this.SetTime(value);

                this._time = value;
            }
        }

        private void SetTime(TimeSpan time)
        {
            this.ClockMainTextBlock.Text = $"{(time.Days * 24 + time.Hours).ToString("D2")} : {time.Minutes.ToString("D2")} : {time.Seconds.ToString("D2")}.{(int)Math.Floor((double)time.Milliseconds / 100)}";
            this.ClockMilliTextBlock.Text = $"{(time.Milliseconds % 100).ToString("D2")}";
        }
    }
}
