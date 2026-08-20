using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private Panel currentPanel;

    protected void ShowPanel(Panel panel)
    {
        if (panel == null || panel.IsOpen)
            return;

        panel.Show();
    }

    protected void ClosePanel(Panel panel)
    {
        if (panel == null || !panel.IsOpen)
            return;

        panel.Hide();

        if (currentPanel == panel)
            currentPanel = null;
    }

    protected void ShowExclusivePanel(Panel panel)
    {
        if (panel == null)
            return;

        if (currentPanel == panel && panel.IsOpen)
            return;

        currentPanel?.Hide();

        currentPanel = panel;

        panel.Show();
    }
}
