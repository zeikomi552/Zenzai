using Microsoft.Win32;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Common.Utilities;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;

namespace Zenzai.Models.Zenzai
{
    public class ZenzaiConfigManager : BindableBase
    {
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

        /// <summary>
        /// 一時フォルダ
        /// </summary>
        private const string TemporaryDirectoryName = "ZenzaiTemporary";
        /// <summary>
        /// 一時フォルダ配下に作成する保存用フォルダ
        /// </summary>
        private const string TemporarySaveDirectoryName = "SaveConfig";
        /// <summary>
        /// 一時フォルダ配下に作成するロード用フォルダ
        /// </summary>
        private const string TemporaryLoadDirectoryName = "LoadConfig";
        /// <summary>
        /// WebUI用のコンフィグファイル名
        /// </summary>
        private const string WebuiConfigFileName = "webui.conf";
        /// <summary>
        /// Ollama用のコンフィグファイル名
        /// </summary>
        private const string OllamaConfigFileName = "ollama.conf";

        public ZenzaiConfigManager(IOllamaControllerModel ollama, IWebUIControllerModel webui)
        {
            this.OllamaConfig.SetParameters(ollama);
            this.WebUIConfig.SetParameters(webui);
        }


        #region Configファイルの読み込み
        /// <summary>
        /// Configファイルの読み込み
        /// </summary>
        private void SaveConfig<T>(string dir, string filename, T value) where T : new()
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


        #region Zipファイルで保存する処理
        /// <summary>
        /// Zipファイルで保存する処理
        /// </summary>
        /// <param name="filepath">保存先ファイルパス</param>
        public void SaveZipConfig(string filepath)
        {
            // ↓Zip保存
            string tmpdir = Path.GetTempPath();
            string path = PathManager.GetApplicationFolder();
            string zipbaseDir = Path.Combine(tmpdir, TemporaryDirectoryName, TemporarySaveDirectoryName);

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

            SaveConfig<WebUIConfig>(zipbaseDir, WebuiConfigFileName, (WebUIConfig)this.WebUIConfig);
            SaveConfig<OllamaConfig>(zipbaseDir, OllamaConfigFileName, (OllamaConfig)this.OllamaConfig);

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

        public void LoadZipConfig(string filepath)
        {
            string tmpdir = Path.GetTempPath();
            string path = PathManager.GetApplicationFolder();
            string zipbaseDir = Path.Combine(tmpdir, TemporaryDirectoryName, TemporaryLoadDirectoryName);

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
                filepath,
            zipbaseDir);

            this.WebUIConfig = LoadConfig<WebUIConfig>(zipbaseDir, WebuiConfigFileName)!;
            this.OllamaConfig = LoadConfig<OllamaConfig>(zipbaseDir, OllamaConfigFileName)!;
        }

        #region コンフィグファイルの読み込み
        /// <summary>
        /// コンフィグファイルの読み込み
        /// </summary>
        private T? LoadConfig<T>(string dir, string filename) where T : new()
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

        /// <summary>
        /// コンフィグファイルの保存処理
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                SaveConfig<WebUIConfig>("Config", "webui.conf", (WebUIConfig)this.WebUIConfig);
                SaveConfig<OllamaConfig>("Config", "ollama.conf", (OllamaConfig)this.OllamaConfig);
            }
            catch { }
        }
    }
}
