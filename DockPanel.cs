using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using DockPanelControler.Components;

namespace DockPanelControler
{
    public class DockPanel : Control
    {
        private DockPanelPanelsManager _dockPanelBodyPanelsManager;
        private DockPanelFormManager _dockPanelFormManager;

        private int _additionalScope;

        public DockPanel()
        {
            DoubleBuffered = true;
            GlobalFormManager.DockPanels.Add(this);
        }

        internal Color currentOutlineColor = DeffaultPropertyValues.DockPanelBackColorOnFormMove;

        [Browsable(false)]
        public DockableFormBase AttachedForm { get; internal set; }

        #region Свойства

        public int AdditionalScope { get; set; }

        public float OutlineWidth { get; set; } = DeffaultPropertyValues.DockPanelOutlineWidth;

        public Color OutlineColorOnFormMove { get; set; } = DeffaultPropertyValues.DockPanelOutlineColorOnFormMove;

        public Color OutlineColorOnFormEnter { get; set; } = DeffaultPropertyValues.DockPanelOutlineColorOnFormEnter;

        public Color BackColorOnFormMove { get; set; } = DeffaultPropertyValues.DockPanelBackColorOnFormMove;

        public Color TitleBarPanelBackColor { get; set; } = DeffaultPropertyValues.DockPanelTitleBarPanelBackColor;

        public Color BodyPanelBackColor { get; set; } = DeffaultPropertyValues.DockPanelBodyPanelBackColor;

        #endregion

        public void AddForm(DockableFormBase dockableFormBase)
        {
            GlobalFormManager.AddForm(dockableFormBase);
        }

        internal void AddFormInternal(DockableFormBase dockableFormBase)
        {
            _dockPanelFormManager.AttachFormEvents(dockableFormBase);
        }

        private void InstanteDockPanelFormManager(DockPanelPanelsManager dockPanelPanelManager)
        {
            _dockPanelFormManager = new DockPanelFormManager(this, dockPanelPanelManager);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            currentOutlineColor = BackColor;
            AdditionalScope = DeffaultPropertyValues.DockPanelAdditionalScope;

            _dockPanelBodyPanelsManager = new DockPanelPanelsManager(this, 20, TitleBarPanelBackColor, BodyPanelBackColor);
            InstanteDockPanelFormManager(_dockPanelBodyPanelsManager);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            if (DesignMode)
            {
                Pen pen = new Pen(new SolidBrush(OutlineColorOnFormMove), OutlineWidth);
                pen.Alignment = PenAlignment.Inset;

                graphics.FillRectangle(new SolidBrush(BackColorOnFormMove), 0, 0, Size.Width, Size.Height);
                graphics.DrawRectangle(pen, 0, 0, Size.Width, Size.Height);
            }
            else if (AttachedForm == null)
            {
                Pen pen = new Pen(new SolidBrush(currentOutlineColor), OutlineWidth);
                pen.Alignment = PenAlignment.Inset;

                if (_dockPanelFormManager.MouseEnterAdditionScope)
                {
                    graphics.FillRectangle(new SolidBrush(BackColorOnFormMove), 0, 0, Size.Width, Size.Height);
                }

                graphics.DrawRectangle(pen, 0, 0, Size.Width, Size.Height);
            }
        }
    }
}