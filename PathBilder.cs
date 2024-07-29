using System.Drawing;
using System.Drawing.Drawing2D;

namespace DockPanelControler
{
    internal static class PathBuilder
    {
        public static RectangleF GetRecrangleFFromSize(Size size)
        {
            RectangleF rectangleF = new RectangleF(0, 0, size.Width, size.Height);
            return rectangleF;
        }

        public static GraphicsPath GetRoundRectanglePath(RectangleF rect, int topRadius, int bottomRadius, int width)
        {
            rect = new RectangleF(rect.X + width / 2, rect.Y + width / 2, rect.Width - width, rect.Height - width);

            GraphicsPath graphicsPath = GetRoundRectanglePath(rect, topRadius, bottomRadius);

            return graphicsPath;
        }

        public static GraphicsPath GetRoundRectanglePath(RectangleF rect, int topRadius, int bottomRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (topRadius > 0)
            {
                RectangleF arcLeft = new RectangleF(rect.Left, rect.Top, topRadius * 2, topRadius * 2);
                path.AddArc(arcLeft, 180, 90);

                RectangleF arcRight = new RectangleF(rect.Right - topRadius * 2, rect.Top, topRadius * 2, topRadius * 2);
                path.AddArc(arcRight, 270, 90);
            }
            else
            {
                path.AddLine(rect.Left, rect.Top, rect.Right, rect.Top);
                path.AddLine(rect.Right, rect.Top, rect.Right, rect.Top);
            }

            if (bottomRadius > 0)
            {
                RectangleF arcRight = new RectangleF(rect.Right - bottomRadius * 2, rect.Bottom - bottomRadius * 2, bottomRadius * 2, bottomRadius * 2);
                path.AddArc(arcRight, 0, 90);

                RectangleF arcLeft = new RectangleF(rect.Left, rect.Bottom - bottomRadius * 2, bottomRadius * 2, bottomRadius * 2);
                path.AddArc(arcLeft, 90, 90);
            }
            else
            {
                path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);
                path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Bottom);
            }

            path.CloseFigure();

            return path;
        }
    }
}