using Ollapi.api;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.Ollama
{
    public interface IOllamaControllerModel
    {
        public string FirstMessage { get; }

        public string PromptMessage {  get; }

        public string Role { get; }

        public string Host { get; }

        public int Port { get; }

        public string Model { get; }

        public Task<OllapiChatResponse> BaseChat(List<IOllapiMessage> sourceList, string message);
    }
}
