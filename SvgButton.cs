using System;
using System.Drawing;
using System.Windows.Forms;

using Svg;

namespace DockPanelControler
{
    public class SvgButton : Control
    {
        private string _svgName;
        private SvgDocument _svgDocument;

        private bool _mouseEnter;
        private bool _mouseDown;

        public SvgButton()
        {
            DoubleBuffered = true;
        }

        #region Свойства

        public string SvgName
        {
            get
            {
                return _svgName;
            }
            set
            {
                _svgName = value;

                if (string.IsNullOrEmpty(_svgName))
                {
                    _svgDocument = null;
                    return;
                }

                _svgDocument = SvgController.GetSvgDocumentFromResourcesName(_svgName);
                SvgController.ChangeFillColor(_svgDocument, SvgDeffaultColor);

                Invalidate();
            }
        }

        public Color BackColorOnMouseEnter { get; set; } = Color.FromArgb(210, 210, 210);

        public Color BackColorOnMouseDown { get; set; } = Color.FromArgb(200, 200, 200);

        public Color SvgDeffaultColor { get; set; } = Color.White;

        public Color SvgColorOnMouseEnter { get; set; } = Color.FromArgb(255, 255, 192);

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            _mouseEnter = true;
            if (_svgDocument != null)
            {
                SvgController.ChangeFillColor(_svgDocument, SvgColorOnMouseEnter);
            }
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _mouseEnter = false;
            if (_svgDocument != null)
            {
                SvgController.ChangeFillColor(_svgDocument, SvgDeffaultColor);
            }
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

        private void DrawFon(Graphics graphics)
        {
            var backColor = _mouseDown ?
                BackColorOnMouseDown :
                _mouseEnter ? BackColorOnMouseEnter : BackColor;

            using (var brush = new SolidBrush(backColor))
            {
                graphics.FillRectangle(brush, 0, 0, Size.Width, Size.Height);
            }
        }

        private void DrawSvg(Graphics graphics)
        {
            if (_svgDocument != null)
            {
                if (DesignMode)
                {
                    SvgController.ChangeFillColor(_svgDocument, SvgDeffaultColor);
                }

                var svgBitmap = SvgController.GetBitmapFromSvgDocument(_svgDocument, Size);
                graphics.DrawImage(svgBitmap, 0, 0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            DrawFon(graphics);
            DrawSvg(graphics);
        }
    }
}
