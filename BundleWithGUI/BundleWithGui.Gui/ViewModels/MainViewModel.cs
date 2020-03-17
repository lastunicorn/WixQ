using System;
using System.Windows.Threading;
using BundleWithGui.Gui.Commands;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace BundleWithGui.Gui.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private static Dispatcher dispatcher;
        private readonly CustomBootstrapperApplication bootstrapperApplication;

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public InstallCommand InstallCommand { get; }

        public UninstallCommand UninstallCommand { get; }

        public ExitCommand ExitCommand { get; }

        public MainViewModel(CustomBootstrapperApplication bootstrapperApplication)
        {
            this.bootstrapperApplication = bootstrapperApplication ?? throw new ArgumentNullException(nameof(bootstrapperApplication));

            dispatcher = Dispatcher.CurrentDispatcher;

            InstallCommand = new InstallCommand(bootstrapperApplication);
            UninstallCommand = new UninstallCommand(bootstrapperApplication);
            ExitCommand = new ExitCommand(bootstrapperApplication);

            this.bootstrapperApplication.PlanBegin += HandlePlanBegin;
            this.bootstrapperApplication.ApplyComplete += HandleApplyComplete;
        }

        private void HandlePlanBegin(object sender, PlanBeginEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                IsLoading = true;
            });
        }

        private void HandleApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                IsLoading = false;
            });
        }
    }
}