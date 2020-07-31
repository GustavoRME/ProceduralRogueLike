using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void OnNotify(ObserverEvent obsEvent);    
}

public enum ObserverEvent
{
    FirstInput,
    FirstWall,
    FirstEnemy,
    FirstFood,
    FirstLevel10,
    FirstLevel15,
    FirstRespawnPoint,
    FirstDeath
}

