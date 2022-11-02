using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    /// <summary>
    /// 书签
    /// </summary>
    public class BookmarkPage
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string address;

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }

    }
}
