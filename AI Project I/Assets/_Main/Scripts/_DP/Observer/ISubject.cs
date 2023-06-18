using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    List<IObserver> Subscribers { get; }
    void Subscribe(IObserver observer);
    void Unsubscribe(IObserver observer);
    void NotifyAll(string message, params object[] args);
}
