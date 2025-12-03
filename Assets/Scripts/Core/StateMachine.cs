using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine<T>
{
    public T State { get; private set; }
    private readonly Action<T, T> _onChanged;
    public StateMachine(T initial, Action<T, T> onChanged = null)
    {
        State = initial; _onChanged = onChanged;
    }
    public void Change(T next)
    {
        if (Equals(State, next)) return;
        var prev = State; State = next; _onChanged?.Invoke(prev, next);
    }
}