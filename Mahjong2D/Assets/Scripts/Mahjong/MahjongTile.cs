using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MahjongTile :
    MonoBehaviour,
    IPointerClickHandler
{
    [SerializeField]
    private Image background;


    private int id;


    public void Initialize(
        int id)
    {
        this.id = id;
    }


    public void SetActiveVisual(
        bool isActive)
    {
        if (background == null)
            return;


        Color color =
            background.color;


        if (isActive)
        {
            color.r = 1f;
            color.g = 1f;
            color.b = 1f;
            color.a = 1f;
        }
        else
        {
            color.r = 0.5f;
            color.g = 0.5f;
            color.b = 0.5f;
            color.a = 0.7f;
        }


        background.color =
            color;
    }


    public void OnPointerClick(
        PointerEventData eventData)
    {
        OnClick?.Invoke(
            id
        );
    }


    public event Action<int>
        OnClick;
}