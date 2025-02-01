using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public interface IWebUIControllerModel
    {
        public WebUIBaseModel WebUI { get; }
        public string WebuiUri { get; }
        public string WebuiOutputDirectory { get; }
        public string WebuiCurrentDirectory {  get; }
        public string NegativePrompt {  get; }
        public void InitWebUI();
        public void CloseWebUI();

        public Task<string> ExecutePrompt(string prompt);
    }
}
