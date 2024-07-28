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

        public Color OutlineColorOnMove { get; set; } = Color.Gray;

        public Color OutlineColorOnHover { get; set; } = Color.Red;

        public Color BackColorOnMove { get; set; } = Color.Silver;

        public float OutlineWidth { get; set; } = 3;

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
            ColorAnimation animationOnMove = new ColorAnimation(this, "currentOutlineColor", 40, BackColor, OutlineColorOnMove);
            ColorAnimation animationOnStopMove = new ColorAnimation(this, "currentOutlineColor", 40, OutlineColorOnMove, BackColor);
            ColorAnimation animationOnHover = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnMove, OutlineColorOnHover);
            ColorAnimation animationOnLeave = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnHover, OutlineColorOnMove);

            _dockPanelFormManager = new DockPanelFormManager(this, dockPanelPanelManager, animationOnMove, animationOnStopMove, animationOnHover, animationOnLeave);
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
                Pen pen = new Pen(new SolidBrush(OutlineColorOnMove), OutlineWidth);

                graphics.FillRectangle(new SolidBrush(BackColorOnMove), 0, 0, Size.Width, Size.Height);
                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
            else if (AttachedForm == null)
            {
                Pen pen = new Pen(new SolidBrush(currentOutlineColor), OutlineWidth);

                if (_dockPanelFormManager.IsFormMoving)
                {
                    graphics.FillRectangle(new SolidBrush(BackColorOnMove), 0, 0, Size.Width, Size.Height);
                }

                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
        }
    }
}