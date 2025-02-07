using DryIoc.ImTools;
using MahApps.Metro.Converters;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Ollapi.api;
using Ollapi.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Zenzai.Common.Utilities;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;
using Zenzai.Models.Zenzai;

namespace Zenzai.ViewModels
{
    public class StoryCreatorViewModel : BindableBase
    {
        #region 本アプリ操作用モデル
        /// <summary>
        /// 本アプリ操作用モデル
        /// </summary>
        ZenzaiManagerModel _ZenzaiManager;
        /// <summary>
        /// 本アプリ操作用モデル
        /// </summary>
        public ZenzaiManagerModel ZenzaiManager
        {
            get
            {
                return _ZenzaiManager;
            }
            set
            {
                if (_ZenzaiManager == null || !_ZenzaiManager.Equals(value))
                {
                    _ZenzaiManager = value;
                    RaisePropertyChanged("ZenzaiManager");
                }
            }
        }
        #endregion

        #region コマンド用
        private DelegateCommand? _showDialogCommand;
        public DelegateCommand ShowDialogCommand => _showDialogCommand ?? (_showDialogCommand = new DelegateCommand(ShowDialog));
        private DelegateCommand? _ChatCommand;
        public DelegateCommand ChatCommand => _ChatCommand ?? (_ChatCommand = new DelegateCommand(Chat));

        private DelegateCommand? _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ?? (_SaveCommand = new DelegateCommand(Save));

        private DelegateCommand? _LoadCommand;
        public DelegateCommand LoadCommand => _LoadCommand ?? (_LoadCommand = new DelegateCommand(Load));


        private DelegateCommand? _SaveMarkdownCommand;
        public DelegateCommand SaveMarkdownCommand => _SaveMarkdownCommand ?? (_SaveMarkdownCommand = new DelegateCommand(SaveMarkdown));



        #endregion

        private IDialogService _dialogService;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService"></param>
        public StoryCreatorViewModel(IDialogService dialogService, IOllamaControllerModel ollama, IWebUIControllerModel webUi)
        {
            _dialogService = dialogService;
            _ZenzaiManager = new ZenzaiManagerModel(ollama, webUi);
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

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            try
            {
                // 初期化処理
                this.ZenzaiManager.Initialize();
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
        public void Chat()
        {
            try
            {
                // 最初のチャット
                this.ZenzaiManager.Chat(this.SendMessage);

                this.SendMessage = string.Empty;
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

        #region ゲームファイルの保存処理
        /// <summary>
        /// ゲームファイルの保存処理
        /// </summary>
        private void Save()
        {
            try
            {
                this.ZenzaiManager.Save();
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
        private void Load()
        {
            try
            {
                this.ZenzaiManager.Load();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region マークダウンの保存処理
        /// <summary>
        /// マークダウンの保存処理
        /// </summary>
        private void SaveMarkdown()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "ストーリー (*.md)|*.md";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    StringBuilder text = new StringBuilder();

                    var dir = System.IO.Path.GetDirectoryName(dialog.FileName)!;

                    foreach (var tmp in this.ZenzaiManager.ChatHistory.Items)
                    {
                        text.AppendLine(tmp.Role);
                        text.AppendLine("```");
                        text.AppendLine(tmp.Content);
                        text.AppendLine("```");
                        text.AppendLine();

                        if (!string.IsNullOrEmpty(tmp.FilePath))
                        {
                            var filename = System.IO.Path.GetFileName(tmp.FilePath);
                            var filepath = Path.Combine(dir, filename);
                            File.Copy(tmp.FilePath, filepath);
                            text.AppendLine($"![]({filename})");
                            text.AppendLine();
                            text.AppendLine($"Prompt");
                            text.AppendLine($"");
                            text.AppendLine($"```");
                            text.AppendLine($"{tmp.Prompt}");
                            text.AppendLine($"```");
                            text.AppendLine($"Negative Prompt");
                            text.AppendLine($"");
                            text.AppendLine($"");
                            text.AppendLine($"```");
                            text.AppendLine($"{tmp.NegativePrompt}");
                            text.AppendLine($"```");
                        }
                        text.AppendLine();
                    }
                    using (StreamWriter writer = new StreamWriter(dialog.FileName, false, Encoding.UTF8)) // ファイルへの書き込み
                    {
                        writer.WriteLine(text.ToString()); // データを書き込む
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
