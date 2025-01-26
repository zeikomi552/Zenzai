using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenzai.Views;

namespace Zenzai.Models
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("StoryCreatorRegion", typeof(StoryCreator));
            //regionManager.RegisterViewWithRegion("GitHubLanguageRegion", typeof(ucGitHubLanguageV));
            //regionManager.RegisterViewWithRegion("WordpressRegion", typeof(ucWordpressV));
            //regionManager.RegisterViewWithRegion("CivitaiRegion", typeof(ucCivitaiV));
            //regionManager.RegisterViewWithRegion("CivitaiImageRegion", typeof(ucCivitaiImageV));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }

}
