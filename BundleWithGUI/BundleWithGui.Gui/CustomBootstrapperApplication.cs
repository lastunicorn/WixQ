using System;
using System.Windows.Threading;
using BundleWithGui.Gui.ViewModels;
using BundleWithGui.Gui.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace BundleWithGui.Gui
{
    public class CustomBootstrapperApplication : BootstrapperApplication
    {
        private static Dispatcher bootstrapperDispatcher;

        protected override void Run()
        {
            Engine.Log(LogLevel.Verbose, "Launching custom Bootstrapper Application UX");

            bootstrapperDispatcher = Dispatcher.CurrentDispatcher;

            MainViewModel viewModel = new MainViewModel(this);

            MainWindow view = new MainWindow { DataContext = viewModel };
            view.Closed += HandleViewClosed;

            Engine.Detect();
            view.Show();

            Dispatcher.Run();

            Engine.Quit(0);
        }

        private static void HandleViewClosed(object sender, EventArgs e)
        {
            bootstrapperDispatcher.InvokeShutdown();
        }

        public void InvokeShutDown()
        {
            bootstrapperDispatcher.InvokeShutdown();
        }

        protected override void OnPlanComplete(PlanCompleteEventArgs args)
        {
            base.OnPlanComplete(args);

            if (args.Status >= 0)
                Engine.Apply(System.IntPtr.Zero);
        }
    }
}