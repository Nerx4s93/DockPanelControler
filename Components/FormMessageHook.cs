using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal class FormMessageHook
{
    #region user32.dll

    public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public const int GWL_WNDPROC = -4;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    #endregion

    public event FormMessage OnFormMessage;
    public delegate void FormMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    private IntPtr _originalWndProc;
    private WndProcDelegate _wndProcDelegate;

    public FormMessageHook(Form form)
    {
        _wndProcDelegate = new WndProcDelegate(WndProc);

        IntPtr hWnd = form.Handle;

        _originalWndProc = SetWindowLong(hWnd, GWL_WNDPROC,
            Marshal.GetFunctionPointerForDelegate(_wndProcDelegate));
    }

    private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        OnFormMessage?.Invoke(hWnd, msg, wParam, lParam);
        return CallWindowProc(_originalWndProc, hWnd, msg, wParam, lParam);
    }
}
