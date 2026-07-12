﻿﻿﻿﻿using GitLocalService.Services;
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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IWizardService, WizardService>();

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