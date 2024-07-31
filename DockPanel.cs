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

        public Color currentOutlineColor = Color.FromArgb(255, 255, 255);

        [Browsable(false)]
        public DockableFormBase AttachedForm { get; internal set; }

        #region Свойства

        public float OutlineWidth { get; set; } = 3;

        public Color OutlineColorOnFormMove { get; set; } = Color.Gray;

        public Color OutlineColorOnFormEnter { get; set; } = Color.Red;

        public Color BackColorOnFormMove { get; set; } = Color.Silver;

        public Color TitleBarPanelBackColor { get; set; } = Color.Silver;

        public Color BodyPanelBackColor { get; set; } = Color.FromArgb(224, 224, 224);

        #endregion

        public void AddForm(DockableFormBase form)
        {
            GlobalFormManager.AddForm(form);
        }

        internal void AddFormInternal(DockableFormBase form)
        {
            _dockPanelFormManager.AttachFormEvents(form);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

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