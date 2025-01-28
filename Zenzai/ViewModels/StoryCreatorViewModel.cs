using Ollapi.api;
using Ollapi.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Zenzai.Common.Utilities;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;

namespace Zenzai.ViewModels
{
    public class StoryCreatorViewModel : BindableBase
    {
        #region コマンド用
        private DelegateCommand? _showDialogCommand;
        public DelegateCommand ShowDialogCommand =>
            _showDialogCommand ?? (_showDialogCommand = new DelegateCommand(ShowDialog));


        private DelegateCommand? _ChatCommand;
        public DelegateCommand ChatCommand =>
            _ChatCommand ?? (_ChatCommand = new DelegateCommand(Chat));

        #endregion


        private IDialogService _dialogService;

        public StoryCreatorViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

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

        #region 送信メッセージ
        /// <summary>
        /// 送信メッセージ
        /// </summary>
        string _SendMessage = string.Empty;
        /// <summary>
        /// 送信メッセージ
        /// </summary>
        public string SendMessage
        {
            get
            {
                return _SendMessage;
            }
            set
            {
                if (_SendMessage == null || !_SendMessage.Equals(value))
                {
                    _SendMessage = value;
                    RaisePropertyChanged("SendMessage");
                }
            }
        }
        #endregion

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

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            try
            {
                InitWebUI();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region ダイアログの表示
        /// <summary>
        /// ダイアログの表示
        /// </summary>
        private void ShowDialog()
        {
            var message = "This is a message that should be shown in the dialog.";
            //using the dialog service as-is
            _dialogService.ShowDialog("SettingDialog", new DialogParameters($"message={message}"), r =>
            {
                //if (r.Result == ButtonResult.None)
                //    Title = "Result is None";
                //else if (r.Result == ButtonResult.OK)
                //    Title = "Result is OK";
                //else if (r.Result == ButtonResult.Cancel)
                //    Title = "Result is Cancel";
                //else
                //    Title = "I Don't know what you did!?";
            });
        }
        #endregion

        #region チャット
        /// <summary>
        /// チャット
        /// </summary>
        public async void Chat()
        {
            try
            {
                this.ChatHistory.Items.Add(
                    new OllapiMessage()
                    {
                        Role = "user",
                        Content = this.SendMessage
                    }
                    );

                OllapiChatRequest test = new OllapiChatRequest("localhost", 11434, "example");
                test.Open();
                var ret = await test.Request(this.ChatHistory.Items.ToList());

                int retry = 0;
                while (retry < 10)
                {
                    try
                    {
                        var tmp = JSONUtil.DeserializeFromText<OllapiChatResponse>(ret);

                        if (tmp.Message != null)
                        {
                            this.ChatHistory.Items.Add(tmp.Message);
                            break;
                        }
                    }
                    catch
                    {
                    }
                    retry++;
                }

                test.Close();

                this.SendMessage = string.Empty;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                this.WebUI.ExecuteWebUI(@"C:\Work\stable-diffusion-webui");
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

        #region Promptの実行処理
        /// <summary>
        /// Promptの実行処理
        /// </summary>
        private async Task<bool> ExecutePromptSub()
        {
            try
            {
                string url = "http://127.0.0.1:7861";
                string outdir = @"C:\output";
                List<string> path_list = new List<string>();
                bool ret = false;

                (ret, path_list) = await this.WebUI.Request.PostRequest(url, outdir, this.WebUI.Request.PromptItem);


                if (path_list.Count > 0)
                {
                    this.FilePath = path_list.ElementAt(0);
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

        #region WebUIの終了処理
        /// <summary>
        /// WebUIの終了処理
        /// </summary>
        public void CloseWebUI()
        {
            this.WebUI.WebUIProcessEnd();
        }
        #endregion
    }
}
