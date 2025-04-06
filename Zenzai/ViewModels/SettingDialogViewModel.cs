using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Ollapi.api;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zenzai.Common.Utilities;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;

namespace Zenzai.ViewModels
{
    public class SettingDialogViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware
        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        #region バックアップコマンド
        /// <summary>
        /// バックアップコマンド
        /// </summary>
        private DelegateCommand<string>? _BackUpCommand;
        public DelegateCommand<string> BackUpCommand =>
            _BackUpCommand ?? (_BackUpCommand = new DelegateCommand<string>(BackupSetting));
        #endregion

        #region リストアコマンドの作成
        private DelegateCommand<string>? _RestoreCommand;
        public DelegateCommand<string> RestoreCommand =>
            _RestoreCommand ?? (_RestoreCommand = new DelegateCommand<string>(RestoreSetting));
        #endregion

        private string? _message;
        public string? Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title = "Notification";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DialogCloseListener RequestClose { get; }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                this._OllamaCtrl.SetConfig(this.OllamaConfig);
                this._WebUICtrl.SetConfig(this.WebUIConfig);

                SaveConfig<WebUIConfig>("Config", "webui.conf", (WebUIConfig)this.WebUIConfig);
                SaveConfig<OllamaConfig>("Config", "ollama.conf", (OllamaConfig)this.OllamaConfig);

                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
            {
                result = ButtonResult.Cancel;
            }

            RaiseRequestClose(new DialogResult(result));
        }


        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }
        #endregion


        #region Ollama用Configデータ
        /// <summary>
        /// Ollama用Configデータ
        /// </summary>
        IOllamaConfig _OllamaConfig = new OllamaConfig();
        /// <summary>
        /// Ollama用Configデータ
        /// </summary>
        public IOllamaConfig OllamaConfig
        {
            get
            {
                return _OllamaConfig;
            }
            set
            {
                if (_OllamaConfig == null || !_OllamaConfig.Equals(value))
                {
                    _OllamaConfig = value;
                    RaisePropertyChanged("OllamaConfig");
                }
            }
        }
        #endregion

        #region WebUI用Configデータ
        /// <summary>
        /// WebUI用Configデータ
        /// </summary>
        IWebUIConfig _WebUIConfig = new WebUIConfig();
        /// <summary>
        /// WebUI用Configデータ
        /// </summary>
        public IWebUIConfig WebUIConfig
        {
            get
            {
                return _WebUIConfig;
            }
            set
            {
                if (_WebUIConfig == null || !_WebUIConfig.Equals(value))
                {
                    _WebUIConfig = value;
                    RaisePropertyChanged("WebUIConfig");
                }
            }
        }
        #endregion

        IOllamaControllerModel _OllamaCtrl;
        IWebUIControllerModel _WebUICtrl;


        public SettingDialogViewModel(IOllamaControllerModel ollama, IWebUIControllerModel webui)
        {
            this.OllamaConfig.SetParameters(ollama);
            this.WebUIConfig.SetParameters(webui);

            _OllamaCtrl = ollama;
            _WebUICtrl = webui;
        }


        #region Wordpress用ファイルの読み込み
        /// <summary>
        /// Wordpress用Configファイルの読み込み
        /// </summary>
        public void SaveConfig<T>(string dir, string filename, T value) where T : new()
        {
            try
            {
                var tmp = new ConfigManager<T>(dir, filename, new T());

                tmp.Item = value;

                tmp.SaveXML(); // XMLのセーブ
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 設定ファイルのバックアップ処理
        /// <summary>
        /// 設定ファイルのバックアップ処理
        /// </summary>
        /// <param name="parameter"></param>
        protected virtual void BackupSetting(string parameter)
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "Zenzai設定ファイル (*.znconf)|*.znconf";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this._OllamaCtrl.SetConfig(this.OllamaConfig);
                    this._WebUICtrl.SetConfig(this.WebUIConfig);

                    SaveZipConfig(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }

        }
        #endregion

        #region Zipファイルで保存する処理
        /// <summary>
        /// Zipファイルで保存する処理
        /// </summary>
        /// <param name="filepath">保存先ファイルパス</param>
        private void SaveZipConfig(string filepath)
        {
            // ↓Zip保存
            string tmpdir = Path.GetTempPath();
            string path = PathManager.GetApplicationFolder();
            string zipbaseDir = Path.Combine(tmpdir, "ZenzaiTemporary", "SaveConfig");

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

            SaveConfig<WebUIConfig>(zipbaseDir, "webui.conf", (WebUIConfig)this.WebUIConfig);
            SaveConfig<OllamaConfig>(zipbaseDir, "ollama.conf", (OllamaConfig)this.OllamaConfig);

            // すでにファイルが存在する場合は削除
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            //ZIP書庫を作成
            System.IO.Compression.ZipFile.CreateFromDirectory(
            zipbaseDir,
                filepath,
                System.IO.Compression.CompressionLevel.Optimal,
                false,
                System.Text.Encoding.UTF8);
        }
        #endregion

        #region 設定ファイルのリストア処理
        /// <summary>
        /// 設定ファイルのリストア処理
        /// </summary>
        /// <param name="parameter">コマンドパラメータ（未使用）</param>
        public void RestoreSetting(string parameter)
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "Zenzai設定ファイル (*.znconf)|*.znconf";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    string tmpdir = Path.GetTempPath();
                    string path = PathManager.GetApplicationFolder();
                    string zipbaseDir = Path.Combine(tmpdir, "ZenzaiTemporary", "LoadConfig");

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

                    WebUIConfig webuiConf = LoadConfig<WebUIConfig>(zipbaseDir, "webui.conf")!;
                    this._WebUICtrl.SetConfig(webuiConf);
                    this.WebUIConfig.SetParameters(this._WebUICtrl);

                    OllamaConfig ollamaConf = LoadConfig<OllamaConfig>(zipbaseDir, "ollama.conf")!;
                    this._OllamaCtrl.SetConfig(ollamaConf);
                    this.OllamaConfig.SetParameters(this._OllamaCtrl);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion


        #region Wordpress用ファイルの読み込み
        /// <summary>
        /// Wordpress用Configファイルの読み込み
        /// </summary>
        public T? LoadConfig<T>(string dir, string filename) where T : new()
        {
            try
            {
                var tmp = new ConfigManager<T>(dir, filename, new T());

                // ファイルの存在確認
                if (!File.Exists(tmp.ConfigFile))
                {
                    tmp.SaveXML(); // XMLのセーブ
                }
                else
                {
                    tmp.LoadXML(); // XMLのロード
                }
                return tmp.Item;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
