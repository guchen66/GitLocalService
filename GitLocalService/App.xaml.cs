using GitLocalService.Models;
using GitLocalService.Services;
using GitLocalService.Views;
using Prism.Ioc;
using System.Windows;

namespace GitLocalService
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            // 启动时从 JSON 加载配置，填充到已注册的单例中
            var config = Container.Resolve<ServiceConfig>();
            var loaded = JsonProvider.LoadConfig();

            config.AcceptLicense = loaded.AcceptLicense;
            config.RegistryRightKey = loaded.RegistryRightKey;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IWizardService, WizardService>();
            containerRegistry.RegisterSingleton<ServiceConfig>();
            containerRegistry.RegisterForNavigation<WelcomeView>();
            containerRegistry.RegisterForNavigation<LicenseView>();
            containerRegistry.RegisterForNavigation<DestinationView>();
            containerRegistry.RegisterForNavigation<ComponentsView>();
            containerRegistry.RegisterForNavigation<StartMenuView>();
            containerRegistry.RegisterForNavigation<EditorView>();
            containerRegistry.RegisterForNavigation<BranchView>();
            containerRegistry.RegisterForNavigation<PathView>();
            containerRegistry.RegisterForNavigation<SshView>();
            containerRegistry.RegisterForNavigation<HttpsView>();
            containerRegistry.RegisterForNavigation<LineEndingView>();
            containerRegistry.RegisterForNavigation<TerminalView>();
            containerRegistry.RegisterForNavigation<PullView>();
            containerRegistry.RegisterForNavigation<ExtraOptionsView>();
            containerRegistry.RegisterForNavigation<ReadyView>();
            containerRegistry.RegisterForNavigation<InstallingView>();
            containerRegistry.RegisterForNavigation<FinishedView>();
        }
    }
}