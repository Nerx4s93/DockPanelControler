using System;
using System.Windows.Forms;

namespace DockPanelControler
{
    public partial class DockableFormBase : Form
    {
        private const int FormOnMoveMessage = 70;
        private const int FormOnStopMoveMessage = 160;
        
        private DockPanel _dockPanel;

        public DockableFormBase()
        {
            InitializeComponent();
        }

        public DockPanel DockPanel
        {
            get
            {
                return _dockPanel;
            }
            internal set
            {
                _dockPanel = value;

                FormOnChangeDockPanel?.Invoke(this);
            }
        }

        private void DockableFormBase_Shown(object sender, EventArgs e)
        {
            GlobalFormManager.AddForm(this);
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == FormOnMoveMessage)
            {
                FormOnMove?.Invoke(this);
            }
            else if (message.Msg == FormOnStopMoveMessage)
            {
                FormOnStopMove?.Invoke(this);
            }
        }

        public event FormMoveEventHandler FormOnMove;
        public event FormStopMoveEventHandler FormOnStopMove;
        public event FormChangeDockPanelEventHandler FormOnChangeDockPanel;

        public delegate void FormMoveEventHandler(object sendler);
        public delegate void FormStopMoveEventHandler(object sendler);
        public delegate void FormChangeDockPanelEventHandler(object sendler);
    }
}
