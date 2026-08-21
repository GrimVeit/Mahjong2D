using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UIRootView : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CircleTransitionUI circleTransitionUI;
    [SerializeField] private Transform uiSceneContainer;

    private void Awake()
    {
        circleTransitionUI.Hide();

        circleTransitionUI.Initialize();
    }

    public async UniTask ShowLoadingScreen(LoadingType loadingType)
    {
        circleTransitionUI.Show();

        await UniTask.Delay(600, DelayType.UnscaledDeltaTime);
    }

    public async UniTask HideLoadingScreen(LoadingType loadingType)
    {
        circleTransitionUI?.Hide();

        await UniTask.Delay(600, DelayType.UnscaledDeltaTime);
    }

    public void AttachSceneUI(GameObject sceneUI, Camera camera)
    {
        ClearSceneUI();

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = camera;

        sceneUI.transform.SetParent(uiSceneContainer, false);
        sceneUI.transform.localScale = Vector3.one;

        RectTransform rect = sceneUI.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private void ClearSceneUI()
    {
        for (int i = 0; i < uiSceneContainer.childCount; i++)
        {
            GameObject.Destroy(uiSceneContainer.GetChild(i).gameObject);
        }
    }
}
