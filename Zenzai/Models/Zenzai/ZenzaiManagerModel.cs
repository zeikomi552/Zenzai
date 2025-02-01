using Ollapi.api;
using Ollapi.Common;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Zenzai.Common.Utilities;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

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
                // メッセージ送信用リストの作成
                List<IOllapiMessage> list = new List<IOllapiMessage>();

                foreach (var element in this.ChatHistory.Items)
                {
                    list.Add(new OllapiMessage()
                    {
                        Content = element.Content,
                        Role = element.Role,
                        Images = element.Images,
                    });
                }

                list.Add(new OllapiMessage()
                {
                    Role = this.Role,
                    Content = message,
                });

                // Ollapiの起動
                OllapiChatRequest ollapi = new OllapiChatRequest(this.OllapiHost, this.OllapiPort, this.OllapiModel);

                // 接続
                ollapi.Open();

                // リクエストの実行
                var ret = await ollapi.Request(list);

                // メッセージの展開
                var tmp = JSONUtil.DeserializeFromText<OllapiChatResponse>(ret);

                // 切断
                ollapi.Close();

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
                this.ChatHistory.Items.Add(new OllapiMessageEx() { Role = "user", Content = FirstMessage });

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
                this.ChatHistory.Items.Add(new OllapiMessageEx() { Role = "user", Content = message });
                this.UserMessage = message;

                var tmp = await BaseChat(message);

                if (tmp.Message != null)
                {
                    this.SystemMessage = tmp.Message.Content;   // 受信メッセージの画面表示
                    this.ChatHistory.Items.Add(new OllapiMessageEx(tmp.Message));
                }

                // 画像生成の実行
                CreateImage();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region 画像生成の実行処理(WebUIを使用)
        /// <summary>
        /// 画像生成の実行処理(WebUIを使用)
        /// </summary>
        private async void CreateImage()
        {
            // プロンプト生成用チャットの実行
            var ret = await PromptChat();

            if (ret)
            {
                // 画像生成の実行
                await ExecutePrompt(this.ImagePrompt, "EasyNegative");
            }
        }
        #endregion

        #region プロンプト生成用チャット
        /// <summary>
        /// プロンプト生成用チャット
        /// </summary>
        public async Task<bool> PromptChat()
        {
            try
            {
                var tmp = await BaseChat(PromptMessage);

                if (tmp.Message != null)
                {
                    this.ImagePrompt = tmp.Message.Content;   // 画像生成用のプロンプトを取得
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
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
        private async Task<bool> ExecutePrompt(string prompt, string negativePrompt)
        {
            try
            {
                string url = this.WebuiUri;
                string outdir = this.WebuiOutputDirectory;
                this.WebUI.Request.PromptItem.Prompt = prompt;
                this.WebUI.Request.PromptItem.NegativePrompt = negativePrompt;

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
