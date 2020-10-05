using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Windows;

public class Win32 : MonoBehaviour
{
    [DllImport("User32.Dll")]
    static extern long SetCursorPos(int x, int y);
       

    const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    const uint MOUSEEVENTF_LEFTUP = 0x0004;


    [DllImport("user32.dll")]
    private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);


    public static void SendUp()
    {
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        new Thread(() =>
       {
           SocketSwypeHelper.instance.SendToClient("PIPIPUPUCHECK");
       }).Start();
    }

    public static void MoveCursor(int x, int y)
    {
        SetCursorPos(x, y);
        //Thread.Sleep(10);
    }

    public static void SendDown()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
        //Thread.Sleep(20);
    }

    /// <summary>
    /// Struct representing a point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// Retrieves the cursor's position, in screen coordinates.
    /// </summary>
    /// <see>See MSDN documentation for further information.</see>
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);
}