using Ollapi.api;
using Ollapi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zenzai.Common.Utilities;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;

namespace Zenzai.Models.Zenzai
{
    public class ZenzaiManagerModel : BindableBase
    {
        public string FirstMessage { get; private set; }
            = "あなたとできるチャットゲームのおススメを教えてください。\r\nTRPGのようなものを想定しています。\r\n以降、全て日本語でお願いします。\r\n適切な位置で改行をお願いします。";
        public string PromptMessage { get; private set; }
            = "雰囲気を感じさせる画像をStable diffusionで出したいと思います。\r\nStable Diffusion用のプロンプトを英語で教えてください。";

        public string Role { get; private set; } = "user";

        /// <summary>
        /// Ollapi用ホスト
        /// </summary>
        public string OllapiHost { get; private set; } = "localhost";

        public int OllapiPort { get; private set; } = 11434;

        public string OllapiModel { get; private set; } = "example";

        public string WebuiUri { get; private set; } = "http://127.0.0.1:7861";

        public string WebuiOutputDirectory { get; private set; } = @"C:\output";

        public string WebuiCurrentDirectory { get; private set; } = @"C:\Work\stable-diffusion-webui";

        #region 画像生成用プロンプト
        /// <summary>
        /// 画像生成用プロンプト
        /// </summary>
        string _ImagePrompt = string.Empty;
        /// <summary>
        /// 画像生成用プロンプト
        /// </summary>
        public string ImagePrompt
        {
            get
            {
                return _ImagePrompt;
            }
            private set
            {
                if (_ImagePrompt == null || !_ImagePrompt.Equals(value))
                {
                    _ImagePrompt = value;
                    RaisePropertyChanged("ImagePrompt");
                }
            }
        }
        #endregion

        #region チャット履歴
        /// <summary>
        /// チャット履歴
        /// </summary>
        ChatManagerModel _ChatHistory = new ChatManagerModel();
        /// <summary>
        /// チャット履歴
        /// </summary>
        public ChatManagerModel ChatHistory
        {
            get
            {
                return _ChatHistory;
            }
            set
            {
                if (_ChatHistory == null || !_ChatHistory.Equals(value))
                {
                    _ChatHistory = value;
                    RaisePropertyChanged("ChatHistory");
                }
            }
        }
        #endregion

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
        public ZenzaiManagerModel()
        {

        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="firstMsg">最初のメッセージ設定</param>
        /// <param name="promptMsg">プロンプト生成用メッセージ設定</param>
        /// <param name="ollapiHost">Ollama用ホスト設定</param>
        /// <param name="ollapiPort">Ollama用ポート</param>
        /// <param name="ollapiModel">Ollama用モデル</param>
        /// <param name="webuiUri">WebUI用URI</param>
        public ZenzaiManagerModel(string firstMsg, string promptMsg, string ollapiHost, int ollapiPort, string ollapiModel, string webuiUri)
        {
            this.FirstMessage = firstMsg;
            this.PromptMessage = promptMsg;
            this.OllapiHost = ollapiHost;
            this.OllapiPort = ollapiPort;
            this.OllapiModel = ollapiModel;
            this.WebuiUri = webuiUri;
        }
        #endregion

        #region 最初のメッセージ
        /// <summary>
        /// 最初のメッセージ
        /// </summary>
        private async Task<OllapiChatResponse> BaseChat(string message)
        {
            try
            {
                this.ChatHistory.Items.Add(
                    new OllapiMessageEx()
                    {
                        Role = this.Role,
                        Content = message
                    });

                // Ollapiの起動
                OllapiChatRequest ollapi = new OllapiChatRequest(this.OllapiHost, this.OllapiPort, this.OllapiModel);
                ollapi.Open();  // 接続
                var ret = await ollapi.Request(this.ChatHistory.ToOllapiMessage()); // リクエストの実行

                // メッセージの展開
                var tmp = JSONUtil.DeserializeFromText<OllapiChatResponse>(ret);

                ollapi.Close(); // 切断

                return tmp;
            }
            catch
            {
                return new OllapiChatResponse();
            }
        }
        #endregion

        #region 送信（ユーザー）メッセージ
        /// <summary>
        /// 送信（ユーザー）メッセージ
        /// </summary>
        string _UserMessage = string.Empty;
        /// <summary>
        /// 送信（ユーザー）メッセージ
        /// </summary>
        public string UserMessage
        {
            get
            {
                return _UserMessage;
            }
            private set
            {
                if (_UserMessage == null || !_UserMessage.Equals(value))
                {
                    _UserMessage = value;
                    RaisePropertyChanged("UserMessage");
                }
            }
        }
        #endregion

        #region 受信（システム）メッセージ
        /// <summary>
        /// 受信（システム）メッセージ
        /// </summary>
        string _SystemMessage = string.Empty;
        /// <summary>
        /// 受信（システム）メッセージ
        /// </summary>
        public string SystemMessage
        {
            get
            {
                return _SystemMessage;
            }
            private set
            {
                if (_SystemMessage == null || !_SystemMessage.Equals(value))
                {
                    _SystemMessage = value;
                    RaisePropertyChanged("SystemMessage");
                }
            }
        }
        #endregion

        #region 最初のチャット
        /// <summary>
        /// 最初のチャット
        /// </summary>
        private async void FirstChat()
        {
            try
            {
                var tmp = await BaseChat(FirstMessage);

                if (tmp.Message != null)
                {
                    this.SystemMessage = tmp.Message.Content;   // 受信メッセージの画面表示
                    this.ChatHistory.Items.Add(new OllapiMessageEx(tmp.Message));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region チャット
        /// <summary>
        /// チャット
        /// </summary>
        public async void Chat(string message)
        {
            try
            {
                this.UserMessage = message;
                var tmp = await BaseChat(message);

                if (tmp.Message != null)
                {
                    this.SystemMessage = tmp.Message.Content;   // 受信メッセージの画面表示
                    this.ChatHistory.Items.Add(new OllapiMessageEx(tmp.Message));
                }

                PromptChat();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region プロンプト生成用チャット
        /// <summary>
        /// プロンプト生成用チャット
        /// </summary>
        public async void PromptChat()
        {
            try
            {
                var tmp = await BaseChat(PromptMessage);

                if (tmp.Message != null)
                {
                    this.ImagePrompt = tmp.Message.Content;   // 画像生成用のプロンプトを取得
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region プロンプトの作成処理
        /// <summary>
        /// プロンプトの作成処理
        /// </summary>
        public async void ExecutePrompt()
        {
            var ret = await ExecutePromptSub();
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
        private async Task<bool> ExecutePromptSub()
        {
            try
            {
                string url = this.WebuiUri;
                string outdir = this.WebuiUri;
                List<string> path_list = new List<string>();
                bool ret = false;

                (ret, path_list) = await this.WebUI.Request.PostRequest(url, outdir, this.WebUI.Request.PromptItem);

                if (path_list.Count > 0 && this.ChatHistory.SelectedItem != null)
                {
                    ((OllapiMessageEx)this.ChatHistory.SelectedItem).FilePath = path_list.ElementAt(0);
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
                return false;
            }
        }
        #endregion

        #region WebUIの初期化処理
        /// <summary>
        /// WebUIの初期化処理
        /// </summary>
        private void InitWebUI()
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

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            try
            {
                FirstChat();

                InitWebUI();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
