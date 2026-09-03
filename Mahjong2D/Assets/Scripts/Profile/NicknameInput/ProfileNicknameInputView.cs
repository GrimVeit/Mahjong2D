using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileNicknameInputView : View
{
    [Header("Input")]
    [SerializeField] private TMP_InputField inputFieldNickname;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI textDescriptionError;
    [SerializeField] private UIEffect effectError;

    [Header("Save")]
    [SerializeField] private Button buttonSave;
    [SerializeField] private UIEffect effectSave;

    public void Initialize()
    {
        inputFieldNickname.onValueChanged.AddListener(HandleNicknameChanged);
        buttonSave.onClick.AddListener(HandleSubmitNickname);

        effectError.Initialize();
        effectSave.Initialize();
    }

    public void Dispose()
    {
        inputFieldNickname.onValueChanged.RemoveListener(HandleNicknameChanged);
        buttonSave.onClick.RemoveListener(HandleSubmitNickname);

        effectError.Dispose();
        effectSave.Dispose();
    }

    public void SetValidate()
    {
        buttonSave.interactable = true;

        if(!effectSave.IsActive)
            effectSave.PlayShow();

        if(effectSave.IsActive)
           effectError.PlayHide();
    }
    
    public void SetNotValidate(string text)
    {
        buttonSave.interactable = false;

        if(effectSave.IsActive)
            effectSave.PlayHide();

        if(textDescriptionError.text != text)
           textDescriptionError.text = text;

        if(!effectError.IsActive)
            effectError.PlayShow();
    }

    #region Output

    public event Action<string> OnChangeNickname;
    public event Action OnSubmitNickname;

    private void HandleNicknameChanged(string value)
    {
        OnChangeNickname?.Invoke(value);
    }

    private void HandleSubmitNickname()
    {
        OnSubmitNickname?.Invoke();
    }

    #endregion
}
