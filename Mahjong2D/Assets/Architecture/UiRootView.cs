using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UIRootView : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private FadePanel fadeBlackPanel;
    [SerializeField] private FadePanel fadeStaticPanel;
    [SerializeField] private CircleTransitionUI circleTransitionUI;
    [SerializeField] private Transform uiSceneContainer;

    public UniTask Initialize()
    {
        fadeBlackPanel.Initialize();

        return circleTransitionUI.Initialize();
    }

    public async UniTask ShowLoadingScreen(LoadingType loadingType)
    {
        switch (loadingType)
        {
            case LoadingType.Black:

                fadeBlackPanel.Show();
                await UniTask.Delay(100);

                return;
            case LoadingType.Start:

                fadeStaticPanel.Show();
                await UniTask.Delay(100);

                return;
            case LoadingType.Default:
                await circleTransitionUI.Show();
                return;
        }
    }

    public async UniTask HideLoadingScreen(LoadingType loadingType)
    {
        switch (loadingType)
        {
            case LoadingType.Black:

                fadeBlackPanel.Hide();

                return;
            case LoadingType.Start:

                fadeStaticPanel.Hide();

                return;
            case LoadingType.Default:
                await circleTransitionUI.Hide();
                return;
        }
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
