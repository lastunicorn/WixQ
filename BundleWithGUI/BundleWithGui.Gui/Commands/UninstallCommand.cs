using System;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace BundleWithGui.Gui.Commands
{
    internal class UninstallCommand : ICommand
    {
        private static Dispatcher dispatcher;
        private readonly CustomBootstrapperApplication bootstrapperApplication;
        private bool canExecute;

        public event EventHandler CanExecuteChanged;

        public UninstallCommand(CustomBootstrapperApplication bootstrapperApplication)
        {
            this.bootstrapperApplication = bootstrapperApplication ?? throw new ArgumentNullException(nameof(bootstrapperApplication));

            dispatcher = Dispatcher.CurrentDispatcher;

            this.bootstrapperApplication.PlanBegin += HandlePlanBegin;
            this.bootstrapperApplication.DetectPackageComplete += HandleDetectPackageComplete;
        }

        private void HandlePlanBegin(object sender, PlanBeginEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                canExecute = false;
                OnCanExecuteChanged();
            });
        }

        private void HandleDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                if (e.State == PackageState.Present)
                {
                    canExecute = true;
                    OnCanExecuteChanged();
                }
            });
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            bootstrapperApplication.Engine.Plan(LaunchAction.Uninstall);
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}