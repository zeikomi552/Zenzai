using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public class SdPrompt : BindableBase
    {
        #region プロンプト名
        /// <summary>
        /// プロンプト名
        /// </summary>
        string _Name = string.Empty;
        /// <summary>
        /// プロンプト名
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == null || !_Name.Equals(value))
                {
                    _Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        #endregion

        #region ネガティブプロンプト
        /// <summary>
        /// ネガティブプロンプト
        /// </summary>
        string _Prompt = "";
        /// <summary>
        /// ネガティブプロンプト
        /// </summary>
        public string Prompt
        {
            get
            {
                return _Prompt;
            }
            set
            {
                if (_Prompt == null || !_Prompt.Equals(value))
                {
                    _Prompt = value;
                    RaisePropertyChanged("Prompt");
                }
            }
        }
        #endregion
        #region ネガティブプロンプト
        /// <summary>
        /// ネガティブプロンプト
        /// </summary>
        string _NegativePrompt = "EasyNegative";
        /// <summary>
        /// ネガティブプロンプト
        /// </summary>
        public string NegativePrompt
        {
            get
            {
                return _NegativePrompt;
            }
            set
            {
                if (_NegativePrompt == null || !_NegativePrompt.Equals(value))
                {
                    _NegativePrompt = value;
                    RaisePropertyChanged("NegativePrompt");
                }
            }
        }
        #endregion
    }
}
