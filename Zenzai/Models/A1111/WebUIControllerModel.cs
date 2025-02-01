using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zenzai.Common.Utilities;
using Zenzai.Models.Zenzai;

namespace Zenzai.Models.A1111
{
    public class WebUIControllerModel : BindableBase
    {
        #region WebUIA1111用オブジェクト
        /// <summary>
        /// WebUIA1111用オブジェクト
        /// </summary>
        WebUIBaseModel _WebUI = new WebUIBaseModel();
        /// <summary>
        /// WebUIA1111用オブジェクト
        /// </summary>
        public WebUIBaseModel WebUI
        {
            get
            {
                return _WebUI;
            }
            set
            {
                if (_WebUI == null || !_WebUI.Equals(value))
                {
                    _WebUI = value;
                    RaisePropertyChanged("WebUI");
                }
            }
        }
        #endregion

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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WebUIControllerModel(string webuiUri, string outputDirectory, string curDirectory)
        {
            this.WebuiUri = webuiUri;
            this.WebuiOutputDirectory = outputDirectory;
            this.WebuiCurrentDirectory = curDirectory;
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WebUIControllerModel()
        {

        }
        #endregion

        #region WebUIの終了処理
        /// <summary>
        /// WebUIの終了処理
        /// </summary>
        public void CloseWebUI()
        {
            this.WebUI.WebUIProcessEnd();
        }
        #endregion

        #region Promptの実行処理
        /// <summary>
        /// Promptの実行処理
        /// </summary>
        public async Task<string> ExecutePrompt(string prompt)
        {
            try
            {
                string url = this.WebuiUri;
                string outdir = this.WebuiOutputDirectory;
                this.WebUI.Request.PromptItem.Prompt = prompt;
                this.WebUI.Request.PromptItem.NegativePrompt = this.NegativePrompt;

                List<string> path_list = new List<string>();
                bool ret = false;

                (ret, path_list) = await this.WebUI.Request.PostRequest(url, outdir, this.WebUI.Request.PromptItem);

                if (ret)
                {
                    return path_list.ElementAt(0);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
                return string.Empty;
            }
        }
        #endregion

        #region WebUIの初期化処理
        /// <summary>
        /// WebUIの初期化処理
        /// </summary>
        public void InitWebUI()
        {
            try
            {
                this.WebUI.ExecuteWebUI(this.WebuiCurrentDirectory);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
