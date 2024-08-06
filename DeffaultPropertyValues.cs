using System.Drawing;

namespace DockPanelControler
{
    public static class DeffaultPropertyValues
    {
        public static float DockPanelOutlineWidth = 3;
        public static Color DockPanelOutlineColorOnFormMove = Color.Gray;
        public static Color DockPanelOutlineColorOnFormEnter = Color.Red;
        public static Color DockPanelBackColorOnFormMove = Color.Silver;
        public static Color DockPanelTitleBarPanelBackColor = Color.Silver;
        public static Color DockPanelBodyPanelBackColor = Color.FromArgb(224, 224, 224);
        public static int DockPanelAdditionalScope = 100;

        public static Color ButtonFlatBaseBackColorOnMouseEnter = Color.FromArgb(210, 210, 210);
        public static Color ButtonFlatBaseBackColorOnMouseDown = Color.FromArgb(200, 200, 200);

        public static Color SvgButtonSvgColorStandart = Color.White;
        public static Color SvgButtonSvgColorOnMouseEnter = Color.FromArgb(255, 255, 192);
    }
}
