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
        #region Ollamaコントロール用オブジェクト
        /// <summary>
        /// Ollamaコントロール用オブジェクト
        /// </summary>
        IOllamaControllerModel _OllamaCtrl = new OllamaControllerModel();
        /// <summary>
        /// Ollamaコントロール用オブジェクト
        /// </summary>
        public IOllamaControllerModel OllamaCtrl
        {
            get
            {
                return _OllamaCtrl;
            }
            set
            {
                if (_OllamaCtrl == null || !_OllamaCtrl.Equals(value))
                {
                    _OllamaCtrl = value;
                    RaisePropertyChanged("OllamaCtrl");
                }
            }
        }
        #endregion

        #region WebUI用コントローラー
        /// <summary>
        /// WebUI用コントローラー
        /// </summary>
        IWebUIControllerModel _WebUICtrl = new WebUIControllerModel();
        /// <summary>
        /// WebUI用コントローラー
        /// </summary>
        public IWebUIControllerModel WebUICtrl
        {
            get
            {
                return _WebUICtrl;
            }
            set
            {
                if (_WebUICtrl == null || !_WebUICtrl.Equals(value))
                {
                    _WebUICtrl = value;
                    RaisePropertyChanged("WebUICtrl");
                }
            }
        }
        #endregion

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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ZenzaiManagerModel(IOllamaControllerModel ollamaCtrl, IWebUIControllerModel webUi)
        {
            this.OllamaCtrl = ollamaCtrl;
            this.WebUICtrl = webUi;
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

                return await OllamaCtrl.BaseChat(list, message);
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
                this.ChatHistory.Items.Add(new OllapiMessageEx() { Role = "user", Content = this.OllamaCtrl.FirstMessage });

                var tmp = await BaseChat(this.OllamaCtrl.FirstMessage);

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
                    this.ChatHistory.SelectedItem = this.ChatHistory.Items.Last();
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
                this.ChatHistory.SelectedItem.FilePath = await this.WebUICtrl.ExecutePrompt(this.ImagePrompt);
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
                var tmp = await BaseChat(this.OllamaCtrl.PromptMessage);

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

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            try
            {
                FirstChat();

                this.WebUICtrl.InitWebUI();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
