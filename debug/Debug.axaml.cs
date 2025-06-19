using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
namespace Lightning_Bolt_Kit_Creator;


public partial class Debug : Window
{
    private static bool debug;
    private static bool isLoaded = false;
    private static Debug debugger = null;
    public static void Write(String text)
    {
        if (debug)
        {
            debugger.DebuggerText.Text = text + "\n" + (debugger.DebuggerText.Text);
        }
    }
    public static Boolean IsDebug() { return debug; }

    public Debug(bool D)
    {
        debug = D;
        if (debug)
        {
            InitializeComponent();
            Show();
            debugger = this;
            debugger.DebuggerText.Text = "\n\n\n\n\n\n";
        }
    }
    public Debug()
    {
        debug = false;
    }
    public static void CloseDebug()
    {
        if (debugger != null)
        {
            debugger.Close();
        }
    }
    public void On_Load(object sender, RoutedEventArgs e)
    {
        isLoaded = true;
    }
    public static bool GetLoadState()
    {
        return isLoaded;
    }
}