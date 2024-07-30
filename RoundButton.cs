using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DockPanelControler
{
    public class RoundButton : Button
    {
        private bool _mouseEnter;
        private bool _mouseDown;

        public RoundButton()
        {
            DoubleBuffered = true;
        }

        #region Свойства

        public float OutlineWidth { get; set; } = 3f;

        public Color OutlineColor { get; set; } = Color.DarkGray;

        public Color OutlineColorOnMouseEnter { get; set; } = Color.Gray;

        public Color OutlineColorOnClick { get; set; } = Color.DimGray;

        public Color BackColorOnMouseEnter { get; set; } = Color.FromArgb(224, 224, 224);

        public Color BackColorOnClick { get; set; } = Color.FromArgb(200, 200, 200);

        public int TopRadius { get; set; } = 5;

        public int BottomRadius { get; set; } = 5;

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            _mouseEnter = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _mouseEnter = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _mouseDown = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _mouseDown = false;
            Invalidate();
        }

        public void Draw(Graphics graphics)
        {
            var outlineColor = _mouseDown ?
                OutlineColorOnClick :
                _mouseEnter ? OutlineColorOnMouseEnter : OutlineColor;
            var backColor = _mouseDown ?
                BackColorOnClick :
                _mouseEnter ? BackColorOnMouseEnter : BackColor;

            graphics.Clear(backColor);

            var rectangle = PathBuilder.GetRecrangleFFromSize(Size);
            var graphicsPath = PathBuilder.GetRoundRectanglePath(rectangle, TopRadius, BottomRadius);

            Region = new Region(graphicsPath);

            if (OutlineWidth != 0)
            {
                using (Pen pen = new Pen(outlineColor, OutlineWidth))
                {
                    pen.Alignment = PenAlignment.Inset;
                    graphics.DrawPath(pen, graphicsPath);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw(e.Graphics);
        }
    }
}
