using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace DockPanelControler
{
    public class RoundButton : Button
    {
        public RoundButton()
        {
            DoubleBuffered = true;
        }

        #region Свойства

        public float OutlineWidth { get; set; } = 2f;

        public Color OutlineColor { get; set; } = Color.DarkGray;

        public int TopRadius { get; set; } = 5;

        public int BottomRadius { get; set; } = 5;

        #endregion

        public void DrawPath(Graphics graphics)
        {
            var rectangle = PathBuilder.GetRecrangleFFromSize(Size);
            var graphicsPath = PathBuilder.GetRoundRectanglePath(rectangle, TopRadius, BottomRadius);

            Region = new Region(graphicsPath);

            if (OutlineWidth != 0)
            {
                using (Pen pen = new Pen(OutlineColor, OutlineWidth))
                {
                    pen.Alignment = PenAlignment.Inset;
                    graphics.DrawPath(pen, graphicsPath);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawPath(e.Graphics);
        }
    }
}
