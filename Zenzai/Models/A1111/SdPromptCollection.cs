using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public class SdPromptCollection : BindableBase
    {
        #region StableDiffusion用プロンプトリスト
        /// <summary>
        /// StableDiffusion用プロンプトリスト
        /// </summary>
        ObservableCollection<SdPrompt> _Items = new ObservableCollection<SdPrompt>();
        /// <summary>
        /// StableDiffusion用プロンプトリスト
        /// </summary>
        public ObservableCollection<SdPrompt> Items
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

        #region 選択アイテム
        /// <summary>
        /// 選択アイテム
        /// </summary>
        SdPrompt _SelectedItem = new SdPrompt();
        /// <summary>
        /// 選択アイテム
        /// </summary>
        public SdPrompt SelectedItem
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
