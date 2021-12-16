using System;
using UnityEngine;
using UnityEngine.Events;


public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            if (value < 0) value = 0;
            onHealthChanged?.Invoke(_health, value);
            _health = value;
            if(_health == 0 && !isDeath) {
                OnDeath.Invoke();
                isDeath = true;
			}
        }
    }
    private bool isDeath = false;
    [SerializeField] protected UnityEvent<float, float> onHealthChanged;
    [SerializeField] protected UnityEvent OnDeath;

    public void addOnHealthChangeListener(UnityAction<float, float> action)
    {
        onHealthChanged.AddListener(action);
    }
}
