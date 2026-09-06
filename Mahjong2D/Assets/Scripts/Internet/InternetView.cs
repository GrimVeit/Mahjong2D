using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InternetView : View
{
    [SerializeField] private TextMeshProUGUI textDescription;

    public void OnGetStatusDescription(string description)
    {
        if (textDescription != null)
            textDescription.text = description;
    }
}
