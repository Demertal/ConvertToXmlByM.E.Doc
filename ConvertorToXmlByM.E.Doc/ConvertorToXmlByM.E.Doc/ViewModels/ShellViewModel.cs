using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ConvertorToXmlByM.E.Doc.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand<string> NavigateCommand { get; }

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            LoadedCommand = new DelegateCommand(Load);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            
        }

        private void Load()
        {
            _regionManager.RequestNavigate("ContentRegionShell", "J029500", new NavigationParameters());
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath == null) return;
            _regionManager.RequestNavigate("ContentRegionShell", navigatePath, new NavigationParameters());
        }
    }
}
