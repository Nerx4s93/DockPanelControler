using System;
using System.Drawing;
using System.Windows.Forms;

using Svg;

namespace DockPanelControler
{
    internal class SvgButton : ButtonFlatBase
    {
        private string _svgName;
        private SvgDocument _svgDocument;

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

        public Color SvgDeffaultColor { get; set; } = Color.White;

        public Color SvgColorOnMouseEnter { get; set; } = Color.FromArgb(255, 255, 192);

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            if (_svgDocument != null)
            {
                SvgController.ChangeFillColor(_svgDocument, SvgColorOnMouseEnter);
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_svgDocument != null)
            {
                SvgController.ChangeFillColor(_svgDocument, SvgDeffaultColor);
            }
            base.OnMouseEnter(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;

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
    }
}
