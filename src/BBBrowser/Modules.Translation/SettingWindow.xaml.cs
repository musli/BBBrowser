using Common.HotKey;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Modules.Translation
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {

        #region 字段

        #endregion

        #region 属性
        //private ObservableCollection<HotKeyEntity> m_HotKeyList = new ObservableCollection<HotKeyEntity>();
        ///// <summary>
        ///// 快捷键设置项集合
        ///// </summary>
        //public ObservableCollection<HotKeyEntity> HotKeyList
        //{
        //    get { return m_HotKeyList; }
        //    set { m_HotKeyList = value; }
        //}
        #endregion

        #region 方法

        /// <summary>
        /// 初始化快捷键
        /// </summary>
        private void InitHotKey()
        {
            
            var list = Common.Common.LoadDefaultHotKey();
            list.ToList().ForEach(x => Common.Common.Appearance.HotKeys.Add(x));
        }
        public SettingWindow()
        {
            InitializeComponent();
            this.DataContext = Common.Common.Appearance;
        }
        #endregion

        #region 事件
        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            //InitHotKey();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.ShowInTaskbar = Common.Common.Appearance.IsShowTaskBar;
        }
        private void btnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            if (!Common.Common.OnRegisterGlobalHotKey(Common.Common.Appearance.HotKeys))return;

            this.Close();
        }
        #endregion

       
    }
   
   
}
