using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zenzai.Models;
using Zenzai.Models.A1111;
using Zenzai.Models.Ollama;
using Zenzai.ViewModels;
using Zenzai.Views;

namespace Zenzai
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // シングルトンクラスとして登録したい時
            containerRegistry.RegisterDialog<SettingDialog, SettingDialogViewModel>();

            containerRegistry.RegisterSingleton<IOllamaControllerModel?, OllamaControllerModel>();
            containerRegistry.RegisterSingleton<IWebUIControllerModel?, WebUIControllerModel>();

        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Module>();
        }
    }

}
