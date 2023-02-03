using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeGamemodeInvoke : MonoBehaviour
{
    [SerializeField] 
    private GameModeType _gameModeType;
    
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(InvokeChangeMapSize);
    }

    private void InvokeChangeMapSize()
    {
        MenuController.updateGamemodeTypeText?.Invoke(_gameModeType);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
