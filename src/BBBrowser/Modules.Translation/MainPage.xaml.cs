using CefSharp;
using CefSharp.Wpf;
using CefSharp.Wpf.Internals;
using Common;
using Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using Win32;

namespace Modules.Translation
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        #region Fields

        SettingWindow settingWindow = null;
        #endregion

        #region Methods
        public MainPage()
        {

            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FloatingWindowTool\\config.txt";
            if (File.Exists(path))
                Common.Common.Appearance = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(path));
            else
                Common.Common.Appearance = new ConfigModel() { Height = 500, Width = 800, Opactiy = 1, Url = "https://www.baidu.com", HotKeys = Common.Common.LoadDefaultHotKey() };
            //https://cg.163.com/index.html#/search?key=%E5%86%B3%E6%96%97%E9%93%BE%E6%8E%A5

            CefSettings _settings = new CefSettings();
            if (Common.Common.Appearance.IsPhone)
                _settings.UserAgent = "tv.danmaku.bili/6250300 (Linux; U; Android 11; zh_CN; V1824A; Build/RP1A.200720.012; Cronet/81.0.4044.156)";
            _settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FloatingWindowTool\\Cache";
            Cef.Initialize(_settings);

            InitializeComponent();
            CommonHelper.ContentPanel = ContentPanel;
            //Browser.RequestContext.LoadExtensionFromDirectory("",null);
            //支持中文输入，但是ime不能定位到光标位置，但是可以在popup里面输入中文
            Browser.WpfKeyboardHandler = new WpfKeyboardHandler(Browser);
            //支持中文输入以及ime定位但是在popup里面失效，推测是popup没有实体句柄之类的
            //Browser.WpfKeyboardHandler = new WpfImeKeyboardHandler(Browser);
            Browser.LifeSpanHandler = new CefLifeSpanHandler(Browser);
            Browser.MenuHandler = new MenuHandler();
            //Browser.GetBrowserHost().SetAudioMuted(true);
            //Browser.RequestHandler = new MyRequestHandler();
            //Browser.LoadHandler = new MyLoadHandler();

            Browser.DataContext = menu.DataContext = Common.Common.Appearance;
            Application.Current.MainWindow.Top = Common.Common.Appearance.Top;
            Application.Current.MainWindow.Left = Common.Common.Appearance.Left;


            //每次注册热键的时候都保存到配置
            Common.Common.SaveConfig += () => { SaveConfig(); };
            //快捷键实现
            Common.Common.ShowHide += () => { togBro.IsChecked = !togBro.IsChecked; };
            Common.Common.OpacitySub += () => { Common.Common.Appearance.Opactiy -= 0.1; };
            Common.Common.OpacityAdd += () => { Common.Common.Appearance.Opactiy += 0.1; };

            //程序运行的时候预先注册一次热键
            settingWindow = new SettingWindow();
            settingWindow.Owner = Application.Current.MainWindow;
            var result = Common.Common.OnRegisterGlobalHotKey(Common.Common.Appearance.HotKeys);
            if (!result) settingWindow.ShowDialog();

            //设置是否显示任务栏
            Application.Current.MainWindow.ShowInTaskbar = Common.Common.Appearance.IsShowTaskBar;
        }


        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            Common.Common.Appearance.Top = (int)Application.Current.MainWindow.Top;
            Common.Common.Appearance.Left = (int)Application.Current.MainWindow.Left;
            if (Browser.Address != null)
                Common.Common.Appearance.Url = Browser.Address;
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FloatingWindowTool"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FloatingWindowTool");
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FloatingWindowTool\\config.txt", JsonConvert.SerializeObject(Common.Common.Appearance));
        }
        #endregion

        #region Events
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            Application.Current.Shutdown();
        }
        /// <summary>
        /// 跳转项目地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = (sender as Hyperlink).NavigateUri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        /// <summary>
        /// 鼠标离开的时候隐藏popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popBro_MouseLeave(object sender, MouseEventArgs e)
        {
            if (cheHide.IsChecked.Value == true)
            {
                popBro.SetCurrentValue(Grid.VisibilityProperty, Visibility.Collapsed);
            }
        }
        /// <summary>
        /// 滚动防缩页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browser_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;

            if (e.Delta > 0)
            {
                Browser.ZoomInCommand.Execute(null);
            }
            else if (e.Delta < 0)
            {
                Browser.ZoomOutCommand.Execute(null);
            }
            e.Handled = true;
        }
        /// <summary>
        /// 打开开发者工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Browser.ShowDevTools();
        }
        /// <summary>
        /// 页面刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemReflash_Click(object sender, RoutedEventArgs e)
        {
            Browser.Reload();
        }
        /// <summary>
        /// 页面前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemForward_Click(object sender, RoutedEventArgs e)
        {
            Browser.Forward();
        }

        /// <summary>
        /// 页面后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemBack_Click(object sender, RoutedEventArgs e)
        {
            Browser.Back();
        }

        /// <summary>
        /// 浏览方式按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowserMode_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();

            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "BBBrowser.exe");
            Application.Current.Shutdown();
        }
        /// <summary>
        /// 页面消失的时候 自动播放停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popBro_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Common.Common.Appearance.IsLeaveHidePlay)
            {
                var CommandID = (int)AppComandCode.MEDIA_PLAY_PAUSE << 16;
                IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
                User.SendMessage(hwnd, (uint)AppComandCode.WM_APPCOMMAND, hwnd, (IntPtr)CommandID);
            }
        }


        /// <summary>
        /// 收藏按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMark_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Browser.Address)) return;
            if (Common.Common.Appearance.BookmarkPages != null)
                if (Common.Common.Appearance.BookmarkPages.Where(u => u.Address.Equals(Browser.Address)).Any()) return;
            BookmarkWindow bookmarkWindow = new BookmarkWindow();
            bookmarkWindow.Owner = Application.Current.MainWindow;
            if (bookmarkWindow.ShowDialog().Value)
            {
                if (Common.Common.Appearance.BookmarkPages == null)
                    Common.Common.Appearance.BookmarkPages = new ObservableCollection<BookmarkPage>();
                Common.Common.Appearance.BookmarkPages.Add(new BookmarkPage() { Name = bookmarkWindow.txtName.Text, Address = Browser.Address });
                SaveConfig();
            }
        }
        /// <summary>
        /// 书签按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBookmark_Click(object sender, RoutedEventArgs e)
        {
            Browser.Address = ((sender as Button).DataContext as BookmarkPage).Address;
        }
        /// <summary>
        /// 标签删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBookmarkDelete_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as Button).DataContext as BookmarkPage;
            Common.Common.Appearance.BookmarkPages.Remove(data);
            SaveConfig();
        }
        /// <summary>
        /// 右键菜单打开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_Opened(object sender, RoutedEventArgs e)
        {
            txtAddress.Text = Browser.Address;
        }

        /// <summary>
        /// 选择项改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            switch (Common.Common.Appearance.Scale)
            {
                case 0:
                    break;
                case 1:
                    Common.Common.Appearance.Height = Common.Common.Appearance.Width / 2.4 * 1;
                    break;
                case 2:
                    Common.Common.Appearance.Height = Common.Common.Appearance.Width / 16 * 9;
                    break;
                case 3:
                    Common.Common.Appearance.Height = Common.Common.Appearance.Width / 21 * 9;
                    break;
            }
            SaveConfig();

        }

        /// <summary>
        /// 聚焦后自动全选地址栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_GotFocus(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(100);
                Dispatcher.Invoke(() =>
                {
                    txtAddress.SelectAll();
                });
            });
        }
        /// <summary>
        /// 页面加载时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (Common.Common.Appearance.IsGray)
                Browser.GetMainFrame().EvaluateScriptAsync("var bodys = document.getElementsByTagName(\"html\")[0];  bodys.style.WebkitFilter = \"grayscale(100%) \";");
        }

        /// <summary>
        /// 黑白模式点击时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrayToggleButton_Click_1(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            Browser.Reload();
        }

        /// <summary>
        /// 缩小页面放缩事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Browser.ZoomLevel -= 1;
        }

        /// <summary>
        /// 页面放缩还原事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNormal_Click(object sender, RoutedEventArgs e)
        {
            Browser.ZoomLevel = 0;
        }

        /// <summary>
        /// 放大页面放缩事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Browser.ZoomLevel += 1;
        }

        /// <summary>
        /// 关于按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("软件名称：BB浏览器\r\n" +
                "软件版本：v1.0\r\n" +
                "开发者：穆斯穆斯理", "关于");
        }

        /// <summary>
        /// 设置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            settingWindow = new SettingWindow();
            settingWindow.Owner = Application.Current.MainWindow;
            settingWindow.ShowDialog();
        }
        
        /// <summary>
        /// 常驻显示按钮按下时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void togBro_Click(object sender, RoutedEventArgs e)
        {
            //给予界面控件焦点，以解决CEFBrowser中文输入不上的问题
            if (togBro.IsChecked == true)
                txtdd.Focus();
        }
        #endregion

        #region Commands

        /// <summary>
        /// 浏览命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddress_Click(object sender, RoutedEventArgs e)
        {
            Browser.Address = txtAddress.Text;
        }

        /// <summary>
        /// 浏览命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowserCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Browser.Address = txtAddress.Text;
            menu.IsOpen = false;
        }

        /// <summary>
        /// 高度滑块移动时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void verSli_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            switch (Common.Common.Appearance.Scale)
            {
                case 0:
                    break;
                case 1:
                    Common.Common.Appearance.Width = Common.Common.Appearance.Height / 1 * 2.4;
                    break;
                case 2:
                    Common.Common.Appearance.Width = Common.Common.Appearance.Height / 9 * 16;
                    break;
                case 3:
                    Common.Common.Appearance.Width = Common.Common.Appearance.Height / 9 * 21;
                    break;
            }
            SaveConfig();
        }
        /// <summary>
        /// 水平滑块移动时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hoSli_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            switch (Common.Common.Appearance.Scale)
            {
                case 0:
                    break;
                case 1:
                    Common.Common.Appearance.Height = Common.Common.Appearance.Width / 2.4 * 1;
                    break;
                case 2:
                    Common.Common.Appearance.Height = Common.Common.Appearance.Width / 16 * 9;
                    break;
                case 3:
                    Common.Common.Appearance.Height = Common.Common.Appearance.Width / 21 * 9;
                    break;
            }
            SaveConfig();
        }

        #endregion

    }

    /// <summary>
    /// 扩展
    /// </summary>
    public class MyExtHandler : IExtensionHandler
    {
        public bool CanAccessBrowser(IExtension extension, IBrowser browser, bool includeIncognito, IBrowser targetBrowser)
        {
            return true;
        }

        public void Dispose()
        {

        }

        public IBrowser GetActiveBrowser(IExtension extension, IBrowser browser, bool includeIncognito)
        {
            return browser;
        }

        public bool GetExtensionResource(IExtension extension, IBrowser browser, string file, IGetExtensionResourceCallback callback)
        {
            return true;
        }

        public bool OnBeforeBackgroundBrowser(IExtension extension, string url, IBrowserSettings settings)
        {
            return true;
        }

        public bool OnBeforeBrowser(IExtension extension, IBrowser browser, IBrowser activeBrowser, int index, string url, bool active, IWindowInfo windowInfo, IBrowserSettings settings)
        {
            return true;
        }

        public void OnExtensionLoaded(IExtension extension)
        {

        }

        public void OnExtensionLoadFailed(CefErrorCode errorCode)
        {

        }

        public void OnExtensionUnloaded(IExtension extension)
        {

        }
    }
    /// <summary>
    /// 页面生命周期
    /// </summary>
    public class CefLifeSpanHandler : CefSharp.ILifeSpanHandler
    {
        ChromiumWebBrowser chromium;
        public CefLifeSpanHandler(ChromiumWebBrowser owner)
        {
            chromium = owner;
        }
        public bool DoClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
        {
            if (browser.IsDisposed || browser.IsPopup)
            {
                return false;
            }
            return true;
        }
        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            var dd = (ChromiumWebBrowser)browserControl;
            dd.Dispatcher.Invoke(() =>
            {
                var bb = dd.Address;
            });
        }
        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
        }
        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {


            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            if (!targetUrl.Equals("about:blank#blocked"))
            {
                chromiumWebBrowser.Load(targetUrl);
                newBrowser = null;
                return true;

            }
            else
            {

                ChromiumWebBrowser popupChromiumWebBrowser = null;
                chromiumWebBrowser.Dispatcher.Invoke(() =>
                {
                    var owner = Window.GetWindow(chromiumWebBrowser);
                    popupChromiumWebBrowser = new ChromiumWebBrowser();
                    popupChromiumWebBrowser.SetAsPopup();
                    popupChromiumWebBrowser.LifeSpanHandler = this;
                    popupChromiumWebBrowser.Opacity = 1;
                    CommonHelper.ContentPanel.Children.Clear();
                    CommonHelper.ContentPanel.Children.Add(popupChromiumWebBrowser);


                    var windowInteropHelper = new WindowInteropHelper(Application.Current.MainWindow);
                    var handle = windowInteropHelper.EnsureHandle();
                    windowInfo.SetAsWindowless(handle);

                });

                newBrowser = popupChromiumWebBrowser;

                return false;
            }

        }
    }
    /// <summary>
    /// 右键菜单
    /// </summary>
    public class MenuHandler : IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
        }
        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }
        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
        }
        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            //var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            //chromiumWebBrowser.Dispatcher.Invoke(() =>
            //{
            //    //显示菜单
            //    chromiumWebBrowser.ContextMenu.IsOpen = true;

            //});

            return true;
        }
    }
}
