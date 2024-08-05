using System;
using System.Drawing;
using System.Windows.Forms;

namespace DockPanelControler
{
    public class ButtonFlatBase : Control
    {
        private bool _mouseEnter;
        private bool _mouseDown;

        public ButtonFlatBase()
        {
            DoubleBuffered = true;
            BackColor = Color.Gainsboro;
        }

        #region Свойства

        public Color BackColorOnMouseEnter { get; set; } = DeffaultPropertyValues.ButtonFlatBaseBackColorOnMouseEnter;

        public Color BackColorOnMouseDown { get; set; } = DeffaultPropertyValues.ButtonFlatBaseBackColorOnMouseDown;

        #endregion

        protected override void OnMouseEnter(EventArgs eventArgs)
        {
            _mouseEnter = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs eventArgs)
        {
            _mouseEnter = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mouseEventArgs)
        {
            _mouseDown = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mouseEventArgs)
        {
            _mouseDown = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            var graphics = e.Graphics;

            var backColor = _mouseDown ?
                BackColorOnMouseDown :
                _mouseEnter ? BackColorOnMouseEnter : BackColor;

            using (var brush = new SolidBrush(backColor))
            {
                graphics.FillRectangle(brush, 0, 0, Size.Width, Size.Height);
            }
        }
    }
}
