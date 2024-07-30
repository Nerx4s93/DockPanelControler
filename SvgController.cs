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

            byte[] resourceBytes = (byte[])resourceManager.GetObject(name);

            if (resourceBytes != null)
            {
                using (var stream = new MemoryStream(resourceBytes))
                {
                    var svgDocument = SvgDocument.Open<SvgDocument>(stream);
                    return svgDocument;
                }
            }

            return null;
        }

        public static Bitmap GetBitmapFromSvgDocument(SvgDocument svgDocument, Size size)
        {
            svgDocument.Width = size.Width;
            svgDocument.Height = size.Height;

            var bitmap = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                svgDocument.Draw(g);
            }

            return bitmap;
        }

        public static void ChangeFillColor(SvgDocument svgDocument, Color fillColor)
        {
            foreach (var element in svgDocument.Descendants())
            {
                if (element is SvgPath svgPath)
                {
                    svgPath.Fill = new SvgColourServer(fillColor);
                }
            }
        }
    }
}
