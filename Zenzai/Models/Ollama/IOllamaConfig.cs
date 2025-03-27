using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Models.Zenzai;

namespace Zenzai.Models.Ollama
{
    public interface IOllamaConfig
    {
        public string FirstMessage { get; }

        public string PromptMessage { get; }

        public PersonaCollection Personas { get; }

        public string Host { get; }

        public int Port { get; }

        public string Model { get; }

        public void SetParameters(IOllamaControllerModel ctrl);
    }
}
