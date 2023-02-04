using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string name;
    public string GetName => name;
    public void SetName(string newName)
    {
        name = newName;
    }
    
    private bool isComputer;
    public bool IsComputer => isComputer;
    public void ChangeToComputer()
    {
        isComputer = true;
    }

    public Sprite moveIcon;

    public Player()
    {
        this.isComputer = false;
    }
}
