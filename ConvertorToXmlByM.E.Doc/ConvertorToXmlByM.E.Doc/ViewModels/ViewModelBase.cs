using Prism.Mvvm;
using Prism.Regions;

namespace ConvertorToXmlByM.E.Doc.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware
    {
        #region INavigationAware

        public abstract void OnNavigatedTo(NavigationContext navigationContext);

        public abstract bool IsNavigationTarget(NavigationContext navigationContext);

        public abstract void OnNavigatedFrom(NavigationContext navigationContext);

        #endregion
    }
}