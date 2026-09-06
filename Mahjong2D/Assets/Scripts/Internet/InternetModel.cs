using System;
using System.Collections;
using UnityEngine;

public class InternetModel
{
    public bool HasNetwork => Application.internetReachability != NetworkReachability.NotReachable;
}
