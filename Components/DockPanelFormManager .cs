using System;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable CS1690

namespace DockPanelControler.Components
{
    internal class DockPanelFormManager
    {
        private readonly DockPanel _dockPanel;

        private readonly DockPanelPanelsManager _dockPanelPanelsManagerl;

        private bool _mouseEnterAdditionScope;

        public DockPanelFormManager(DockPanel dockPanel, DockPanelPanelsManager dockPanelPanelsManager)
        {
            _dockPanel = dockPanel;
            _dockPanelPanelsManagerl = dockPanelPanelsManager;
        }

        public bool MouseEnterAdditionScope => _mouseEnterAdditionScope;

        public void AttachFormEvents(FormDockHandler formDockHandler)
        {
            formDockHandler.FormOnMove += FormOnMove;
            formDockHandler.FormOnStopMove += FormOnStopMove;
            formDockHandler.VisibleChanged += FormVisibleChanged;
        }

        public void DetachFormEvents(FormDockHandler formDockHandler)
        {
            formDockHandler.FormOnMove -= FormOnMove;
            formDockHandler.FormOnStopMove -= FormOnStopMove;
            formDockHandler.VisibleChanged -= FormVisibleChanged;
        }

        private void FormVisibleChanged(object sender, EventArgs e)
        {
            var formDockHandler = sender as FormDockHandler;

            DetachFormEvents(formDockHandler);
            GlobalFormManager.FormCollection.Remove(formDockHandler);

            _dockPanel.currentOutlineColor = _dockPanel.BackColor;
            _mouseEnterAdditionScope = false;
            _dockPanel.Invalidate();
        }

        private void FormOnMove(object sender)
        {
            if (IsMouseEnterAdditionScope())
            {
                _mouseEnterAdditionScope = true;
                _dockPanel.currentOutlineColor = _dockPanel.OutlineColorOnFormMove;
                UpdateHoverState();
                _dockPanel.Invalidate();
            }
            else
            {
                _mouseEnterAdditionScope = false;
                _dockPanel.currentOutlineColor = _dockPanel.BackColor;
                _dockPanel.Invalidate();
            }
        }

        private void FormOnStopMove(object sender)
        {
            _dockPanel.currentOutlineColor = _dockPanel.BackColor;
            var dockableFormBase = sender as FormDockHandler;
            UpdateFormDockState(dockableFormBase);
            _mouseEnterAdditionScope = false;
            _dockPanel.Invalidate();
        }

        private void UpdateHoverState()
        {
            bool isMouseEnterDockPanel = IsMouseEnterDockPanel();

            if (isMouseEnterDockPanel)
            {
                _dockPanel.currentOutlineColor = _dockPanel.OutlineColorOnFormEnter;
            }
            else
            {
                _dockPanel.currentOutlineColor = _dockPanel.OutlineColorOnFormMove;
            }
        }

        private void UpdateFormDockState(FormDockHandler formDockHandler)
        {
            bool isMouseEnter = IsMouseEnterDockPanel();

            if (_dockPanel.AttachedForm == null && isMouseEnter && _mouseEnterAdditionScope)
            {
                _dockPanel.AttachedForm = formDockHandler;
                _dockPanel.AttachedForm.DockPanel = _dockPanel;
                _dockPanel.AttachedForm.form.Size = _dockPanel.Size;
                _dockPanel.AttachedForm.form.Location = _dockPanel.PointToScreen(Point.Empty);

                _dockPanelPanelsManagerl.ClearPanel();
                _dockPanelPanelsManagerl.AddControlsPanel(formDockHandler.form.Controls);
                _dockPanelPanelsManagerl.ShowPanels();

                formDockHandler.form.Hide();
            }
        }

        private bool IsMouseEnterDockPanel()
        {
            var cursorPosition = Cursor.Position;
            var panelBounds = _dockPanel.Bounds;

            bool result = panelBounds.Contains(_dockPanel.Parent.PointToClient(cursorPosition));
            return result;
        }

        private bool IsMouseEnterAdditionScope()
        {
            var cursorPosition = Cursor.Position;

            int x = _dockPanel.Bounds.X - _dockPanel.AdditionalScope;
            int y = _dockPanel.Bounds.Y - _dockPanel.AdditionalScope;
            int width = _dockPanel.Bounds.Width + 2 * _dockPanel.AdditionalScope;
            int height = _dockPanel.Bounds.Height + 2 * _dockPanel.AdditionalScope;

            Rectangle additionalScopeBounds = new Rectangle(x, y, width, height);

            bool result = additionalScopeBounds.Contains(_dockPanel.Parent.PointToClient(cursorPosition));
            return result;
        }
    }
}