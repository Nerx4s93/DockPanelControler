using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Animations;

namespace DockPanelControler
{
    public class DockPanel : Panel
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
                    form.FormClosed += FormOnClosed;
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

        private void FormOnClosed(object sender, FormClosedEventArgs e)
        {
            _formMove = false;

            Invalidate();
        }

        private void FormOnMove(object sendler)
        {
            _formMove = true;

            if (!_startAnimationOnMove)
            {
                _animationOnMove.Run();
                _startAnimationOnMove = true;
            }

            var cursorPosition = Cursor.Position;
            var panelBounds = Bounds;

            bool isHovering = panelBounds.Contains(Parent.PointToClient(cursorPosition));

            if (isHovering)
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
            if (_startAnimationOnMove)
            {
                _animationOnStopMove.Run();
                _startAnimationOnMove = false;
            }

            var cursorPosition = Cursor.Position;
            var panelBounds = Bounds;

            bool isHovering = panelBounds.Contains(Parent.PointToClient(cursorPosition));

            if (isHovering && _formMove == true && AttachedForm == null)
            {
                var form = (DockableFormBase)sendler;

                form.DockParent = this;
                AttachedForm = form;
                AttachedForm.Size = Size;

                Point attachedFormNewLocation = PointToScreen(Point.Empty);
                AttachedForm.Location = attachedFormNewLocation;

                Control[] controls = new Control[AttachedForm.Controls.Count];
                AttachedForm.Controls.CopyTo(controls, 0);

                Controls.Clear();
                Controls.AddRange(controls);

                AttachedForm.Close();
            }

            _formMove = false;

            Invalidate();
        }

        #endregion

        public void AddForm(DockableFormBase dockableFormBase)
        {
            var listFormCollection = FormCollection.ToList();
            listFormCollection.Add(dockableFormBase);

            foreach (var form in listFormCollection)
            {
                form.FormOnMove -= FormOnMove;
                form.FormOnStopMove -= FormOnStopMove;
            }

            FormCollection = listFormCollection.ToArray();
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            currentOutlineColor = BackColor;

            _animationOnMove      = new ColorAnimation(this, "currentOutlineColor", 40, BackColor,           OutlineColorOnMove);
            _animationOnStopMove  = new ColorAnimation(this, "currentOutlineColor", 40, OutlineColorOnMove,  BackColor);
            _animationOnHover     = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnMove,  OutlineColorOnHover);
            _animationOnLeave     = new ColorAnimation(this, "currentOutlineColor", 50, OutlineColorOnHover, OutlineColorOnMove);
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

                if (_formMove)
                {
                    graphics.FillRectangle(new SolidBrush(BackColorOnMove), 0, 0, Size.Width, Size.Height);
                }

                graphics.DrawRectangle(pen, OutlineWidth / 2, OutlineWidth / 2, Size.Width - OutlineWidth, Size.Height - OutlineWidth);
            }
        }
    }
}