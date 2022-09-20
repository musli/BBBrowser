using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Win32;

namespace BBBrowser
{
    /// <summary>
    /// Interaction logic for TipWindow.xaml
    /// </summary>
    public partial class TipWindow : Window
    {
        public TipWindow()
        {
            InitializeComponent();
            Task.Run(() => {
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => {
                    this.Topmost = true;
                    this.Activate();
                });
            });
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //屏蔽alert+f4
            if (e.Key == Key.System && e.SystemKey == Key.F4) e.Handled = true;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            //屏蔽alert+tab win+tab的任务视图，将本窗体的父级指定为桌面，这样任务视图就不会将本窗体列出来
            if (sender is System.Windows.Window window)
            {
                var tempInterop = new WindowInteropHelper(window);
                tempInterop.Owner = (IntPtr)User.GetDesktopWindow();
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //拖动窗体
            this.DragMove();
        }
    }
}
