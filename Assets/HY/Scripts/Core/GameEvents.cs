using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents {
    public static Action OnGameStarted;
    public static Action OnGameOver;
    public static Action OnGamePaused;
    public static Action OnGameResumed;
    public static Action OnSelectionOpened;
    public static Action OnSelectionClosed;
    public static Action<string> OnRequestSceneChange;
}
