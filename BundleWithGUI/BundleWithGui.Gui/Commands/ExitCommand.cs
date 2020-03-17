using System;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace BundleWithGui.Gui.Commands
{
    internal class ExitCommand : ICommand
    {
        private static Dispatcher dispatcher;
        private readonly CustomBootstrapperApplication bootstrapperApplication;
        private bool canExecute = true;

        public event EventHandler CanExecuteChanged;

        public ExitCommand(CustomBootstrapperApplication bootstrapperApplication)
        {
            this.bootstrapperApplication = bootstrapperApplication ?? throw new ArgumentNullException(nameof(bootstrapperApplication));

            dispatcher = Dispatcher.CurrentDispatcher;

            this.bootstrapperApplication.PlanBegin += HandlePlanBegin;
            this.bootstrapperApplication.ApplyComplete += HandleApplyComplete;
        }

        private void HandlePlanBegin(object sender, PlanBeginEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                canExecute = false;
                OnCanExecuteChanged();
            });
        }

        private void HandleApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                canExecute = true;
                OnCanExecuteChanged();
            });
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            bootstrapperApplication.InvokeShutDown();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}