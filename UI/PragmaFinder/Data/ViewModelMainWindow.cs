namespace MyMod.UI.PragmaFinder.Data
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Helpers;
    using AtomicTorch.GameEngine.Common.Primitives;

    public class ViewModelMainWindow : BaseViewModel
    {
        public ObservableCollection<ViewModelEllipse> VisibleEllipses { get; set; }
        public ObservableCollection<ViewModelEllipse> HiddenEllipses { get; set; }

        public static ViewModelMainWindow Instance;

        public ViewModelMainWindow()
        {
            //AllSettings = new ObservableCollection<ViewModelSettings>(
            //AutomatonManager.GetAllSettings().Select(s => new ViewModelSettings(s)));
            //AutomatonManager.IsEnabledChanged += OnIsEnabledChanged;

            VisibleEllipses = new ObservableCollection<ViewModelEllipse>();
            HiddenEllipses = new ObservableCollection<ViewModelEllipse>();

            Pong(10000, 10000, 1.5);
            Pong(10020, 10000, 1.2);
            Pong(10040, 10000, 0.9);
            Pong(10060, 10000, 0.6);
            Pong(10080, 10000, 0.3);

            //HiddenEllipses.Add(new ViewModelEllipse()
            //{
            //    Left = 0,
            //    Top = 0,
            //    Width = 300,
            //    Height = 300,
            //    Color = Color.FromArgb(255,255,255,255),
            //    Thickness = 20,
            //    XTransform = 0,
            //    YTransform = 0
            //});

            Instance = this;
        }

        public void Reset()
        {
            foreach (var e in VisibleEllipses)
            {
                e.Dispose();
            }

            foreach (var e in HiddenEllipses)
            {
                e.Dispose();
            }

            VisibleEllipses.Clear();
            HiddenEllipses.Clear();
        }

        public void UpdatePosition(double x, double y)
        {
            foreach (var e in VisibleEllipses)
            {
                e.XTransform = (int)(-x + 100);
                e.YTransform = (int)(y + 100);
            }

            foreach (var e in HiddenEllipses)
            {
                e.XTransform = (int)(-x + 100);
                e.YTransform = (int)(y + 100);
            }
        }

        public void Pong(double x, double y, double timeSincePing)
        {
            Color color = Color.FromArgb(255, 255, 255, 255); // not used, can't get binding to work with value converter
            Color hiddenColor = Color.FromArgb(255, 0, 0, 0); // not used, can't get binding to work with value converter

            // special when no pong received, maximum circle
            if (timeSincePing == 0)
            {
                double radius = 100;

                HiddenEllipses.Add(new ViewModelEllipse()
                {
                    Left = x - radius,
                    Top = -y - radius,
                    Width = radius * 2,
                    Height = radius * 2,
                    Color = hiddenColor,
                    Thickness = radius,
                    XTransform = (int)-x + 100,
                    YTransform = (int)y + 100
                });

                //UpdatePosition(x, y);

                return;
            }

            int stage = (int)(Math.Round(timeSincePing / 0.3));
            int outerRadius = stage * 20;
            int innerRadius = outerRadius - 20;
            int superOuterRadius = outerRadius + 60;

            switch (stage)
            {
                case 1:
                    color = Color.FromArgb(128, 0, 255, 0); // green inner
                    break;
                case 2:
                    color = Color.FromArgb(128, 0, 0, 255); // blue inner
                    break;
                case 3:
                    color = Color.FromArgb(128, 255, 0, 0); // red inner
                    break;
                case 4:
                    color = Color.FromArgb(128, 192, 128, 0); // ?? 
                    break;
                case 5:
                    color = Color.FromArgb(128, 0, 192, 128); // ?? 
                    break;
                default:
                    color = Color.FromArgb(255, 255, 255, 0);
                    break;
            }

            VisibleEllipses.Add(new ViewModelEllipse()
            {
                Left = x - outerRadius,
                Top = -y - outerRadius,
                Width = outerRadius * 2,
                Height = outerRadius * 2,
                Color = color,
                Thickness = 20,
                XTransform = (int)-x + 100,
                YTransform = (int)y + 100
            });

            HiddenEllipses.Add(new ViewModelEllipse()
            {
                Left = x - innerRadius,
                Top = -y - innerRadius,
                Width = innerRadius * 2,
                Height = innerRadius * 2,
                Color = hiddenColor,
                Thickness = innerRadius - 2,
                XTransform = (int)-x + 100,
                YTransform = (int)y + 100
            });


            HiddenEllipses.Add(new ViewModelEllipse()
            {
                Left = x - superOuterRadius,
                Top = -y - superOuterRadius,
                Width = superOuterRadius * 2,
                Height = superOuterRadius * 2,
                Color = hiddenColor,
                Thickness = 60 - 2,
                XTransform = (int)-x + 100,
                YTransform = (int)y + 100
            });

            //UpdatePosition(x, y);
        }

        //private void OnIsEnabledChanged()
        //{
        //    // NotifyPropertyChanged(nameof(IsEnabled));
        //}

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();

            foreach (var e in VisibleEllipses)
            {
                e.Dispose();
            }

            foreach (var e in HiddenEllipses)
            {
                e.Dispose();
            }
            //foreach (var viewModelSettings in AllSettings)
            //{
            //    viewModelSettings.Dispose();
            //}

            //AutomatonManager.IsEnabledChanged -= OnIsEnabledChanged;
        }
    }
}