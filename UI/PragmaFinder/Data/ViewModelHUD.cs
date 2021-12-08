namespace MyMod.UI.PragmaFinder.Data
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public class ViewModelHUD : BaseViewModel
    {
        public enum ModeEnum
        {
            Pragmium = 0,
            Generic = 1
        }

        ModeEnum mode = ModeEnum.Pragmium;

        public ModeEnum Mode
        {
            get => mode;
            set
            {
                mode = value;
                this.NotifyPropertyChanged("Mode");
                this.NotifyPropertyChanged("IsModePragmium");
                this.NotifyPropertyChanged("IsModeGeneric");
            }
        }

        public bool IsModePragmium => Mode == ModeEnum.Pragmium;

        public bool IsModeGeneric => Mode == ModeEnum.Generic;

        double centerX, centerY;

        bool isHUDVisible = true;

        public bool IsHUDVisible
        {
            get => isHUDVisible;

            set
            {
                isHUDVisible = value;
                this.NotifyPropertyChanged("IsHUDVisible");
            }
        }

        public ObservableCollection<ViewModelEllipse> VisibleEllipses { get; set; }
        public ObservableCollection<ViewModelEllipse> HiddenEllipses { get; set; }

        public ViewModelHUD()
        {
            VisibleEllipses = new ObservableCollection<ViewModelEllipse>();
            HiddenEllipses = new ObservableCollection<ViewModelEllipse>();

            ClearCommand = new ActionCommand(() => this.Clear());
            PragmiumCommand = new ActionCommand(() => this.Mode = ModeEnum.Pragmium);
            GenericCommand = new ActionCommand(() => this.Mode = ModeEnum.Generic);
            ToggleModeCommand = new ActionCommand(() =>
            {
                this.Mode = (this.Mode == ModeEnum.Pragmium) ? ModeEnum.Generic : ModeEnum.Pragmium;
                Api.Logger.Warning($"Mode changed to {this.Mode}");
            });
        }

        public void Clear()
        {
            Api.Logger.Warning("ViewModelHUD.Clear()");

            foreach (var e in VisibleEllipses)
            {
                Api.Logger.Warning($"Visible: {e}");

                e.Dispose();
            }

            foreach (var e in HiddenEllipses)
            {
                Api.Logger.Warning($"Hidden: {e}");

                e.Dispose();
            }

            VisibleEllipses.Clear();
            HiddenEllipses.Clear();
        }

        public void UpdatePosition(double x, double y)
        {
            Api.Logger.Warning($"ViewModelHUD.UpdatePosition({x}, {y})");
            centerX = x;
            centerY = y;

            foreach (var e in VisibleEllipses)
            {
                e.XTransform = (int)(-centerX + 100);
                e.YTransform = (int)(centerY + 100);
            }

            foreach (var e in HiddenEllipses)
            {
                e.XTransform = (int)(-centerX + 100);
                e.YTransform = (int)(centerY + 100);
            }
        }

        public void Pong(double x, double y, double timeSincePing)
        {
            Api.Logger.Warning($"ViewModelHUD.Pong({x}, {y}, {timeSincePing})");

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
                    XTransform = (int)-centerX + 100,
                    YTransform = (int)centerY + 100
                });

                //UpdatePosition(x, y);

                int count = HiddenEllipses.Count;
                Api.Logger.Important($"Ping with no pong, HiddenEllipses now has {count} elements");

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
                XTransform = (int)-centerX + 100,
                YTransform = (int)centerY + 100
            });

            HiddenEllipses.Add(new ViewModelEllipse()
            {
                Left = x - innerRadius,
                Top = -y - innerRadius,
                Width = innerRadius * 2,
                Height = innerRadius * 2,
                Color = hiddenColor,
                Thickness = innerRadius - 2,
                XTransform = (int)-centerX + 100,
                YTransform = (int)centerY + 100
            });


            HiddenEllipses.Add(new ViewModelEllipse()
            {
                Left = x - superOuterRadius,
                Top = -y - superOuterRadius,
                Width = superOuterRadius * 2,
                Height = superOuterRadius * 2,
                Color = hiddenColor,
                Thickness = 60 - 2,
                XTransform = (int)-centerX + 100,
                YTransform = (int)centerY + 100
            });

            // UpdatePosition(x, y);
        }

        //private void OnIsEnabledChanged()
        //{
        //    // NotifyPropertyChanged(nameof(IsEnabled));
        //}

        protected override void DisposeViewModel()
        {
            Api.Logger.Warning($"ViewModelHUD.DisposeViewModel()");
            
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

        public BaseCommand ClearCommand { get; }

        public BaseCommand PragmiumCommand { get; }

        public BaseCommand GenericCommand { get; }

        public BaseCommand ToggleModeCommand { get; }
    }
}