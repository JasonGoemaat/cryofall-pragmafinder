namespace MyMod.UI.PragmaFinder.Data
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.Helpers.Client;
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
                if (value == ModeEnum.Generic)
                {
                    // this.SetTestGeometry();
                    Rectangles.Clear();
                }
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
        public ObservableCollection<ViewModelRectangle> Rectangles { get; set; }

        public ViewModelHUD()
        {
            VisibleEllipses = new ObservableCollection<ViewModelEllipse>();
            HiddenEllipses = new ObservableCollection<ViewModelEllipse>();
            Rectangles = new ObservableCollection<ViewModelRectangle>();

            ClearCommand = new ActionCommand(() => this.Clear());
            PragmiumCommand = new ActionCommand(() => this.Mode = ModeEnum.Pragmium);
            GenericCommand = new ActionCommand(() => this.Mode = ModeEnum.Generic);
            ToggleModeCommand = new ActionCommand(() =>
            {
                this.Mode = (this.Mode == ModeEnum.Pragmium) ? ModeEnum.Generic : ModeEnum.Pragmium;
                Api.Logger.Warning($"Mode changed to {this.Mode}");
            });
        }

        #region Pragma
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
            Rectangles.Clear();
        }

        public void UpdatePosition(double x, double y)
        {
            Api.Logger.Warning($"ViewModelHUD.UpdatePosition({x}, {y})");
            centerX = x;
            centerY = y;

            var tempx = (int)(-centerX + 100);
            var tempy = (int)(centerY + 100);
            this.XTransform = tempx;
            this.YTransform = tempy;

            if (this.mode == ModeEnum.Generic)
            {
                var px = x;
                var py = -y;

                //var newRect = new RectangleGeometry(new Rect(new Point(px - 14, py - 14), new Point(px + 14, py + 14)));
                var rect = new ViewModelRectangle(px - 14, py - 14, 29, 29, this.XTransform, this.YTransform);
                Rectangles.Add(rect);
                Api.Logger.Important($"Rectangles: ({Rectangles.Count}), last Left: {rect.Left}, Top: {rect.Top}, Width: {rect.Width}, Height: {rect.Height}, transform {rect.XTransform},{rect.YTransform}");
                var f = Rectangles[0];
                Api.Logger.Important($"Rectangles: ({Rectangles.Count}), first Left: {f.Left}, Top: {f.Top}, Width: {f.Width}, Height: {f.Height}, transform {f.XTransform},{f.YTransform}");

                foreach (var r in Rectangles)
                {
                    r.XTransform = this.XTransform;
                    r.YTransform = this.YTransform;
                }
                
                return;
            }
            foreach (var e in VisibleEllipses)
            {
                e.XTransform = this.XTransform;
                e.YTransform = this.YTransform;
            }

            foreach (var e in HiddenEllipses)
            {
                e.XTransform = this.XTransform;
                e.YTransform = this.YTransform;
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

        #endregion

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

        #region Generic
        // this is for combining boxes as the player moves

        int xTransform;

        public int XTransform
        {
            get => xTransform;

            set
            {
                xTransform = value;
                this.NotifyPropertyChanged("XTransform");
            }
        }

        int yTransform;

        public int YTransform
        {
            get => yTransform;

            set
            {
                yTransform = value;
                this.NotifyPropertyChanged("YTransform");
            }
        }

        Geometry genericGeometry;

        public Geometry GenericGeometry
        {
            get => genericGeometry;
            set
            {
                this.genericGeometry = value;
                this.NotifyThisPropertyChanged("GenericGeometry");
            }
        }

        public void ResetGenericGeometry()
        {
            // NOT USING in favor of rectangles list
            //this.genericGeometry = new RectangleGeometry();
        }

        public void AddPlayerPositionToGenericGeometry(double x, double y)
        {
            //var px = Math.Round(x);
            //var py = -Math.Round(y);

            //var newRec = new RectangleGeometry(new Rect(new Point(px - 14, py - 14), new Point(px + 14, py + 14)));
            ////this.GenericGeometry = newRec;
            
            //// ERRORS - Trying to use Noesis.GeometryCombineMode and Noesis.Geometry which have different constructor argument positions
            //var combined = new System.Windows.Media.CombinedGeometry(
            //    GeometryCombineMode.Union, this.genericGeometry, newRec);
            //this.GenericGeometry = combined;
        }
          
        public void SetTestGeometry()
        {
            //this.GenericGeometry = new RectangleGeometry(new Rect(new Point(100, 100), new Point(150, 150)));
            //return;

            var player = ClientCurrentCharacterHelper.Character;
            var x = Math.Round(player.Position.X);
            var y = -Math.Round(player.Position.Y);

            var newRec = new RectangleGeometry(new Rect(new Point(x - 14, y - 14), new Point(x + 14, y + 14)));

            Api.Logger.Warning($"GenericGeometry: {newRec.Bounds.Left}, {newRec.Bounds.Top}, width {newRec.Bounds.Width}, height {newRec.Bounds.Height}");
            this.GenericGeometry = newRec;
        }
        #endregion
    }
}