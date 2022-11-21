using Common.HotKey;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        #region 字段
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();
        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<BBHotKey, int> m_HotKeySettings = new Dictionary<BBHotKey, int>();
        #endregion

        #region 属性

        #endregion

        #region 方法
        public TipWindow()
        {
            InitializeComponent();
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Dispatcher.Invoke(() =>
                {
                    this.Topmost = true;
                    this.Activate();
                });
            });
        }
        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey(ObservableCollection<HotKeyEntity> hotKeyModelList = null)
        {
            var list = hotKeyModelList ?? Common.Common.LoadDefaultHotKey();
            // 注册全局快捷键
            string failList = HotKeyHelper.RegisterGlobalHotKey(list, new WindowInteropHelper(Application.Current.MainWindow).Handle, out m_HotKeySettings);
            if (string.IsNullOrEmpty(failList)) return true;

            MessageBoxResult mbResult = MessageBox.Show(string.Format("无法注册下列快捷键\n\r{0}是否要重新设置这些快捷键？", failList), "提示", MessageBoxButton.YesNo);

            if (mbResult == MessageBoxResult.Yes) return false;

            return true;
        }
        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            switch (msg)
            {
                case User.WM_HOTKEY:
                    int sid = wideParam.ToInt32();
                    if (sid == m_HotKeySettings[BBHotKey.显隐])
                        Common.Common.OnShowHide();
                    else if (sid == m_HotKeySettings[BBHotKey.减不透明度])
                        Common.Common.OnOpacitySub();
                    else if (sid == m_HotKeySettings[BBHotKey.加不透明度])
                        Common.Common.OnOpacityAdd();
                    else if (sid == m_HotKeySettings[BBHotKey.播放_暂停])
                        Common.Common.OnPlayPuse();
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }
        #endregion

        #region 事件
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Common.Common.RegisterGlobalHotKeyEvent += (e) => { return InitHotKey(e); };
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
            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc);
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //拖动窗体
            this.DragMove();
        }

        #endregion

    }
}
