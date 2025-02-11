using Microsoft.Win32;
using Ollapi.api;
using Ollapi.Common;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.IO;
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
                    if (!element.Equals(this.ChatHistory.SelectedItem))
                    {
                        list.Add(new OllapiMessage()
                        {
                            Content = element.Content,
                            Role = element.Role,
                            Images = element.Images,
                        });
                    }
                    else
                    {
                        break;
                    }
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
        public async void CreateImage()
        {
            // プロンプト生成用チャットの実行
            var ret = await PromptChat();

            if (ret)
            {
                string prompt = this.ImagePrompt;
                if (this.ImagePrompt.Contains("\""))
                {
                    int bfIdx = this.ImagePrompt.IndexOf("\"");
                    int index = this.ImagePrompt.IndexOf("\"", bfIdx + 1);

                    if (index > bfIdx + 1)
                    {
                        prompt = prompt.Substring(bfIdx + 1, index - bfIdx - 1);
                    }
                    else
                    {
                        prompt = prompt.Substring(bfIdx + 1);
                    }
                }

                prompt = this.WebUICtrl.Prompt + "," + prompt;

                // 画像生成の実行
                this.ChatHistory.SelectedItem.FilePath = await this.WebUICtrl.ExecutePrompt(prompt);
                this.ChatHistory.SelectedItem.Prompt = prompt;
                this.ChatHistory.SelectedItem.NegativePrompt = this.WebUICtrl.NegativePrompt;
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
                //FirstChat();

                this.WebUICtrl.InitWebUI();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region ゲームファイルの保存処理
        /// <summary>
        /// ゲームファイルの保存処理
        /// </summary>
        public void Save()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "セーブファイル (*.znzi)|*.znzi";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    string tmpdir = Path.GetTempPath();


                    string path = PathManager.GetApplicationFolder();
                    string zipbaseDir = Path.Combine(tmpdir, "ZenzaiTemporary", "SaveTemporary");
                    string imageDir = Path.Combine(zipbaseDir, "Image");

                    try
                    {
                        // ディレクトリが存在する場合
                        if (Directory.Exists(zipbaseDir))
                        {
                            // 一時フォルダを削除
                            Directory.Delete(zipbaseDir, true);
                        }
                    }
                    catch { }

                    PathManager.CreateDirectory(zipbaseDir);    // 一時フォルダの作成
                    PathManager.CreateDirectory(imageDir);      // イメージフォルダの作成

                    // Imageファイルのコピー
                    foreach (var chatitem in this.ChatHistory.Items)
                    {
                        // ファイルの存在チェック
                        if (!string.IsNullOrEmpty(chatitem.FilePath) && File.Exists(chatitem.FilePath))
                        {
                            // ファイルのコピー
                            File.Copy(chatitem.FilePath, Path.Combine(imageDir, System.IO.Path.GetFileName(chatitem.FilePath)));
                        }
                    }

                    // ストーリーデータの保存
                    ChatManagerModel savedata = new ChatManagerModel()
                    {
                        Items = new System.Collections.ObjectModel.ObservableCollection<OllapiMessageEx>(CreateSaveChatHistory())
                    };
                    var filename = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
                    XMLUtil.Seialize(Path.Combine(zipbaseDir, "story.conf"), savedata);

                    // すでにファイルが存在する場合は削除
                    if (File.Exists(dialog.FileName))
                    {
                        File.Delete(dialog.FileName);
                    }

                    //ZIP書庫を作成
                    System.IO.Compression.ZipFile.CreateFromDirectory(
                       zipbaseDir,
                        dialog.FileName,
                        System.IO.Compression.CompressionLevel.Optimal,
                        false,
                        System.Text.Encoding.UTF8);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region ファイルのロード処理
        /// <summary>
        /// ファイルのロード処理
        /// </summary>
        public void Load()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "テキストファイル (*.znzi)|*.znzi";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    string tmpdir = Path.GetTempPath();
                    string path = PathManager.GetApplicationFolder();
                    string zipbaseDir = Path.Combine(tmpdir, "ZenzaiTemporary", "LoadTemporary");
                    string imageDir = Path.Combine(zipbaseDir, "Image");

                    try
                    {
                        // ディレクトリが存在する場合
                        if (Directory.Exists(zipbaseDir))
                        {
                            // 一時フォルダを削除
                            Directory.Delete(zipbaseDir, true);
                        }
                    }
                    catch { }

                    //ZIP書庫を展開する
                    System.IO.Compression.ZipFile.ExtractToDirectory(
                        dialog.FileName,
                        zipbaseDir);

                    this.ChatHistory = XMLUtil.Deserialize<ChatManagerModel>(Path.Combine(zipbaseDir, "story.conf"));

                    foreach (var chatitem in this.ChatHistory.Items)
                    {
                        // ファイル名がない場合は飛ばす
                        if (string.IsNullOrEmpty(chatitem.FilePath))
                        {
                            continue;
                        }

                        chatitem.FilePath = Path.Combine(this.WebUICtrl.WebuiOutputDirectory, chatitem.FilePath);

                        if (File.Exists(chatitem.FilePath))
                        {
                            File.Delete(chatitem.FilePath);
                        }

                        // ファイルのコピー
                        File.Copy(Path.Combine(imageDir, System.IO.Path.GetFileName(chatitem.FilePath)), chatitem.FilePath);
                    }
                    this.ChatHistory.SelectedItem = this.ChatHistory.Items.Last();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private List<OllapiMessageEx> CreateSaveChatHistory()
        {
            List<OllapiMessageEx> list = new List<OllapiMessageEx>();
            foreach (var item in this.ChatHistory.Items)
            {
                var copy = item.Copy();
                copy.FilePath = copy.FilePath.Replace(this.WebUICtrl.WebuiOutputDirectory + "\\", "");
                list.Add(copy);
            }
            return list;
        }

        /// <summary>
        /// メッセージのリフレッシュ
        /// </summary>
        public void RefreshMessage()
        {
            var sel = this.ChatHistory.SelectedItem;
            if (sel != null)
            {
                int index = this.ChatHistory.Items.IndexOf(sel);

                if (sel.Role.Equals("user"))
                {
                    this.UserMessage = sel.Content;

                    if (this.ChatHistory.Items.Count() > index + 1)
                    {
                        this.SystemMessage = this.ChatHistory.Items.ElementAt(index + 1).Content;
                    }
                    else
                    {
                        this.SystemMessage = string.Empty;
                    }
                }
                else
                {
                    if (index - 1 >= 0)
                    {
                        this.UserMessage = this.ChatHistory.Items.ElementAt(index - 1).Content;
                    }
                    else
                    {
                        this.UserMessage = string.Empty;
                    }

                    this.SystemMessage = this.ChatHistory.Items.ElementAt(index).Content;
                }
            }

        }
    }
}
