using System;
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

        private ColorAnimation _animationOnHover;

        private ColorAnimation _animationOnLeave;

        private Color _outlineColorOnMove;

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

        public Color OutlineColorOnMove
        {
            get
            {
                return _outlineColorOnMove;
            }
            set
            {
                _outlineColorOnMove = value;
                currentOutlineColor = value;

                Invalidate();
            }
        }

        public Color OutlineColorOnHover { get; set; }

        public Color BackColorOnMove { get; set; }

        public float OutlineWidth { get; set; }

        #endregion

        #region Методы форм
        private void FormOnMove(object sendler)
        {
            _formMove = true;
            Invalidate();
        }

        private void FormOnStopMove(object sendler)
        {
            _formMove = false;
            Invalidate();
        }

        #endregion

        //Test
        public void Run()
        {
            _animationOnHover.Run();
        }

        //Test
        public void Run1()
        {
            _animationOnLeave.Run();
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            _animationOnHover = new ColorAnimation(this, "currentOutlineColor", 10, OutlineColorOnMove, OutlineColorOnHover);
            _animationOnLeave = new ColorAnimation(this, "currentOutlineColor", 10, OutlineColorOnHover, OutlineColorOnMove);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(Brushes.Red, 0, 0, 100, 100);

            if (_formMove || DesignMode)
            {
                Console.WriteLine(currentOutlineColor.R + " " +  currentOutlineColor.G + " " + currentOutlineColor.B);
                Pen pen = new Pen(new SolidBrush(currentOutlineColor), OutlineWidth);

                graphics.FillRectangle(new SolidBrush(BackColorOnMove), 0, 0, Size.Width, Size.Height);
                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
        }
    }
}