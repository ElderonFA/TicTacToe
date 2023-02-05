using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterNamePopUp : MonoBehaviour, PopUp
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Button confirmButton;
    [SerializeField] 
    private InputField inputField;

    [Space] 
    [SerializeField] private Text headerText;

    public Action<string> onConfirm;
    public Action onDestroy;

    private void Start()
    {
        confirmButton.onClick.AddListener(Confirm);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UIHelper.showCanvasEvent?.Invoke(canvasGroup, null);
    }

    private void Confirm()
    {
        onConfirm?.Invoke(inputField.text);
        Close();
    }

    public void Close()
    {
        UIHelper.hideCanvasAndDeleteObjEvent(canvasGroup, gameObject);
    }

    public void SetHeaderText(string text)
    {
        headerText.text = text;
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
        confirmButton.onClick.RemoveAllListeners();
    }
}
