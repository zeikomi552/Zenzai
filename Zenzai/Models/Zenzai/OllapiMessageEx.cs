﻿using Ollapi.api;
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

        public OllapiMessageEx(OllapiMessage message, string personaName )
        {
            this.Role = message.Role;
            this.Content = message.Content;
            this.Images = message.Images;
            this.PersonaName = personaName;
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
        #region プロンプト
        /// <summary>
        /// プロンプト
        /// </summary>
        string _Prompt = string.Empty;
        /// <summary>
        /// プロンプト
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
        string _NegativePrompt = string.Empty;
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

        #region 作成時間
        /// <summary>
        /// 作成時間
        /// </summary>
        DateTime? _CreatedAt = null;
        /// <summary>
        /// 作成時間
        /// </summary>
        public DateTime? CreatedAt
        {
            get
            {
                return _CreatedAt;
            }
            set
            {
                if (_CreatedAt == null || !_CreatedAt.Equals(value))
                {
                    _CreatedAt = value;
                    RaisePropertyChanged("CreatedAt");
                }
            }
        }
        #endregion

        #region ペルソナ名
        /// <summary>
        /// ペルソナ名
        /// </summary>
        string _PersonaName = string.Empty;
        /// <summary>
        /// ペルソナ名
        /// </summary>
        public string PersonaName
        {
            get
            {
                return _PersonaName;
            }
            set
            {
                if (_PersonaName == null || !_PersonaName.Equals(value))
                {
                    _PersonaName = value;
                    RaisePropertyChanged("PersonaName");
                }
            }
        }
        #endregion



        #region コピー処理
        /// <summary>
        /// コピー処理
        /// </summary>
        /// <returns>コピーした値</returns>
        public OllapiMessageEx Copy()
        {
            return new OllapiMessageEx()
            {
                Content = this.Content,
                FilePath = this.FilePath,
                Images = this.Images,
                NegativePrompt = this.NegativePrompt,
                Role = this.Role,
                PersonaName = this.PersonaName,
                Prompt = this.Prompt,
                CreatedAt = this.CreatedAt,
            };
        }
        #endregion
    }
}
