using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public class WebUIConfig : BindableBase, IWebUIConfig
    {

        #region WebUI用URI
        /// <summary>
        /// WebUI用URI
        /// </summary>
        string _WebuiUri = "http://127.0.0.1:7861";
        /// <summary>
        /// WebUI用URI
        /// </summary>
        public string WebuiUri
        {
            get
            {
                return _WebuiUri;
            }
            set
            {
                if (_WebuiUri == null || !_WebuiUri.Equals(value))
                {
                    _WebuiUri = value;
                    RaisePropertyChanged("WebuiUri");
                }
            }
        }
        #endregion

        #region WebUI用画像出力先ディレクトリ
        /// <summary>
        /// WebUI用画像出力先ディレクトリ
        /// </summary>
        string _WebuiOutputDirectory = @"C:\output";
        /// <summary>
        /// WebUI用画像出力先ディレクトリ
        /// </summary>
        public string WebuiOutputDirectory
        {
            get
            {
                return _WebuiOutputDirectory;
            }
            set
            {
                if (_WebuiOutputDirectory == null || !_WebuiOutputDirectory.Equals(value))
                {
                    _WebuiOutputDirectory = value;
                    RaisePropertyChanged("WebuiOutputDirectory");
                }
            }
        }
        #endregion

        #region WebUI用カレントディレクトリ
        /// <summary>
        /// WebUI用カレントディレクトリ
        /// </summary>
        string _WebuiCurrentDirectory = @"C:\Work\stable-diffusion-webui";
        /// <summary>
        /// WebUI用カレントディレクトリ
        /// </summary>
        public string WebuiCurrentDirectory
        {
            get
            {
                return _WebuiCurrentDirectory;
            }
            set
            {
                if (_WebuiCurrentDirectory == null || !_WebuiCurrentDirectory.Equals(value))
                {
                    _WebuiCurrentDirectory = value;
                    RaisePropertyChanged("WebuiCurrentDirectory");
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
