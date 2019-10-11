using System.Windows;
using ConvertorToXmlByM.E.Doc_v._2._0.Views;
using Prism.Ioc;
using Prism.Unity;

namespace ConvertorToXmlByM.E.Doc_v._2._0
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
