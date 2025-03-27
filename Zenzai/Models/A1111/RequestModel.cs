using Ollapi.Common;
using Stdapi;
using Stdapi.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Common.Utilities;

namespace Zenzai.Models.A1111
{
    public class RequestModel : BindableBase
    {
        #region POSTのリクエスト実行処理
        /// <summary>
        /// POSTのリクエスト実行処理
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="outdir">出力先ディレクトリ</param>
        public async Task<(bool, List<string>)> PostRequest(string uri, string outdir, StdTxt2ImageM prompt)
        {
            try
            {
                StdClient client = new StdClient();

                var ret = await client.Txt2ImgRequest(uri, prompt);

                int count = 0;
                List<string> ret_path = new List<string>();
                foreach (var base64string in ret.Images)
                {
                    var path = Path.Combine(outdir, $"{DateTime.Now.ToString("yyyyMMddHHmmss-") + count.ToString()}.png");
                    StdClient.ConvertImage(base64string, path);

                    ret_path.Add(path);
                    count++;
                }

                return (true, ret_path);
            }
            catch (JSONDeserializeException e)
            {
                string msg = e.Message + "\r\n" + e.JSON;
                ShowMessage.ShowErrorOK(msg, "Error");
                return (false, new List<string>());
            }
            finally
            {

            }
        }
        #endregion

        #region POSTのリクエスト実行処理
        /// <summary>
        /// POSTのリクエスト実行処理
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="outdir">出力先ディレクトリ</param>
        public async Task<bool> PostOptions(string uri, string checkpoint, int clip_CLIP_stop_at_last_layers)
        {
            try
            {
                StdClient client = new StdClient();

                var ret = await client.PostOptionsRequest(uri, new Stdapi.Models.Post.PostOptions()
                {
                    SdModelCheckpoint = checkpoint,
                    CLIPStopAtLastLayers = clip_CLIP_stop_at_last_layers
                });

                return (ret.Equals("null"));
            }
            catch (JSONDeserializeException e)
            {
                string msg = e.Message + "\r\n" + e.JSON;
                ShowMessage.ShowErrorOK(msg, "Error");
                return (false);
            }
            finally
            {

            }
        }
        #endregion
        #region プロンプト要素[PromptItem]プロパティ
        /// <summary>
        /// プロンプト要素[PromptItem]プロパティ用変数
        /// </summary>
        StdTxt2ImageM _PromptItem = new StdTxt2ImageM();
        /// <summary>
        /// プロンプト要素[PromptItem]プロパティ
        /// </summary>
        public StdTxt2ImageM PromptItem
        {
            get
            {
                return _PromptItem;
            }
            set
            {
                if (_PromptItem == null || !_PromptItem.Equals(value))
                {
                    _PromptItem = value;
                    RaisePropertyChanged("PromptItem");
                }
            }
        }
        #endregion

    }
}
