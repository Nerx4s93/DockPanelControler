using System;
using System.Drawing;
using System.Windows.Forms;

using Animations;

namespace DockPanelControler
{
    public class DockPanelFormManager
    {
        private readonly DockPanel _dockPanel;

        private readonly DockPanelPanelsManager _dockPanelPanelsManagerl;

        private readonly ColorAnimation _animationOnFormMove;
        private readonly ColorAnimation _animationOnFormStopMove;
        private readonly ColorAnimation _animationOnFormEnter;
        private readonly ColorAnimation _animationOnFormLeave;

        private bool _formMove;
        private bool _startAnimationOnMove;
        private bool _startAnimationOnHover;

        public DockPanelFormManager(
            DockPanel dockPanel,
            DockPanelPanelsManager dockPanelPanelsManager,
            ColorAnimation animationOnMove,
            ColorAnimation animationOnFormStopMove,
            ColorAnimation animationOnFormEnter,
            ColorAnimation animationOnFormLeave)
        {
            _dockPanel = dockPanel;
            _dockPanelPanelsManagerl = dockPanelPanelsManager;
            _animationOnFormMove = animationOnMove;
            _animationOnFormStopMove = animationOnFormStopMove;
            _animationOnFormEnter = animationOnFormEnter;
            _animationOnFormLeave = animationOnFormLeave;
        }

        public bool IsFormMoving => _formMove;

        public void AttachFormEvents(DockableFormBase dockableFormBase)
        {
            dockableFormBase.FormOnMove += FormOnMove;
            dockableFormBase.FormOnStopMove += FormOnStopMove;
            dockableFormBase.VisibleChanged += FormVisibleChanged;
        }

        public void DetachFormEvents(DockableFormBase dockableFormBase)
        {
            dockableFormBase.FormOnMove -= FormOnMove;
            dockableFormBase.FormOnStopMove -= FormOnStopMove;
            dockableFormBase.VisibleChanged -= FormVisibleChanged;
        }

        private void FormVisibleChanged(object sender, EventArgs e)
        {
            var form = sender as DockableFormBase;

            DetachFormEvents(form);
            GlobalFormManager.FormCollection.Remove(form);

            StopAnimations();
            _formMove = false;
            _dockPanel.Invalidate();
        }

        private void FormOnMove(object sender)
        {
            _formMove = true;
            if (!_startAnimationOnMove)
            {
                _animationOnFormMove.Run();
                _startAnimationOnMove = true;
            }
            UpdateHoverState();
            _dockPanel.Invalidate();
        }

        private void FormOnStopMove(object sender)
        {
            if (_startAnimationOnMove)
            {
                _animationOnFormStopMove.Run();
                _startAnimationOnMove = false;
            }
            var dockableFormBase = sender as DockableFormBase;
            UpdateFormDockState(dockableFormBase);
            _formMove = false;
            _dockPanel.Invalidate();
        }

        private void UpdateHoverState()
        {
            bool isHovering = IsHover();

            if (isHovering)
            {
                if (!_startAnimationOnHover)
                {
                    _animationOnFormEnter.Run();
                    _startAnimationOnHover = true;
                }
            }
            else if (_startAnimationOnHover)
            {
                _animationOnFormLeave.Run();
                _startAnimationOnHover = false;
            }
        }

        private void UpdateFormDockState(DockableFormBase dockableFormBase)
        {
            bool isHovering = IsHover();

            if (isHovering && _formMove && _dockPanel.AttachedForm == null)
            {
                _dockPanel.AttachedForm = dockableFormBase;
                _dockPanel.AttachedForm.Size = _dockPanel.Size;
                _dockPanel.AttachedForm.DockPanel = _dockPanel;
                _dockPanel.AttachedForm.Location = _dockPanel.PointToScreen(Point.Empty);

                _dockPanelPanelsManagerl.ClearPanel();
                _dockPanelPanelsManagerl.AddControlsPanel(dockableFormBase.Controls);
                _dockPanelPanelsManagerl.ShowPanels();

                dockableFormBase.Hide();
            }
        }

        private void StopAnimations()
        {
            _animationOnFormMove.Stop();
            _animationOnFormStopMove.Stop();
            _animationOnFormEnter.Stop();
            _animationOnFormLeave.Stop();
        }

        private bool IsHover()
        {
            var cursorPosition = Cursor.Position;
            var panelBounds = _dockPanel.Bounds;

            bool result = panelBounds.Contains(_dockPanel.Parent.PointToClient(cursorPosition));
            return result;
        }
    }
}
