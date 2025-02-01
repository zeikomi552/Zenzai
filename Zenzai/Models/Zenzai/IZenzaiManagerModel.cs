using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;

namespace Zenzai.Models.Zenzai
{
    public interface IZenzaiManagerModel
    {
        public OllamaControllerModel OllamaCtrl { get; set; }

        public WebUIControllerModel WebUICtrl { get; set; }

        public string ImagePrompt { get; }

        public ChatManagerModel ChatHistory { get; set; }

        public string UserMessage { get; }

        public string SystemMessage { get; }

        public void Initialize();
    }
}
