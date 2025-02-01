using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Models.A1111;

namespace Zenzai.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IWebUIControllerModel _WebuiCtrl;
        public MainWindowViewModel(IWebUIControllerModel webUi)
        {
            _WebuiCtrl = webUi;
        }
        public void Closing()
        {
            _WebuiCtrl.CloseWebUI();
        }
    }

}
