using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MyMod.UI.PragmaFinder.Data
{
    public class ViewModelRectangle : BaseViewModel
    {
        public ViewModelRectangle(double left, double top, double width, double height, double xTransform, double yTransform)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.xTransform = (int)xTransform;
            this.yTransform = (int)yTransform;
            this.color = Color.FromArgb(255, 255, 0, 0); // red, but cannot bind so ignored
        }

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
            return $"Rectangle({left},{top},{width},{height}, color:{color}, transform: {XTransform}, {YTransform})";
        }
    }
}
