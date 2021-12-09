using System;
using UnityEngine;
using UnityEngine.Events;


public abstract class Entity : MonoBehaviour
{
    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            onHealthChanged?.Invoke(_health, value);
            _health = value;
        }
    }
    [SerializeField] protected UnityEvent<float, float> onHealthChanged;

    public void addOnHealthChangeListener(UnityAction<float, float> action)
    {
        onHealthChanged.AddListener(action);
    }
}
