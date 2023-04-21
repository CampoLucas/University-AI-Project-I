using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It has the function of storing the mediator
/// </summary>
public class BaseComponent : MonoBehaviour
{
    protected IMediator Mediator;
    
    /// <summary>
    /// Changes the mediator class
    /// </summary>
    /// <param name="mediator"> Any class that has the IMediator interface </param>
    public void SetMediator(IMediator mediator) => Mediator = mediator;
}
