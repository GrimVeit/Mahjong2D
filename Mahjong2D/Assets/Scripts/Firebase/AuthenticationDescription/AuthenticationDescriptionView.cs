using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthenticationDescriptionView : View
{
    [SerializeField] private List<TextMeshProUGUI> textDescriptions = new();

    public void SetDescription(string description)
    {
        textDescriptions.ForEach(x => x.text = description);
    }
}
