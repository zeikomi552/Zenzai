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
using Zenzai.Models.Zenzai;

namespace Zenzai.ViewModels
{
    public class SettingDialogViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware
        #region ダイアログを閉じるコマンド
        /// <summary>
        /// ダイアログを閉じるコマンド
        /// </summary>
        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));
        #endregion

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

        #region ウィンドウタイトル
        /// <summary>
        /// ウィンドウタイトル
        /// </summary>
        private string _title = "Setting Window";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion

        #region クローズリクエスト
        /// <summary>
        /// クローズリクエスト
        /// </summary>
        public DialogCloseListener RequestClose { get; }
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose.Invoke(dialogResult);
        }
        #endregion

        #region ダイアログを閉じる処理
        /// <summary>
        /// ダイアログを閉じる処理
        /// </summary>
        /// <param name="parameter">コマンドパラメータ true: OKボタン false:キャンセルボタン</param>
        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                this._OllamaCtrl.SetConfig(this.ZenzaiConfig!.OllamaConfig);
                this._WebUICtrl.SetConfig(this.ZenzaiConfig!.WebUIConfig);

                // Configファイルの保存処理
                this.ZenzaiConfig!.SaveConfig();

                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
            {
                result = ButtonResult.Cancel;
            }

            RaiseRequestClose(new DialogResult(result));
        }
        #endregion

        #region クローズの可否を確認
        /// <summary>
        /// クローズの可否を確認
        /// </summary>
        /// <returns></returns>
        public virtual bool CanCloseDialog()
        {
            return true;
        }
        #endregion

        #region クローズ処理の実行
        /// <summary>
        /// クローズ処理の実行
        /// </summary>
        public virtual void OnDialogClosed()
        {

        }
        #endregion

        #region ダイアログをOpen時の処理
        /// <summary>
        /// ダイアログをOpen時の処理
        /// </summary>
        /// <param name="parameters">コマンドパラメータ</param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }
        #endregion
        #endregion

        #region ZenzaiConfig用マネージャー
        /// <summary>
        /// ZenzaiConfig用マネージャー
        /// </summary>
        ZenzaiConfigManager? _ZenzaiConfig;
        /// <summary>
        /// ZenzaiConfig用マネージャー
        /// </summary>
        public ZenzaiConfigManager? ZenzaiConfig
        {
            get
            {
                return _ZenzaiConfig;
            }
            set
            {
                if (_ZenzaiConfig == null || !_ZenzaiConfig.Equals(value))
                {
                    _ZenzaiConfig = value;
                    RaisePropertyChanged("ZenzaiConfig");
                }
            }
        }
        #endregion

        #region Ollamaコントロール用オブジェクト
        /// <summary>
        /// Ollamaコントロール用オブジェクト
        /// </summary>
        IOllamaControllerModel _OllamaCtrl;
        #endregion

        #region WebUIコントロール用オブジェクト
        /// <summary>
        /// WebUIコントロール用オブジェクト
        /// </summary>
        IWebUIControllerModel _WebUICtrl;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ollama">Ollamaコントロール用オブジェクト</param>
        /// <param name="webui">WebUIコントロール用オブジェクト</param>
        public SettingDialogViewModel(IOllamaControllerModel ollama, IWebUIControllerModel webui)
        {
            this.ZenzaiConfig = new ZenzaiConfigManager(ollama, webui);

            this._OllamaCtrl = ollama;
            this._WebUICtrl = webui;
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
                    this._OllamaCtrl.SetConfig(this.ZenzaiConfig!.OllamaConfig);
                    this._WebUICtrl.SetConfig(this.ZenzaiConfig!.WebUIConfig);
                    this.ZenzaiConfig!.SaveZipConfig(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }

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
                    this.ZenzaiConfig!.LoadZipConfig(dialog.FileName);
                    this._WebUICtrl.SetConfig(this.ZenzaiConfig!.WebUIConfig);
                    this._OllamaCtrl.SetConfig(this.ZenzaiConfig!.OllamaConfig);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
