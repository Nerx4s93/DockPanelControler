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
        private ColorAnimation _animationOnStopMove;
        private ColorAnimation _animationOnHover;
        private ColorAnimation _animationOnLeave;

        public Color currentOutlineColor = Color.FromArgb(255, 255, 255);

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

        public DockableFormBase AttachedForm { get; internal set; }

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

            int globalLocationX = Parent.Left + Left;
            int globalLocationY = Parent.Top + Top;
            int globalLocationXEnd = Parent.Left + Left + Size.Height;
            int globalLocationYEnd = Parent.Top + Top + Size.Width;

            if (Cursor.Position.X > globalLocationX    && Cursor.Position.Y > globalLocationY &&
                Cursor.Position.X < globalLocationXEnd && Cursor.Position.Y < globalLocationYEnd)
            {
                if (!_startAnimationOnHover)
                {
                    _animationOnHover.Run();
                    _startAnimationOnHover = true;
                }
            }
            else if (_startAnimationOnHover)
            {
                _animationOnLeave.Run();
                _startAnimationOnHover = false;
            }

            Invalidate();
        }

        private void FormOnStopMove(object sendler)
        {
            _formMove = false;

            if (_startAnimationOnMove)
            {
                _animationOnStopMove.Run();
                _startAnimationOnMove = false;
            }

            int globalLocationX = Parent.Left + Left;
            int globalLocationY = Parent.Top + Top;
            int globalLocationXEnd = Parent.Left + Left + Size.Height;
            int globalLocationYEnd = Parent.Top + Top + Size.Width;

            if (Cursor.Position.X > globalLocationX && Cursor.Position.Y > globalLocationY &&
                Cursor.Position.X < globalLocationXEnd && Cursor.Position.Y < globalLocationYEnd)
            {
                var form = (DockableFormBase)sendler;

                AttachedForm = form;
                form.DockParent = this;
            }

            Invalidate();
        }

        #endregion

        protected override void CreateHandle()
        {
            base.CreateHandle();

            currentOutlineColor = BackColor;

            _animationOnMove      = new ColorAnimation(this, "currentOutlineColor", 30, BackColor,           OutlineColorOnMove);
            _animationOnStopMove  = new ColorAnimation(this, "currentOutlineColor", 30, OutlineColorOnMove,  BackColor);
            _animationOnHover     = new ColorAnimation(this, "currentOutlineColor", 40, OutlineColorOnMove,  OutlineColorOnHover);
            _animationOnLeave     = new ColorAnimation(this, "currentOutlineColor", 40, OutlineColorOnHover, OutlineColorOnMove);
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
            else
            {
                Pen pen = new Pen(new SolidBrush(currentOutlineColor), OutlineWidth);

                if (_formMove)
                {
                    graphics.FillRectangle(new SolidBrush(BackColorOnMove), 0, 0, Size.Width, Size.Height);
                }

                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
        }
    }
}