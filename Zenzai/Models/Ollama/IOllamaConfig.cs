using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.Ollama
{
    public interface IOllamaConfig
    {
        public string FirstMessage { get; }

        public string PromptMessage { get; }

        public string SystemMessage { get; }

        public string Role { get; }

        public string Host { get; }

        public int Port { get; }

        public string Model { get; }

        public void SetParameters(IOllamaControllerModel ctrl);
    }
}
