using System.Drawing;
using System.Windows.Forms;

using Animations;

namespace DockPanelControler
{
    public class DockPanelFormManager
    {
        private readonly DockPanel _dockPanel;

        private readonly DockPanelBodyPanelManager _dockPanelBodyPanelManagerl;

        private readonly ColorAnimation _animationOnMove;
        private readonly ColorAnimation _animationOnStopMove;
        private readonly ColorAnimation _animationOnHover;
        private readonly ColorAnimation _animationOnLeave;

        private bool _formMove;
        private bool _startAnimationOnMove;
        private bool _startAnimationOnHover;

        public DockPanelFormManager(
            DockPanel dockPanel, DockPanelBodyPanelManager dockPanelBodyPanelManager,
            ColorAnimation animationOnMove, ColorAnimation animationOnStopMove, ColorAnimation animationOnHover, ColorAnimation animationOnLeave)
        {
            _dockPanel = dockPanel;
            _dockPanelBodyPanelManagerl = dockPanelBodyPanelManager;
            _animationOnMove = animationOnMove;
            _animationOnStopMove = animationOnStopMove;
            _animationOnHover = animationOnHover;
            _animationOnLeave = animationOnLeave;
        }

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

                _dockPanelBodyPanelManagerl.ClearPanel();
                _dockPanelBodyPanelManagerl.AddControlsPanel(form.Controls);
                _dockPanelBodyPanelManagerl.ShowPanel();

                form.Close();
            }
        }
    }
}
