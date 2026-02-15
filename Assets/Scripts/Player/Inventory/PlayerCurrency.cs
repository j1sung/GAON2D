using System;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] private int _bits;
    public int Bits => _bits;

    public event Action<int> OnBitsChanged;

    // 비트 추가
    public void AddBits(int amount)
    {
        if (amount <= 0) return;
        _bits += amount;
        OnBitsChanged?.Invoke(_bits);
    }

    // 비트 지불 시도
    public bool TrySpendBits(int amount)
    {
        if (amount <= 0) return false;
        if (_bits < amount)
        {   
            Debug.Log("비트 부족!!");
            return false;
        }
        _bits -= amount;
        OnBitsChanged?.Invoke(_bits);
        return true;
    }
}