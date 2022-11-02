using Common.HotKey;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Common
    {
        public static ConfigModel Appearance = null;


        /// <summary>
        /// 加载默认快捷键
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<HotKeyEntity> LoadDefaultHotKey()
        {
            var hotKeyList = new ObservableCollection<HotKeyEntity>();
            hotKeyList.Add(new HotKeyEntity { Name = BBHotKey.显隐.ToString(), IsUsable = true, IsSelectCtrl = false, IsSelectAlt = true, IsSelectShift = false, SelectKey = KeyCode.C });
            hotKeyList.Add(new HotKeyEntity { Name = BBHotKey.加不透明度.ToString(), IsUsable = true, IsSelectCtrl = false, IsSelectAlt = true, IsSelectShift = false, SelectKey = KeyCode.Up });
            hotKeyList.Add(new HotKeyEntity { Name = BBHotKey.减不透明度.ToString(), IsUsable = true, IsSelectCtrl = false, IsSelectAlt = true, IsSelectShift = false, SelectKey = KeyCode.Down });
            return hotKeyList;
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public static event Action SaveConfig;
        public static void OnSaveConfig() => SaveConfig?.Invoke();


        /// <summary>
        /// 注册全局热键
        /// </summary>
        public static event Func<ObservableCollection<HotKeyEntity>, bool> RegisterGlobalHotKeyEvent;
        public static bool OnRegisterGlobalHotKey(ObservableCollection<HotKeyEntity> hotKeyModelList)
        {
            if (RegisterGlobalHotKeyEvent != null)
            {
                if (hotKeyModelList == null)
                    hotKeyModelList = LoadDefaultHotKey();
                //每次设置热键前保存
                OnSaveConfig();
                return RegisterGlobalHotKeyEvent(hotKeyModelList);
            }
            return false;
        }

        public static event Action ShowHide;
        public static void OnShowHide()
        {
            ShowHide?.Invoke();
        }

        public static event Action OpacitySub;
        public static void OnOpacitySub()
        {
            OpacitySub?.Invoke();
        }

        public static event Action OpacityAdd;
        public static void OnOpacityAdd()
        {
            OpacityAdd?.Invoke();
        }
    }
}
