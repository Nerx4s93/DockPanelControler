using System.Drawing;
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

        #region Переменные

        private DockableFormBase[] _formCollection = new DockableFormBase[0];

        private bool _formMove;
        private bool _startAnimationOnMove;
        private bool _startAnimationOnHover;

        private ColorAnimation _animationOnMove;
        private ColorAnimation _animationOnHover;
        private ColorAnimation _animationOnLeave;

        public Color currentOutlineColor;

        #endregion
        #region Свойства

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
                    form.FormOnMove += FormOnMove;
                    form.FormOnStopMove += FormOnStopMove;
                }
            }
        }

        public Color OutlineColorOnMove { get; set; }

        public Color OutlineColorOnHover { get; set; }

        public Color BackColorOnMove { get; set; }

        public float OutlineWidth { get; set; }

        #endregion

        #region Методы форм

        private void FormOnMove(object sendler)
        {
            _formMove = true;

            if (!_startAnimationOnMove)
            {
                _animationOnMove.Run();
                _startAnimationOnMove = true;
            }

            Invalidate();
        }

        private void FormOnStopMove(object sendler)
        {
            _formMove = false;
            _startAnimationOnMove = false;

            Invalidate();
        }

        #endregion

        protected override void CreateHandle()
        {
            base.CreateHandle();

            _animationOnMove  = new ColorAnimation(this, "currentOutlineColor", 50, BackColor,           OutlineColorOnMove);
            _animationOnHover = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnMove,  OutlineColorOnHover);
            _animationOnLeave = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnHover, OutlineColorOnMove);
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
            else if (_formMove)
            {
                Pen pen = new Pen(new SolidBrush(currentOutlineColor), OutlineWidth);

                graphics.FillRectangle(new SolidBrush(BackColorOnMove), 0, 0, Size.Width, Size.Height);
                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
        }
    }
}