using System;
using System.Windows.Forms;

namespace DockPanelControler.Components
{
    public class FormDockHandler
    {
        private const int WM_MOVE = 0x0003; //Сообщение движения формы
        private const int WM_EXITSIZEMOVE = 0x0232; // Сообщение выходи из режима изменения расположения/размера окна

        public readonly Form form;

        private DockPanel _dockPanel;

        private FormMessageHook _formMessageHook;

        public FormDockHandler(Form form)
        {
            this.form = form;

            _formMessageHook = new FormMessageHook(this.form);

            _formMessageHook.OnFormMessage += OnFormMessage;
            form.Shown += FormShown;
            form.VisibleChanged += FormVisibleChanged;
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

        private void OnFormMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_MOVE)
            {
                FormOnMove?.Invoke(this);
            }
            else if (msg == WM_EXITSIZEMOVE)
            {
                FormOnStopMove?.Invoke(this);
            }
        }

        private void FormShown(object sender, EventArgs e)
        {
            GlobalFormManager.AddForm(this);
        }

        private void FormVisibleChanged(object sender, EventArgs e)
        {
            VisibleChanged?.Invoke(this, e);
        }

        public event FormMoveEventHandler FormOnMove;
        public event FormStopMoveEventHandler FormOnStopMove;
        public event FormChangeDockPanelEventHandler FormOnChangeDockPanel;
        public event EventHandler VisibleChanged;

        public delegate void FormMoveEventHandler(object sendler);
        public delegate void FormStopMoveEventHandler(object sendler);
        public delegate void FormChangeDockPanelEventHandler(object sendler);
    }
}
