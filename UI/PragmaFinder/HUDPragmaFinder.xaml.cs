namespace MyMod.UI.PragmaFinder
{
    using System.Windows;
    using System.Windows.Input;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using MyMod.UI.PragmaFinder.Data;

    /// <summary>
    /// Interaction logic for HUDPragmaFinder.xaml
    /// </summary>
    public partial class HUDPragmaFinder : BaseUserControl
    {
        private ViewModelHUD viewModel;

        public static HUDPragmaFinder Instance { get; private set; }

        private bool isDisplayed;

        public HUDPragmaFinder()
        {
            InitializeComponent();
        }

        // from LogOverlayControl
        public bool IsDisplayed
        {
            get => this.isDisplayed;
            set
            {
                if (this.isDisplayed == value)
                {
                    return;
                }

                this.isDisplayed = value;

                this.Visibility = this.isDisplayed ? Visibility.Visible : Visibility.Collapsed;

                if (!this.isDisplayed)
                {
                    // this.Clear();
                }
            }
        }

        // from LogOverlayControl
        protected override void InitControl()
        {
            Instance = this;
            this.IsDisplayed = false;
        }

        // from LogOverlayControl
        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.DataContext = this.viewModel = new ViewModelHUD();
            this.MouseRightButtonUp += this.MouseRightButtonUpHandler;
        }

        // from LogOverlayControl
        private void MouseRightButtonUpHandler(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.IsDisplayed = false;
        }

        public static void Toggle()
        {
            Api.Logger.Warning("HUDPragmaFinder.Toggle()");
            if (Instance is null)
            {
                Api.Logger.Warning("HUDPragmaFinder.Toggle() - instance is null, creating");
                Instance = new HUDPragmaFinder();
                Api.Logger.Warning("HUDPragmaFinder.Toggle() - instance is null, adding to LayoutRootChildren");
                Api.Client.UI.LayoutRootChildren.Add(Instance);
            }

            Api.Logger.Warning($"HUDPragmaFinder.Toggle() - instance is null, adding to LayoutRootChildren changing IsDisplayed from: {Instance.IsDisplayed}");
            Instance.IsDisplayed = !Instance.IsDisplayed;
            Api.Logger.Warning($"HUDPragmaFinder.Toggle() - instance is null, adding to LayoutRootChildren changing IsDisplayed to: {Instance.IsDisplayed}");
        }

        public void Pong(double x, double y, double timeSincePing)
        {
            Api.Logger.Warning($"HUDPragmaFinder.Pong({x}, {y}, {timeSincePing})");
            this.viewModel.Pong(x, y, timeSincePing);
        }

        public void Clear()
        {
            Api.Logger.Error("HUDPragmaFinder.Clear()");
            this.viewModel.Clear();
        }

        public void UpdatePosition(double x, double y)
        {
            this.viewModel.UpdatePosition(x, y);
        }
    }
}
