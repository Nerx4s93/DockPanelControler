using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DockPanelControler
{
    public class DockPanelPanelsManager
    {
        private readonly Panel _titleBarPanel;
        private readonly Panel _bodyPanel;

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
            AdjustPanels();

            _dockPanel.Resize += DockPanelOnResize;
        }

        public void ClearPanel()
        {
            _bodyPanel.Controls.Clear();
        }

        public void AddControlsPanel(Control.ControlCollection controlCollection)
        {
            _bodyPanel.Controls.AddRange(controlCollection.OfType<Control>().ToArray());
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

        private void AdjustPanels()
        {
            _titleBarPanel.Visible = false;
            _titleBarPanel.BackColor = _titleBarBackColor;
            _titleBarPanel.Dock = DockStyle.Top;

            _bodyPanel.Visible = false;
            _bodyPanel.BackColor = _bodyBackColor;
            _bodyPanel.Dock = DockStyle.Bottom;

            _dockPanel.Controls.Add(_titleBarPanel);
            _dockPanel.Controls.Add(_bodyPanel);

            ResizePanels();
        }

        private void ResizePanels()
        {
            Size dockPanelSize = _dockPanel.Size;
            _titleBarPanel.Size = new Size(dockPanelSize.Width, _titleBarHeight);
            _bodyPanel.Size = new Size(dockPanelSize.Width, dockPanelSize.Height - _titleBarHeight);
        }

        private void DockPanelOnResize(object sender, EventArgs e)
        {
            if (!_bodyPanel.Visible)
            {
                return;
            }

            ResizePanels();
        }
    }
}
