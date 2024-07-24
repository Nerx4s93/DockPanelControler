using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DockPanelControler
{
    public class DockPanelBodyPanelManager
    {
        private readonly Panel _panel;

        private readonly DockPanel _dockPanel;

        private readonly int _titleBarHeight;

        private readonly Color _backColor;

        public DockPanelBodyPanelManager(DockPanel dockPanel, int titleBarHeight, Color backColor)
        {
            _dockPanel = dockPanel;
            _titleBarHeight = titleBarHeight;
            _backColor = backColor;

            _panel = new Panel();
            AdjustPanel();

            _dockPanel.Resize += DockPanelOnResize;
        }

        public void ClearPanel()
        {
            _panel.Controls.Clear();
        }

        public void AddControlsPanel(Control.ControlCollection controlCollection)
        {
            _panel.Controls.AddRange(controlCollection.OfType<Control>().ToArray());
        }

        public void ShowPanel()
        {
            ResizePanel();
            _panel.Visible = true;
        }

        public void HidePanel()
        {
            _panel.Visible = false;
        }

        private void AdjustPanel()
        {
            _panel.Visible = false;
            _panel.BackColor = _backColor;
            _panel.Dock = DockStyle.Bottom;
            _dockPanel.Controls.Add(_panel);
            ResizePanel();
        }

        private void ResizePanel()
        {
            Size dockPanelSize = _dockPanel.Size;
            _panel.Size = new Size(dockPanelSize.Width, dockPanelSize.Height - _titleBarHeight);
        }

        private void DockPanelOnResize(object sender, EventArgs e)
        {
            if (!_panel.Visible)
            {
                return;
            }

            ResizePanel();
        }
    }
}
