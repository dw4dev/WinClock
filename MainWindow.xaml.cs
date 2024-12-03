using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WinClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private string lastDate;
        private const double BASE_WIDTH = 320;
        private const double TIME_FONT_SIZE = 68;
        private const double DATE_FONT_SIZE = 30;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;

            StateChanged += MainWindow_StateChanged;

            lastDate = DateTime.Now.ToString("yyyy/MM/dd");
            timer.Start();
            UpdateTimeDisplay();
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) {
                timer.Stop();
            }
            else {
                timer.Start();
                UpdateTimeDisplay();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateTimeDisplay();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
        }

        private void UpdateTimeDisplay()
        {
            var now = DateTime.Now;
            var currentDate = now.ToString("yyyy/MM/dd");

            if (currentDate != lastDate) {
                lastDate = currentDate;
            }

            DateText.Text = currentDate;
            TimeText.Text = now.ToString("HH:mm:ss");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFontSizes();
        }

        private void UpdateFontSizes()
        {
            double scale = this.Width / BASE_WIDTH;
            TimeText.FontSize = TIME_FONT_SIZE * scale;
            DateText.FontSize = DATE_FONT_SIZE * scale;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPinTop_Click(object sender, RoutedEventArgs e)
        {
            if (this.Topmost) {
                this.Topmost = false;
                btnPinTop.Background = System.Windows.Media.Brushes.Transparent;
            }
            else {
                this.Topmost = true;
                btnPinTop.Background = System.Windows.Media.Brushes.DarkSeaGreen;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ResizeThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double newWidth = Math.Max(MinWidth, Width + e.HorizontalChange);
            Width = newWidth;
        }
    }
}