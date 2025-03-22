using Ollapi.api;
using Ollapi.Common;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.Ollama
{
    public class OllamaControllerModel : OllamaConfig, IOllamaControllerModel
    {
        #region 最初のメッセージ
        /// <summary>
        /// 最初のメッセージ
        /// </summary>
        public async Task<OllapiChatResponse> BaseChat(List<IOllapiMessage> sourceList)
        {
            try
            {
                // Ollapiの起動
                var ollapi = new OllapiChatRequest(this.Host, this.Port, this.Model);

                // 接続
                ollapi.Open();

                // リクエストの実行
                var ret = await ollapi.Request(sourceList);

                // メッセージの展開
                var tmp = JSONUtil.DeserializeFromText<OllapiChatResponse>(ret);

                // 切断
                ollapi.Close();

                return tmp;
            }
            catch
            {
                return new OllapiChatResponse();
            }
        }
        #endregion


        #region Configのセット処理
        /// <summary>
        /// Configのセット処理
        /// </summary>
        /// <param name="config">Config</param>
        public void SetConfig(IOllamaConfig config)
        {
            this.FirstMessage = config.FirstMessage;
            this.PromptMessage = config.PromptMessage;
            this.SystemMessage = config.SystemMessage;
            this.SystemMessage2 = config.SystemMessage2;
            this.Role = config.Role;
            this.Host = config.Host;
            this.Port = config.Port;
            this.Model = config.Model;
        }
        #endregion
    }
}
