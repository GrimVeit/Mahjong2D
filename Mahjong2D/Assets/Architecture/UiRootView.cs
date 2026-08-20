using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UIRootView : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform uiSceneContainer;

    private void Awake()
    {
        loadingScreen.SetActive(false);
    }

    public async UniTask ShowLoadingScreen(LoadingType loadingType)
    {
        loadingScreen.SetActive(true);
        await UniTask.Delay(300, DelayType.UnscaledDeltaTime);
    }

    public async UniTask HideLoadingScreen(LoadingType loadingType)
    {
        await UniTask.Delay(300, DelayType.UnscaledDeltaTime);
        loadingScreen.SetActive(false);
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
