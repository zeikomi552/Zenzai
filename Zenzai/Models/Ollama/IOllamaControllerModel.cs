using Ollapi.api;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.Ollama
{
    public interface IOllamaControllerModel : IOllamaConfig
    {
        public Task<OllapiChatResponse> BaseChat(List<IOllapiMessage> sourceList, string message);

        public void SetConfig(OllamaConfig config);

    }
}
