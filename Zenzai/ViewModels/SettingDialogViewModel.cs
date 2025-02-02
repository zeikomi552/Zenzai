using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
