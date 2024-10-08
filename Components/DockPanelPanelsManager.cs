﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DockPanelControler.Components
{
    internal class DockPanelPanelsManager
    {
        private readonly Panel _titleBarPanel;
        private readonly Panel _bodyPanel;
        private readonly SvgButton _buttonUnpin;

        private readonly DockPanel _dockPanel;

        private readonly int _titleBarHeight;
        private readonly Color _titleBarBackColor;
        private readonly Color _bodyBackColor;

        public DockPanelPanelsManager(DockPanel dockPanel, int titleBarHeight, Color titleBarBackColor, Color bodyBackColor)
        {
            _dockPanel = dockPanel;
            _titleBarHeight = titleBarHeight;
            _titleBarBackColor = titleBarBackColor;
            _bodyBackColor = bodyBackColor;

            _titleBarPanel = new Panel();
            _bodyPanel = new Panel();
            _buttonUnpin = new SvgButton();
            AdjustControls();

            _dockPanel.Resize += DockPanelOnResize;
        }

        public void ClearPanel()
        {
            _bodyPanel.Controls.Clear();
        }

        public void AddControlsPanel(Control.ControlCollection controlCollection)
        {
            var controlCollectionArray = controlCollection.OfType<Control>().ToArray();
            _bodyPanel.Controls.AddRange(controlCollectionArray);
        }

        public void ShowPanels()
        {
            ResizePanels();
            _titleBarPanel.Visible = true;
            _bodyPanel.Visible = true;
        }

        public void HidePanels()
        {
            _titleBarPanel.Visible = false;
            _bodyPanel.Visible = false;
        }

        private void AdjustControls()
        {
            _titleBarPanel.Visible = false;
            _titleBarPanel.BackColor = _titleBarBackColor;
            _titleBarPanel.Dock = DockStyle.Top;

            _bodyPanel.Visible = false;
            _bodyPanel.BackColor = _bodyBackColor;
            _bodyPanel.Dock = DockStyle.Bottom;

            _buttonUnpin.SvgName = "unpin";
            _buttonUnpin.Size = new Size(_titleBarHeight, _titleBarHeight);
            _buttonUnpin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _buttonUnpin.Location = new Point(_titleBarPanel.Size.Width - _titleBarHeight, 0);

            _titleBarPanel.Controls.Add(_buttonUnpin);
            _dockPanel.Controls.Add(_titleBarPanel);
            _dockPanel.Controls.Add(_bodyPanel);

            _buttonUnpin.MouseClick += ButtonUnpinMouseClick;

            ResizePanels();
        }

        private void ResizePanels()
        {
            Size dockPanelSize = _dockPanel.Size;
            _titleBarPanel.Size = new Size(dockPanelSize.Width, _titleBarHeight);
            _bodyPanel.Size = new Size(dockPanelSize.Width, dockPanelSize.Height - _titleBarHeight);
        }

        private void DockPanelOnResize(object sender, EventArgs eventArgs)
        {
            if (!_bodyPanel.Visible)
            {
                return;
            }

            ResizePanels();
        }

        private void ButtonUnpinMouseClick(object sender, MouseEventArgs e)
        {
            var formDockHandler = _dockPanel.AttachedDockFormHandler;
            var cursorPosition = Cursor.Position;

            var controlCollectionArray = _bodyPanel.Controls.OfType<Control>().ToArray();

            formDockHandler.form.Controls.AddRange(controlCollectionArray);
            formDockHandler.form.Location = new Point(cursorPosition.X + 20, formDockHandler.form.Location.Y);
            _bodyPanel.Controls.Clear();

            formDockHandler.DockPanel = null;
            _dockPanel.AttachedDockFormHandler = null;

            HidePanels();

            formDockHandler.form.Show();
            GlobalFormManager.AddForm(formDockHandler);
        }
    }
}
