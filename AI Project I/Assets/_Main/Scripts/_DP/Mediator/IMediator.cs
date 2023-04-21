using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mediator interface
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Used to notify the mediator of various events
    /// </summary>
    /// <param name="sender"> The one who notified </param>
    /// <param name="ev"> The event name, preferably it is written in uppercase </param>
    void Notify(object sender, string ev);
}
