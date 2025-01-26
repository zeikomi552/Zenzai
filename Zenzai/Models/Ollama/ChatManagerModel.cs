using Ollapi.api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.Ollama
{
    public class ChatManagerModel : BindableBase
    {
        #region チャット履歴
        /// <summary>
        /// チャット履歴
        /// </summary>
        ObservableCollection<OllapiMessage> _Items = new ObservableCollection<OllapiMessage>();
        /// <summary>
        /// チャット履歴
        /// </summary>
        public ObservableCollection<OllapiMessage> Items
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

        #region 選択チャット
        /// <summary>
        /// 選択チャット
        /// </summary>
        OllapiMessage _SelectedItem = new OllapiMessage();
        /// <summary>
        /// 選択チャット
        /// </summary>
        public OllapiMessage SelectedItem
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
