using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Animations;

namespace DockPanelControler
{
    public class DockPanel : Control
    {
        private DockPanelPanelsManager _dockPanelBodyPanelsManager;
        private DockPanelFormManager _dockPanelFormManager;

        public DockPanel()
        {
            DoubleBuffered = true;
            GlobalFormManager.DockPanels.Add(this);
        }

        public Color currentOutlineColor = DeffaultPropertyValues.DockPanelBackColorOnFormMove;

        [Browsable(false)]
        public DockableFormBase AttachedForm { get; internal set; }

        #region Свойства

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
            ColorAnimation animationOnFormMove = new ColorAnimation(this, "currentOutlineColor", 40, BackColor, OutlineColorOnFormMove);
            ColorAnimation animationOnFormStopMove = new ColorAnimation(this, "currentOutlineColor", 40, OutlineColorOnFormMove, BackColor);
            ColorAnimation animationOnFormEnter = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnFormMove, OutlineColorOnFormEnter);
            ColorAnimation animationOnFormLeave = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnFormEnter, OutlineColorOnFormMove);

            _dockPanelFormManager = new DockPanelFormManager(this, dockPanelPanelManager, animationOnFormMove, animationOnFormStopMove, animationOnFormEnter, animationOnFormLeave);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            currentOutlineColor = BackColor;

            _dockPanelBodyPanelsManager = new DockPanelPanelsManager(this, 20, TitleBarPanelBackColor, BodyPanelBackColor);
            InstanteDockPanelFormManager(_dockPanelBodyPanelsManager);
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            Graphics graphics = paintEventArgs.Graphics;

            if (DesignMode)
            {
                Pen pen = new Pen(new SolidBrush(OutlineColorOnFormMove), OutlineWidth);

                graphics.FillRectangle(new SolidBrush(BackColorOnFormMove), 0, 0, Size.Width, Size.Height);
                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
            else if (AttachedForm == null)
            {
                Pen pen = new Pen(new SolidBrush(currentOutlineColor), OutlineWidth);

                if (_dockPanelFormManager.IsFormMoving)
                {
                    graphics.FillRectangle(new SolidBrush(BackColorOnFormMove), 0, 0, Size.Width, Size.Height);
                }

                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
        }
    }
}