namespace MyMod.UI.PragmaFinder
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using MyMod.UI.PragmaFinder.Data;
    using System.Windows;

    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : BaseUserControlWithWindow
    {
        public static MainWindow Instance { get; private set; }

        public ViewModelMainWindow ViewModel { get; set; }

        protected override void WindowOpening()
        {
            Api.Logger.Warning("MainWindow - WindowOpening()!");
        }

        public static void Toggle()
        {
            if (Instance?.IsOpened == true)
            {
                Instance.CloseWindow();
            }
            else
            {
                if (Instance == null)
                {
                    var instance = new MainWindow();
                    Instance = instance;
                    Api.Client.UI.LayoutRootChildren.Add(instance);
                }
                else
                {
                    Instance.Window.Open();
                }
            }
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            Window.IsCached = true;
            
            if (DataContext == null)
            {
                Api.Logger.Warning("DataContext is null");
                DataContext = ViewModel = new ViewModelMainWindow();
            }
            else
            {
                Api.Logger.Warning("DataContext is NOT null");
                ViewModel = DataContext as ViewModelMainWindow;
                foreach (var e in ViewModel.VisibleEllipses)
                {
                    Api.Logger.Important($"VisibleEllipse({e.Left},{e.Top},{e.Width},{e.Height},{e.Thickness}) - {e.XTransform},{e.YTransform},RGBA({e.Color.A},{e.Color.R},{e.Color.G},{e.Color.B})");
                }
                foreach (var e in ViewModel.HiddenEllipses)
                {
                    Api.Logger.Important($"HiddenEllipse({e.Left},{e.Top},{e.Width},{e.Height},{e.Thickness}) - {e.XTransform},{e.YTransform},RGBA({e.Color.A},{e.Color.R},{e.Color.G},{e.Color.B})");
                }
            }
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            DataContext = null;
            ViewModel = null;
            if (Instance == this)
            {
                Instance = null;
            }
        }

        ///////  This isn't valid?
        //protected override void OnGotFocus(RoutedEventArgs e)
        //{
        //    base.OnGotFocus(e);

        //    Api.Logger.Warning("MainWindow got focus!");
        //}
    }
}