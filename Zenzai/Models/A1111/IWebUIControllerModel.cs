using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public interface IWebUIControllerModel : IWebUIConfig
    {
        public WebUIBaseModel WebUI { get; }
        public void InitWebUI();
        public void CloseWebUI();
        public Task<string> ExecutePrompt(string prompt);

        public void SetConfig(WebUIConfig config);
    }
}
