using Animations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DockPanelControler
{
    public class DockPanelFormManager
    {
        public DockPanelFormManager(DockPanel dockPanel, ColorAnimation animationOnMove, ColorAnimation animationOnStopMove, ColorAnimation animationOnHover, ColorAnimation animationOnLeave)
        {
            _dockPanel = dockPanel;
            _animationOnMove = animationOnMove;
            _animationOnStopMove = animationOnStopMove;
            _animationOnHover = animationOnHover;
            _animationOnLeave = animationOnLeave;
        }

        private DockPanel _dockPanel;

        private bool _formMove;
        private bool _startAnimationOnMove;
        private bool _startAnimationOnHover;

        private ColorAnimation _animationOnMove;
        private ColorAnimation _animationOnStopMove;
        private ColorAnimation _animationOnHover;
        private ColorAnimation _animationOnLeave;

        public bool IsFormMoving => _formMove;

        public void AttachFormEvents(DockableFormBase form)
        {
            form.FormOnMove += FormOnMove;
            form.FormOnStopMove += FormOnStopMove;
            form.FormClosed += FormOnClosed;
        }

        public void DetachFormEvents(DockableFormBase form)
        {
            form.FormOnMove -= FormOnMove;
            form.FormOnStopMove -= FormOnStopMove;
            form.FormClosed -= FormOnClosed;
        }

        private void FormOnClosed(object sender, FormClosedEventArgs e)
        {
            _formMove = false;
            _dockPanel.Invalidate();
        }

        private void FormOnMove(object sender)
        {
            _formMove = true;
            if (!_startAnimationOnMove)
            {
                _animationOnMove.Run();
                _startAnimationOnMove = true;
            }
            UpdateHoverState();
            _dockPanel.Invalidate();
        }

        private void FormOnStopMove(object sender)
        {
            if (_startAnimationOnMove)
            {
                _animationOnStopMove.Run();
                _startAnimationOnMove = false;
            }
            UpdateFormDockState(sender as DockableFormBase);
            _formMove = false;
            _dockPanel.Invalidate();
        }

        private void UpdateHoverState()
        {
            var cursorPosition = Cursor.Position;
            var panelBounds = _dockPanel.Bounds;

            bool isHovering = panelBounds.Contains(_dockPanel.Parent.PointToClient(cursorPosition));

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
        }

        private void UpdateFormDockState(DockableFormBase form)
        {
            var cursorPosition = Cursor.Position;
            var panelBounds = _dockPanel.Bounds;

            bool isHovering = panelBounds.Contains(_dockPanel.Parent.PointToClient(cursorPosition));

            if (isHovering && _formMove && _dockPanel.AttachedForm == null)
            {
                _dockPanel.AttachedForm = form;
                _dockPanel.AttachedForm.Size = _dockPanel.Size;
                _dockPanel.AttachedForm.DockParent = _dockPanel;
                _dockPanel.AttachedForm.Location = _dockPanel.PointToScreen(Point.Empty);
                _dockPanel.Controls.Clear();
                _dockPanel.Controls.AddRange(form.Controls.OfType<Control>().ToArray());
                form.Close();
            }
        }
    }
}
