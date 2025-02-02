using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.Ollama
{
    public class OllamaConfig : BindableBase, IOllamaConfig
    {
        #region ゲームの候補を列挙する最初のメッセージ
        /// <summary>
        /// ゲームの候補を列挙する最初のメッセージ
        /// </summary>
        string _FirstMessage = "あなたとできるチャットゲームのおススメを教えてください。\r\nTRPGのようなものを想定しています。\r\n以降、全て日本語でお願いします。\r\n適切な位置で改行をお願いします。";
        /// <summary>
        /// ゲームの候補を列挙する最初のメッセージ
        /// </summary>
        public string FirstMessage
        {
            get
            {
                return _FirstMessage;
            }
            set
            {
                if (_FirstMessage == null || !_FirstMessage.Equals(value))
                {
                    _FirstMessage = value;
                    RaisePropertyChanged("FirstMessage");
                }
            }
        }
        #endregion

        #region プロンプト生成用チャットメッセージ
        /// <summary>
        /// プロンプト生成用チャットメッセージ
        /// </summary>
        string _PromptMessage = "雰囲気を感じさせる画像をStable diffusionで出したいと思います。\r\nStable Diffusion用のプロンプトを英語で教えてください。";
        /// <summary>
        /// プロンプト生成用チャットメッセージ
        /// </summary>
        public string PromptMessage
        {
            get
            {
                return _PromptMessage;
            }
            set
            {
                if (_PromptMessage == null || !_PromptMessage.Equals(value))
                {
                    _PromptMessage = value;
                    RaisePropertyChanged("PromptMessage");
                }
            }
        }
        #endregion

        #region Ollamaでの発信側のロール
        /// <summary>
        /// Ollamaでの発信側のロール
        /// </summary>
        string _Role = "user";
        /// <summary>
        /// Ollamaでの発信側のロール
        /// </summary>
        public string Role
        {
            get
            {
                return _Role;
            }
            set
            {
                if (_Role == null || !_Role.Equals(value))
                {
                    _Role = value;
                    RaisePropertyChanged("Role");
                }
            }
        }
        #endregion

        #region ホスト名
        /// <summary>
        /// ホスト名
        /// </summary>
        string _Host = "localhost";
        /// <summary>
        /// ホスト名
        /// </summary>
        public string Host
        {
            get
            {
                return _Host;
            }
            set
            {
                if (_Host == null || !_Host.Equals(value))
                {
                    _Host = value;
                    RaisePropertyChanged("Host");
                }
            }
        }
        #endregion

        #region ポート
        /// <summary>
        /// ポート
        /// </summary>
        int _Port = 11434;
        /// <summary>
        /// ポート
        /// </summary>
        public int Port
        {
            get
            {
                return _Port;
            }
            set
            {
                if (!_Port.Equals(value))
                {
                    _Port = value;
                    RaisePropertyChanged("Port");
                }
            }
        }
        #endregion

        #region 使用モデル
        /// <summary>
        /// 使用モデル
        /// </summary>
        string _Model = "example";
        /// <summary>
        /// 使用モデル
        /// </summary>
        public string Model
        {
            get
            {
                return _Model;
            }
            set
            {
                if (_Model == null || !_Model.Equals(value))
                {
                    _Model = value;
                    RaisePropertyChanged("Model");
                }
            }
        }
        #endregion

        #region パラメーターのセット処理
        /// <summary>
        /// パラメーターのセット処理
        /// </summary>
        /// <param name="ctrl">コントローラー</param>
        public void SetParameters(IOllamaControllerModel ctrl)
        {
            this.FirstMessage = ctrl.FirstMessage;
            this.PromptMessage = ctrl.PromptMessage;
            this.Role = ctrl.Role;
            this.Host = ctrl.Host;
            this.Port = ctrl.Port;
            this.Model = ctrl.Model;
        }
        #endregion
    }
}
