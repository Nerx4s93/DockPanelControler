using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;

using Svg;

namespace DockPanelControler
{
    internal static class SvgController
    {
        public static SvgDocument GetSvgDocumentFromResourcesName(string name)
        {
            var resourceManager = new ResourceManager(
                "DockPanelControler.Properties.Resources",
                Assembly.GetCallingAssembly());

            var resourceBytes = (byte[])resourceManager.GetObject(name);

            if (resourceBytes == null)
            {
                throw new ArgumentException($"SVG с именем '{name}' не найден.", nameof(name));
            }
            using (var memoryStream = new MemoryStream(resourceBytes))
            {
                var svgDocument = SvgDocument.Open<SvgDocument>(memoryStream);
                return svgDocument;
            }
        }

        public static Bitmap GetBitmapFromSvgDocument(SvgDocument svgDocument, Size size)
        {
            svgDocument.Width = size.Width;
            svgDocument.Height = size.Height;

            var bitmap = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                svgDocument.Draw(graphics);
            }

            return bitmap;
        }

        public static void ChangeFillColor(SvgDocument svgDocument, Color fillColor)
        {
            var svgElements = svgDocument.Descendants();

            foreach (var svgElement in svgElements)
            {
                if (svgElement is SvgPath svgPath)
                {
                    svgPath.Fill = new SvgColourServer(fillColor);
                }
            }
        }
    }
}
