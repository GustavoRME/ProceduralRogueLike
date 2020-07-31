using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Subject
{
    protected List<IObserver> _observers;

    public Subject() => _observers = new List<IObserver>();        

    public void AddObserver(IObserver observer) => _observers.Add(observer);
    public void RemoveObserver(IObserver observer) => _observers.Remove(observer);
    public void Notify(ObserverEvent observerEvent)
    {
        for (int i = 0; i < _observers.Count; i++)
        {
            _observers[i].OnNotify(observerEvent);
        }
    }
    
}
