using Stdapi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public interface IWebUIConfig
    {
        public string WebuiUri { get; }
        public string WebuiOutputDirectory { get; }
        public string WebuiCurrentDirectory { get; }

        public string Prompt { get; }
        public string NegativePrompt { get; }

        public int Steps { get; }
        public int Width { get; }
        public int Height { get; }
        public decimal CfgScale { get; }

        public string SamplerIndex {  get; }

        public SamplerIndexEnum? Sampler { get; }

        public int N_iter { get; }

        public int BatchSize { get; }

        public Int64 Seed { get; }

        public void SetParameters(IWebUIControllerModel ctrl);
    }
}
