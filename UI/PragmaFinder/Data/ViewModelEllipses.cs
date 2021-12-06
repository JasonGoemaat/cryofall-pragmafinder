using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
using AtomicTorch.GameEngine.Common.Primitives;
using System.Windows.Media;

namespace MyMod.UI.PragmaFinder.Data
{
    public class ViewModelEllipses : BaseViewModel
    {
        public Vector2D Position { get; set; }

        public double Radius { get; set; }

        public Color Color { get; set; }

        public ViewModelEllipses(Vector2D position, double radius, Color color)
        {
            this.Position = position;
            this.Radius = radius;
            this.Color = color;
        }
    }
}
