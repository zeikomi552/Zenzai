using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Common.Utilities;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;

namespace Zenzai.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IWebUIControllerModel _WebuiCtrl;
        IOllamaControllerModel _OllamaCtrl;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ollama">Ollamaデータ</param>
        /// <param name="webui">WebUIデータ</param>
        public MainWindowViewModel(IOllamaControllerModel ollama, IWebUIControllerModel webui)
        {
            _WebuiCtrl = webui;
            _OllamaCtrl = ollama;
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            Load();
        }
        #endregion

        #region Configファイルのロード処理
        /// <summary>
        /// Configファイルのロード処理
        /// </summary>
        private void Load()
        {
            WebUIConfig webuiConf = LoadConfig<WebUIConfig>("Config", "webui.conf")!;
            this._WebuiCtrl.SetConfig(webuiConf);

            OllamaConfig ollamaConf = LoadConfig<OllamaConfig>("Config", "ollama.conf")!;
            this._OllamaCtrl.SetConfig(ollamaConf);
        }
        #endregion

        #region アプリケーションのClose処理
        /// <summary>
        /// アプリケーションのClose処理
        /// </summary>
        public void Closing()
        {
            _WebuiCtrl.CloseWebUI();
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
