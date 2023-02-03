using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeMapSizeInvoke : MonoBehaviour
{
    [SerializeField] 
    private MapSizeType _mapSizeType;
    
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(InvokeChangeMapSize);
    }

    private void InvokeChangeMapSize()
    {
        MenuController.updateMapSizeTypeText?.Invoke(_mapSizeType);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
