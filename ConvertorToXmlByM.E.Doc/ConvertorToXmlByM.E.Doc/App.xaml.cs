using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using J029500 = ConvertorToXmlByM.E.Doc.Views.J029500;
using J029540 = ConvertorToXmlByM.E.Doc.Views.J029540;
using Shell = ConvertorToXmlByM.E.Doc.Views.Shell;

namespace ConvertorToXmlByM.E.Doc
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<J029540>();
            containerRegistry.RegisterForNavigation<J029500>();
        }

        protected override Window CreateShell()
        {
            return new Shell();
        }
    }
}
