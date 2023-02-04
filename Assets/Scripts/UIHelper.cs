using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    public static Action<CanvasGroup, GameObject> hideCanvasEvent;
    public static Action<CanvasGroup, GameObject> showCanvasEvent;
    public static Action<CanvasGroup, GameObject> hideCanvasAndDeleteObjEvent;

    public static Action<Text, string> showWarning;

    public void Initialize()
    {
        hideCanvasEvent += HideCanvasGroup;
        showCanvasEvent += ShowCanvasGroup;
        hideCanvasAndDeleteObjEvent += HideCanvasAndDeleteObj;
            
        showWarning += ShowWarning;
    }

    /// <summary>
    /// Нужно чтобы подсвечиваемый текст был
    /// изначально прозрачным.
    /// </summary>
    private void ShowWarning(Text text, string value = "")
    {
        StartCoroutine(ShowWarningCoroutine(text, value));
    }
    
    private IEnumerator ShowWarningCoroutine(Text text, string value)
    {
        if (value!= "")
        {
            text.text = value;
        }
        
        var alph = 1f;
        text.color = new Color(1f, 0f, 0f, alph);

        while (alph > 0)
        {
            alph -= Time.deltaTime;
            text.color = new Color(1f, 0f, 0f, alph);
            yield return null;
        }
    }

    private void HideCanvasGroup(CanvasGroup canvasGroup, GameObject gameObject)
    {
        StartCoroutine(HideCanvasGroupCoroutine(canvasGroup, gameObject));
    }
    
    private void ShowCanvasGroup(CanvasGroup canvasGroup, GameObject gameObject)
    {
        StartCoroutine(ShowCanvasGroupCoroutine(canvasGroup, gameObject));
    }

    private void HideCanvasAndDeleteObj(CanvasGroup canvasGroup, GameObject gameObject)
    {
        StartCoroutine(HideCanvasAndDeleteObjCoroutine(canvasGroup, gameObject));
    }

    private IEnumerator HideCanvasGroupCoroutine(CanvasGroup canvasGroup, GameObject gameObject)
    {
        var alpha = canvasGroup.alpha;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        if (gameObject)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowCanvasGroupCoroutine(CanvasGroup canvasGroup, GameObject gameObject)
    {
        if (gameObject)
        {
            gameObject.SetActive(true);
        }
        
        var alpha = canvasGroup.alpha;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }
    
    private IEnumerator HideCanvasAndDeleteObjCoroutine(CanvasGroup canvasGroup, GameObject gameObject)
    {
        var alpha = canvasGroup.alpha;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        hideCanvasEvent -= HideCanvasGroup;
        showCanvasEvent -= ShowCanvasGroup;
        hideCanvasAndDeleteObjEvent -= HideCanvasAndDeleteObj;
    }
}
