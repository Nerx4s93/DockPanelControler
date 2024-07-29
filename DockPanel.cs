using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Animations;

namespace DockPanelControler
{
    public class DockPanel : Control
    {
        public DockPanel()
        {
            DoubleBuffered = true;
        }

        private DockPanelPanelsManager _dockPanelBodyPanelsManager;

        private DockPanelFormManager _dockPanelFormManager;

        private DockableFormBase[] _formCollection = new DockableFormBase[0];

        public Color currentOutlineColor = Color.FromArgb(255, 255, 255);

        #region Свойства

        [Browsable(false)]
        public DockableFormBase[] FormCollection
        {
            get
            {
                return _formCollection;
            }
            set
            {
                _formCollection = value;

                foreach(DockableFormBase form in _formCollection)
                {
                    _dockPanelFormManager.AttachFormEvents(form);
                }
            }
        }

        [Browsable(false)]
        public DockableFormBase AttachedForm { get; internal set; }

        public float OutlineWidth { get; set; } = 3;

        public Color OutlineColorOnFormMove { get; set; } = Color.Gray;

        public Color OutlineColorOnFormEnter { get; set; } = Color.Red;

        public Color BackColorOnFormMove { get; set; } = Color.Silver;

        public Color TitleBarPanelBackColor { get; set; } = Color.DimGray;

        public Color BodyPanelBackColor { get; set; } = Color.FromArgb(224, 224, 224);

        #endregion

        public void AddForm(DockableFormBase dockableFormBase)
        {
            var listFormCollection = FormCollection.ToList();
            listFormCollection.Add(dockableFormBase);

            foreach (var form in listFormCollection)
            {
                _dockPanelFormManager.DetachFormEvents(form);
            }

            FormCollection = listFormCollection.ToArray();
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