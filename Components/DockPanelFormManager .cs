using System;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable CS1690

namespace DockPanelControler.Components
{
    public class DockPanelFormManager
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
            var dockableFormBase = sender as DockableFormBase;
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

        private void UpdateFormDockState(DockableFormBase dockableFormBase)
        {
            bool isMouseEnter = IsMouseEnterDockPanel();

            if (_dockPanel.AttachedForm == null && isMouseEnter && _mouseEnterAdditionScope)
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