using System.Windows.Forms;

namespace DockPanelControler
{
    public partial class DockableFormBase : Form
    {
        public DockableFormBase()
        {
            InitializeComponent();
        }

        private DockPanel _dockPanel;

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

        private void DockableFormBase_Shown(object sender, System.EventArgs e)
        {
            GlobalFormManager.AddForm(this);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 70)
            {
                FormOnMove?.Invoke(this);
            }
            else if (m.Msg == 160)
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
