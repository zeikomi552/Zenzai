using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zenzai.Models.Zenzai
{
    public class PersonaCollection : BindableBase
    {
        #region ペルソナリスト
        /// <summary>
        /// ペルソナリスト
        /// </summary>
        ObservableCollection<Persona> _Items = new ObservableCollection<Persona>();
        /// <summary>
        /// ペルソナリスト
        /// </summary>
        public ObservableCollection<Persona> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                if (_Items == null || !_Items.Equals(value))
                {
                    _Items = value;
                    RaisePropertyChanged("Items");
                }
            }
        }
        #endregion

        #region 選択アイテム（ペルソナ）
        /// <summary>
        /// 選択アイテム（ペルソナ）
        /// </summary>
        Persona _SelectedItem = new Persona();
        /// <summary>
        /// 選択アイテム（ペルソナ）
        /// </summary>
        [XmlIgnore]
        public Persona SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem == null || !_SelectedItem.Equals(value))
                {
                    _SelectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                }
            }
        }
        #endregion


    }
}
