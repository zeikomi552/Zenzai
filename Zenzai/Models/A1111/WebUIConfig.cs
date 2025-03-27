using Stdapi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public class WebUIConfig : BindableBase, IWebUIConfig
    {

        #region WebUI用URI
        /// <summary>
        /// WebUI用URI
        /// </summary>
        string _WebuiUri = "http://127.0.0.1:7861";
        /// <summary>
        /// WebUI用URI
        /// </summary>
        public string WebuiUri
        {
            get
            {
                return _WebuiUri;
            }
            set
            {
                if (_WebuiUri == null || !_WebuiUri.Equals(value))
                {
                    _WebuiUri = value;
                    RaisePropertyChanged("WebuiUri");
                }
            }
        }
        #endregion

        #region WebUI用画像出力先ディレクトリ
        /// <summary>
        /// WebUI用画像出力先ディレクトリ
        /// </summary>
        string _WebuiOutputDirectory = @"C:\output";
        /// <summary>
        /// WebUI用画像出力先ディレクトリ
        /// </summary>
        public string WebuiOutputDirectory
        {
            get
            {
                return _WebuiOutputDirectory;
            }
            set
            {
                if (_WebuiOutputDirectory == null || !_WebuiOutputDirectory.Equals(value))
                {
                    _WebuiOutputDirectory = value;
                    RaisePropertyChanged("WebuiOutputDirectory");
                }
            }
        }
        #endregion

        #region WebUI用カレントディレクトリ
        /// <summary>
        /// WebUI用カレントディレクトリ
        /// </summary>
        string _WebuiCurrentDirectory = @"C:\Work\stable-diffusion-webui";
        /// <summary>
        /// WebUI用カレントディレクトリ
        /// </summary>
        public string WebuiCurrentDirectory
        {
            get
            {
                return _WebuiCurrentDirectory;
            }
            set
            {
                if (_WebuiCurrentDirectory == null || !_WebuiCurrentDirectory.Equals(value))
                {
                    _WebuiCurrentDirectory = value;
                    RaisePropertyChanged("WebuiCurrentDirectory");
                }
            }
        }
        #endregion

        #region プロンプトリスト
        /// <summary>
        /// プロンプトリスト
        /// </summary>
        SdPromptCollection _Prompts = new SdPromptCollection();
        /// <summary>
        /// プロンプトリスト
        /// </summary>
        public SdPromptCollection Prompts
        {
            get
            {
                return _Prompts;
            }
            set
            {
                if (_Prompts == null || !_Prompts.Equals(value))
                {
                    _Prompts = value;
                    RaisePropertyChanged("Prompts");
                }
            }
        }
        #endregion

        #region 使用するモデル
        /// <summary>
        /// 使用するモデル
        /// </summary>
        string _CheckPoint = string.Empty;
        /// <summary>
        /// 使用するモデル
        /// </summary>
        public string CheckPoint
        {
            get
            {
                return _CheckPoint;
            }
            set
            {
                if (_CheckPoint == null || !_CheckPoint.Equals(value))
                {
                    _CheckPoint = value;
                    RaisePropertyChanged("CheckPoint");
                }
            }
        }
        #endregion

        #region CLIP_stop_at_last_layers
        /// <summary>
        /// CLIP_stop_at_last_layers
        /// </summary>
        int _CLIPStopAtLastLayers = 2;
        /// <summary>
        /// CLIP_stop_at_last_layers
        /// </summary>
        public int CLIPStopAtLastLayers
        {
            get
            {
                return _CLIPStopAtLastLayers;
            }
            set
            {
                if (!_CLIPStopAtLastLayers.Equals(value))
                {
                    _CLIPStopAtLastLayers = value;
                    RaisePropertyChanged("CLIPStopAtLastLayers");
                }
            }
        }
        #endregion

        #region Steps[Steps]プロパティ
        /// <summary>
        /// Steps[Steps]プロパティ用変数
        /// </summary>
        int _Steps = 40;
        /// <summary>
        /// Steps[Steps]プロパティ
        /// </summary>
        public int Steps
        {
            get
            {
                return _Steps;
            }
            set
            {
                if (!_Steps.Equals(value))
                {
                    _Steps = value;
                    RaisePropertyChanged("Steps");
                }
            }
        }
        #endregion

        #region Picture width[Width]プロパティ
        /// <summary>
        /// Picture width[Width]プロパティ用変数
        /// </summary>
        int _Width = 512;
        /// <summary>
        /// Picture width[Width]プロパティ
        /// </summary>
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (!_Width.Equals(value))
                {
                    _Width = value;
                    RaisePropertyChanged("Width");
                }
            }
        }
        #endregion

        #region Picture height[Height]プロパティ
        /// <summary>
        /// Picture height[Height]プロパティ用変数
        /// </summary>
        int _Height = 768;
        /// <summary>
        /// Picture height[Height]プロパティ
        /// </summary>
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (!_Height.Equals(value))
                {
                    _Height = value;
                    RaisePropertyChanged("Height");
                }
            }
        }
        #endregion

        #region cfg scale value[CfgScale]プロパティ
        /// <summary>
        /// cfg scale value[CfgScale]プロパティ用変数
        /// </summary>
        decimal _CfgScale = 7;
        /// <summary>
        /// cfg scale value[CfgScale]プロパティ
        /// </summary>
        public decimal CfgScale
        {
            get
            {
                return _CfgScale;
            }
            set
            {
                if (!_CfgScale.Equals(value))
                {
                    if (value >= 0 && value <= 30)
                    {
                        _CfgScale = value;
                        RaisePropertyChanged("CfgScale");
                    }
                }
            }
        }
        #endregion

        #region Picture Sampler[SamplerIndex]プロパティ
        /// <summary>
        /// Picture Sampler[SamplerIndex]プロパティ用変数
        /// </summary>
        string _SamplerIndex = "DPM++ 2M Karras";
        /// <summary>
        /// Picture Sampler[SamplerIndex]プロパティ
        /// </summary>
        public string SamplerIndex
        {
            get
            {
                return _SamplerIndex;
            }
            set
            {
                if (_SamplerIndex == null || !_SamplerIndex.Equals(value))
                {
                    _SamplerIndex = value;
                    RaisePropertyChanged("SamplerIndex");
                    RaisePropertyChanged("Sampler");
                }
            }
        }
        #endregion

        #region Picture Sampler[Sampler]プロパティ
        /// <summary>
        /// Picture Sampler[Sampler]プロパティ
        /// </summary>
        public SamplerIndexEnum? Sampler
        {
            get
            {
                // 値を列挙してコンソールに出力する
                var samplers = Enum.GetValues(typeof(SamplerIndexEnum));

                // SamplerIndexの列挙を確認
                foreach (SamplerIndexEnum sampler in samplers)
                {
                    DescriptionAttribute? t = new DescriptionAttribute();   // Description属性を取得する
                    sampler.TryGetAttribute<DescriptionAttribute>(out t);

                    // SamplerIndexとの一致確認
                    if (t != null && t.Description.Equals(this.SamplerIndex))
                    {
                        return sampler;
                    }
                }
                return null;

            }
            set
            {
                if (_SamplerIndex == null || !_SamplerIndex.Equals(value))
                {
                    DescriptionAttribute? t = new DescriptionAttribute();   // Description属性を取得する

                    if (value != null && value.TryGetAttribute<DescriptionAttribute>(out t))
                    {
                        _SamplerIndex = t!.Description;
                        RaisePropertyChanged("Sampler");
                    }
                    else
                    {
                        _SamplerIndex = string.Empty;
                        RaisePropertyChanged("Sampler");
                    }
                }
            }
        }
        #endregion

        #region n_iter value[N_iter]プロパティ
        /// <summary>
        /// n_iter value[N_iter]プロパティ用変数
        /// </summary>
        int _N_iter = 1;
        /// <summary>
        /// n_iter value[N_iter]プロパティ
        /// </summary>
        public int N_iter
        {
            get
            {
                return _N_iter;
            }
            set
            {
                if (!_N_iter.Equals(value))
                {
                    if (value >= 1 && value <= 100)
                    {
                        _N_iter = value;
                        RaisePropertyChanged("N_iter");
                    }
                }
            }
        }
        #endregion

        #region Batch size value[BatchSize]プロパティ
        /// <summary>
        /// Batch size value[BatchSize]プロパティ用変数
        /// </summary>
        int _BatchSize = 1;
        /// <summary>
        /// Batch size value[BatchSize]プロパティ
        /// </summary>
        public int BatchSize
        {
            get
            {
                return _BatchSize;
            }
            set
            {
                if (!_BatchSize.Equals(value))
                {
                    if (value >= 1 && value <= 8)
                    {
                        _BatchSize = value;
                        RaisePropertyChanged("BatchSize");
                    }
                }
            }
        }
        #endregion

        #region Random seed[Seed]プロパティ
        /// <summary>
        /// Random seed[Seed]プロパティ用変数
        /// </summary>
        Int64 _Seed = -1;
        /// <summary>
        /// Random seed[Seed]プロパティ
        /// </summary>
        public Int64 Seed
        {
            get
            {
                return _Seed;
            }
            set
            {
                if (!_Seed.Equals(value))
                {
                    _Seed = value;
                    RaisePropertyChanged("Seed");
                }
            }
        }
        #endregion
        #region Seed値のバックアップ（最後に実行したSeed値）[SeedBackup]プロパティ
        /// <summary>
        /// Seed値のバックアップ（最後に実行したSeed値）[SeedBackup]プロパティ用変数
        /// </summary>
        Int64 _SeedBackup = -1;
        /// <summary>
        /// Seed値のバックアップ（最後に実行したSeed値）[SeedBackup]プロパティ
        /// </summary>
        public Int64 SeedBackup
        {
            get
            {
                return _SeedBackup;
            }
            set
            {
                if (!_SeedBackup.Equals(value))
                {
                    _SeedBackup = value;
                    RaisePropertyChanged("SeedBackup");
                }
            }
        }
        #endregion
        #region パラメーターのセット処理
        /// <summary>
        /// パラメーターのセット処理
        /// </summary>
        /// <param name="ctrl">コントローラー</param>
        public void SetParameters(IWebUIControllerModel ctrl)
        {
            this.WebuiUri = ctrl.WebuiUri;
            this.WebuiOutputDirectory = ctrl.WebuiOutputDirectory;
            this.WebuiCurrentDirectory = ctrl.WebuiCurrentDirectory;
            this.Prompts = ctrl.Prompts;

            this.Steps = ctrl.Steps;
            this.Width = ctrl.Width;
            this.Height = ctrl.Height;
            this.CfgScale = ctrl.CfgScale;
            this.SamplerIndex = ctrl.SamplerIndex;
            this.Sampler = ctrl.Sampler;
            this.N_iter = ctrl.N_iter;
            this.BatchSize = ctrl.BatchSize;
            this.Seed = ctrl.Seed;
        }
        #endregion

        Random _Rand = new Random();

        #region Payloadの作成処理
        /// <summary>
        /// Payloadの作成処理
        /// </summary>
        /// <returns></returns>
        public StringContent GetPayload()
        {
            var prompt = this;
            this.SeedBackup = _Rand.Next(); // ランダムのSeed値を作成しておく

            var data = new
            {
                prompt = prompt.Prompts.SelectedItem.Prompt,
                negative_prompt = prompt.Prompts.SelectedItem.Prompt,
                steps = prompt.Steps,
                width = prompt.Width,
                height = prompt.Height,
                cfg_scale = prompt.CfgScale,
                sampler_index = prompt.SamplerIndex,
                n_iter = prompt.N_iter,
                batch_size = prompt.BatchSize,
                seed = prompt.Seed < 0 ? this.SeedBackup : prompt.Seed,
                //override_settings = new
                //{
                //    sd_model_checkpoint = prompt.CheckPoint
                //}
            };

            Debug.WriteLine(data.ToString());

            return data.AsJson();
        }
        #endregion
    }
}
