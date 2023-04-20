using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Methods that all type of menu have in common
/// </summary>
public class Menu : BaseComponent
{
    /// <summary>
    /// Hides the menu
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Shows the menu
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}
