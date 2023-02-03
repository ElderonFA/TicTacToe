using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class PauseButtonImageSwitch : MonoBehaviour
{
    [SerializeField] 
    private Sprite pausedSprite;
    [SerializeField] 
    private Sprite notPausedSprite;

    private Image image;
    private Button button;

    private bool isPaused;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SwitchImage);
    }

    private void SwitchImage()
    {
        image.sprite = isPaused ? pausedSprite : notPausedSprite;
        isPaused = !isPaused;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
