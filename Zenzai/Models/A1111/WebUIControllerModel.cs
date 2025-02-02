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
    public class WebUIControllerModel : WebUIConfig, IWebUIControllerModel
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

        public void SetConfig(WebUIConfig config)
        {
            this.WebuiUri = config.WebuiUri;
            this.WebuiOutputDirectory = config.WebuiOutputDirectory;
            this.WebuiCurrentDirectory = config.WebuiCurrentDirectory;
            this.NegativePrompt = config.NegativePrompt;
        }
    }
}
