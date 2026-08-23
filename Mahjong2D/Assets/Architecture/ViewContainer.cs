using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewContainer : MonoBehaviour
{
    private readonly Dictionary<Type, View> viewsWithoutID = new();
    private readonly Dictionary<(Type, string), View> viewsWithID = new();

    public void Initialize()
    {
        viewsWithoutID.Clear();
        viewsWithID.Clear();

        var views = GetComponentsInChildren<View>(true);

        foreach (var view in views)
        {
            RegisterView(view.GetType(), view);
        }
    }

    public void RegisterView(Type type, View view)
    {
        if (view is IIdentify identify)
        {
            var key = (type, identify.GetID());
            if (!viewsWithID.ContainsKey(key))
            {
                viewsWithID.Add(key, view);
            }
            else
            {
                Debug.LogError("View c типом " + type + " и идентификатором " + key + " уже был зарегистрирован");
            }
        }
        else
        {
            if (!viewsWithoutID.ContainsKey(type))
            {
                viewsWithoutID.Add(type, view);
            }
            else
            {
                Debug.LogError("View c типом " + type + " и идентификатором " + type + " уже был зарегистрирован");
            }
        }

    }

    public T GetView<T>() where T : View
    {
        var type = typeof(T);

        if (viewsWithoutID.TryGetValue(type, out View view))
        {
            return (T)view;
        }

        Debug.Log("View типа " + type + " не был найден");
        return null;
    }

    public T GetView<T>(string ID) where T : View
    {
        var type = (typeof(T), ID);

        if (viewsWithID.TryGetValue(type, out View view))
        {
            return (T)view;
        }

        Debug.Log("View типа " + type + " не был найден");
        return null;
    }
}

public interface IIdentify
{
    string GetID();
}
