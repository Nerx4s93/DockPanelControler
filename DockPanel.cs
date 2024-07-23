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

        private DockPanelFormManager _dockPanelFormManager;

        #region Переменные

        private DockableFormBase[] _formCollection = new DockableFormBase[0];
        public Color currentOutlineColor = Color.FromArgb(255, 255, 255);

        #endregion
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

        public Color OutlineColorOnMove { get; set; }

        public Color OutlineColorOnHover { get; set; }

        public Color BackColorOnMove { get; set; }

        public float OutlineWidth { get; set; }

        #endregion

        private void InstanteDockPanelFormManager()
        {
            ColorAnimation animationOnMove = new ColorAnimation(this, "currentOutlineColor", 40, BackColor, OutlineColorOnMove);
            ColorAnimation animationOnStopMove = new ColorAnimation(this, "currentOutlineColor", 40, OutlineColorOnMove, BackColor);
            ColorAnimation animationOnHover = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnMove, OutlineColorOnHover);
            ColorAnimation animationOnLeave = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnHover, OutlineColorOnMove);

            _dockPanelFormManager = new DockPanelFormManager(this, animationOnMove, animationOnStopMove, animationOnHover, animationOnLeave);
        }

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

        protected override void CreateHandle()
        {
            base.CreateHandle();
            currentOutlineColor = BackColor;
            InstanteDockPanelFormManager();
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