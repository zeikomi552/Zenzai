using Ollapi.api;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Models.Zenzai;

namespace Zenzai.Models.Ollama
{
    public class ChatManagerModel : BindableBase
    {
        #region チャット履歴
        /// <summary>
        /// チャット履歴
        /// </summary>
        ObservableCollection<OllapiMessageEx> _Items = new ObservableCollection<OllapiMessageEx>();
        /// <summary>
        /// チャット履歴
        /// </summary>
        public ObservableCollection<OllapiMessageEx> Items
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
        OllapiMessageEx _SelectedItem = new OllapiMessageEx();
        /// <summary>
        /// 選択チャット
        /// </summary>
        public OllapiMessageEx SelectedItem
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

        #region List<OllapiMessage>に変換
        /// <summary>
        /// List<OllapiMessage>に変換
        /// </summary>
        /// <returns></returns>
        public List<IOllapiMessage> ToOllapiMessage()
        {
            return (from x in _Items
                    select new OllapiMessage()
                    {
                        Content = x.Content,
                        Role = x.Role,
                        Images = x.Images,
                    }).ToList<IOllapiMessage>();

        }
        #endregion
    }
}
