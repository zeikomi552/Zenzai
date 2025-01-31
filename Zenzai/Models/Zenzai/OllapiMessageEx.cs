using Ollapi.api;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zenzai.Models.Zenzai
{
    public class OllapiMessageEx : BindableBase, IOllapiMessage
    {

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("images")]
        public string? Images { get; set; }

        public OllapiMessageEx()
        {

        }

        public OllapiMessageEx(OllapiMessage message )
        {
            this.Role = message.Role;
            this.Content = message.Content;
            this.Images = message.Images;
        }

        #region ファイルパス[FilePath]プロパティ
        /// <summary>
        /// ファイルパス[FilePath]プロパティ用変数
        /// </summary>
        string _FilePath = string.Empty;
        /// <summary>
        /// ファイルパス[FilePath]プロパティ
        /// </summary>
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                if (_FilePath == null || !_FilePath.Equals(value))
                {
                    _FilePath = value;
                    RaisePropertyChanged("FilePath");
                }
            }
        }
        #endregion

    }
}
