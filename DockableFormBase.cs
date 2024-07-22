using System;
using System.Windows.Forms;

namespace DockPanelControler
{
    public partial class DockableFormBase : Form
    {
        public DockableFormBase()
        {
            InitializeComponent();
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

        public delegate void FormMoveEventHandler(object sendler);
        public delegate void FormStopMoveEventHandler(object sendler);
    }
}
