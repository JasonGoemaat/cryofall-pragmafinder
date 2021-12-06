namespace MyMod.UI.PragmaFinder.Data
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;

    public class ViewModelMainWindow : BaseViewModel
    {
        public ObservableCollection<ViewModelEllipses> Ellipses { get; }

        public ViewModelMainWindow()
        {
            //AllSettings = new ObservableCollection<ViewModelSettings>(
            //AutomatonManager.GetAllSettings().Select(s => new ViewModelSettings(s)));
            //AutomatonManager.IsEnabledChanged += OnIsEnabledChanged;

            Ellipses = new ObservableCollection<ViewModelEllipses>();
        }

        //private void OnIsEnabledChanged()
        //{
        //    // NotifyPropertyChanged(nameof(IsEnabled));
        //}

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();

            //foreach (var viewModelSettings in AllSettings)
            //{
            //    viewModelSettings.Dispose();
            //}

            //AutomatonManager.IsEnabledChanged -= OnIsEnabledChanged;
        }
    }
}