﻿using System.Drawing;
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
            DockPanel dockPanel, DockPanelPanelsManager dockPanelPanelsManager,
            ColorAnimation animationOnMove, ColorAnimation animationOnFormStopMove, ColorAnimation animationOnFormEnter, ColorAnimation animationOnFormLeave)
        {
            _dockPanel = dockPanel;
            _dockPanelPanelsManagerl = dockPanelPanelsManager;
            _animationOnFormMove = animationOnMove;
            _animationOnFormStopMove = animationOnFormStopMove;
            _animationOnFormEnter = animationOnFormEnter;
            _animationOnFormLeave = animationOnFormLeave;
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

                _dockPanelPanelsManagerl.ClearPanel();
                _dockPanelPanelsManagerl.AddControlsPanel(form.Controls);
                _dockPanelPanelsManagerl.ShowPanels();

                form.Close();
            }
        }
    }
}
