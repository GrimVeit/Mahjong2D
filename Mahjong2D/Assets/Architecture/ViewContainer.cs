using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewContainer : MonoBehaviour
{
    private readonly Dictionary<Type, MonoBehaviour> viewsWithoutID = new Dictionary<Type, MonoBehaviour>();
    private readonly Dictionary<(Type, string), MonoBehaviour> viewsWithID = new Dictionary<(Type, string), MonoBehaviour>();

    public void Initialize()
    {
        viewsWithoutID.Clear();
        viewsWithID.Clear();

        var views = GetComponentsInChildren<MonoBehaviour>(true);

        foreach (var view in views)
        {
            RegisterView(view.GetType(), view);
        }
    }

    public void RegisterView(Type type, MonoBehaviour view)
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

    public T GetView<T>() where T : MonoBehaviour
    {
        var type = typeof(T);

        if (viewsWithoutID.TryGetValue(type, out MonoBehaviour view))
        {
            return (T)view;
        }

        Debug.Log("View типа " + type + " не был найден");
        return null;
    }

    public T GetView<T>(string ID) where T : MonoBehaviour
    {
        var type = (typeof(T), ID);

        if (viewsWithID.TryGetValue(type, out MonoBehaviour view))
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
