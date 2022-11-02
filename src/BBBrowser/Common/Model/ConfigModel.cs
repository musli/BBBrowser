using Common.HotKey;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    /// <summary>
    /// 配置实体
    /// </summary>
    public class ConfigModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string url;
        /// <summary>
        /// 页面地址
        /// </summary>
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Url"));
            }
        }


        private bool isPhone;
        /// <summary>
        /// 是否竖屏
        /// </summary>
        public bool IsPhone
        {
            get { return isPhone; }
            set
            {
                isPhone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPhone"));
            }
        }

        private bool isGray;
        /// <summary>
        /// 是否黑白模式
        /// </summary>
        public bool IsGray
        {
            get { return isGray; }
            set
            {
                isGray = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsGray"));
            }
        }

        private double width;
        /// <summary>
        /// 宽度
        /// </summary>
        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Width"));
            }
        }
        private double height;
        /// <summary>
        /// 高度
        /// </summary>
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Height"));
            }
        }

        private double opactiy;
        /// <summary>
        /// 不透明度
        /// </summary>
        public double Opactiy
        {
            get { return opactiy; }
            set
            {
                opactiy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Opactiy"));
            }
        }
        private int left;
        /// <summary>
        /// 位置X
        /// </summary>
        public int Left
        {
            get { return left; }
            set
            {
                left = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Opactiy"));
            }
        }

        private int top;
        /// <summary>
        /// 位置Y
        /// </summary>
        public int Top
        {
            get { return top; }
            set
            {
                top = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Top"));
            }
        }

        private int scale;
        /// <summary>
        /// 页面缩放百分比
        /// </summary>
        public int Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Scale"));
            }
        }


        private bool isLeaveHide;
        /// <summary>
        /// 是否离开隐藏
        /// </summary>
        public bool IsLeaveHide
        {
            get { return isLeaveHide; }
            set
            {
                isLeaveHide = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLeaveHide"));
            }
        }

        private bool isLeaveHidePlay;
        /// <summary>
        /// 隐藏模式下是否离开暂停播放
        /// </summary>
        public bool IsLeaveHidePlay
        {
            get { return isLeaveHidePlay; }
            set
            {
                isLeaveHidePlay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLeaveHidePlay"));
            }
        }

        private ObservableCollection<BookmarkPage> bookmarkPages;
        /// <summary>
        /// 书签
        /// </summary>
        public ObservableCollection<BookmarkPage> BookmarkPages
        {
            get { return bookmarkPages; }
            set
            {
                bookmarkPages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BookmarkPages"));
            }
        }

        private ObservableCollection<HotKeyEntity> hotKeys;
        /// <summary>
        /// 快捷键集合
        /// </summary>
        public ObservableCollection<HotKeyEntity> HotKeys
        {
            get { return hotKeys; }
            set
            {
                hotKeys = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HotKeys"));
            }
        }
    }
}
