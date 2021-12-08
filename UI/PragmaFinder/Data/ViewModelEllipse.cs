using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
using System.Windows.Media;

namespace MyMod.UI.PragmaFinder.Data
{
    public class ViewModelEllipse : BaseViewModel
    {
        private int xTransform;

        public int XTransform
        {
            get => xTransform;
            set
            {
                this.xTransform = value;
                this.NotifyPropertyChanged("XTransform");
            }
        }

        private int yTransform;

        public int YTransform
        {
            get => yTransform;
            set
            {
                this.yTransform = value;
                this.NotifyPropertyChanged("YTransform");
            }
        }

        private double left;

        public double Left
        {
            get => left;
            set
            {
                this.left = value;
                this.NotifyPropertyChanged("Left");
            }
        }

        private double top;

        public double Top
        {
            get => top;
            set
            {
                this.top = value;
                this.NotifyPropertyChanged("Top");
            }
        }

        private double width;

        public double Width
        {
            get => width;
            set
            {
                this.width = value;
                this.NotifyPropertyChanged("Width");
            }
        }

        private double height;

        public double Height
        {
            get => height;
            set
            {
                this.height = value;
                this.NotifyPropertyChanged("Height");
            }
        }

        private double thickness;

        public double Thickness
        {
            get => thickness;
            set
            {
                this.thickness = value;
                this.NotifyPropertyChanged("Thickness");
            }
        }

        private Color color;

        public Color Color
        {
            get => color;
            set
            {
                this.color = value;
                this.NotifyPropertyChanged("Color");
            }
        }

        public override string ToString()
        {
            return $"Ellipse({left},{top},{width},{height}, thickness:{thickness}, color:{color}, transform: {XTransform}, {YTransform})";
        }
    }
}
