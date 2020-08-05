using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    private Rect windowRect;
    // Only show it if needed.
    private static bool show = false;
    private static bool exit = false;
    private static string msg;
    GUIStyle buttonStyle = new GUIStyle();
    GUIStyle textStyle = new GUIStyle();
    GUIStyle mainStyle = new GUIStyle();

    // Enhanced Message Box

    void OnEnable()
    {
        windowRect = new Rect(0, Screen.height / 3, Screen.width, Screen.height / 3);
        //buttonStyle = GUI.skin.button;
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.fontSize = 25;
        textStyle.normal.textColor = Color.white;
        buttonStyle.fontSize = 30;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        //mainStyle.fontSize = 30;
    }

    void OnGUI()
    {
        if (show)
        {
            windowRect = GUI.Window(0, windowRect, DialogWindow, "Note");
        }
    }

    // This is the actual window.
    void DialogWindow(int windowID)
    {
        float y = 20;
        GUI.Label(new Rect(0, 0, Screen.width, (0.5f * Screen.height) / 2), msg, textStyle);

        if (GUI.Button(new Rect(125, ((0.9f * Screen.height) / 4), Screen.width - 250, windowRect.height / 4), "Close"))
        {
            show = false;
            if (exit)
            {
                Application.Quit();
            }
        }
    }

    // To open the dialogue from outside of the script.
    public static void Open(string str)
    {
        show = true;
        msg = str;
    }

    public static void Close()
    {
        show = false;
    }
}
