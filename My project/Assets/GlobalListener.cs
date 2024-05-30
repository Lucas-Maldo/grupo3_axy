using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void OnNotifyWin();
    void OnNotifyLose();
    void OnNotifyAudio();
}

public class GlobalListener : MonoBehaviour
{
    public static GlobalListener instance;

    private List<IObserver> observers = new List<IObserver>();
    private void Awake()
    {
        instance = this;
    }
    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyWin()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotifyWin();
        }
    }

    public void NotifyLose()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotifyLose();
        }
    }

    public void NotifyAudio()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotifyAudio();
        }
    }
}