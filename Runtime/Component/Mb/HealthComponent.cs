using System;
using QbGameLib_Utils.Reactive;
using UnityEngine;
using UnityEngine.Events;
using IInitialized = QbGameLib_Utils.Interface.IInitialized;

namespace QbGameLib_Utils.Component.Mb
{
    public class HealthComponent : MonoBehaviour, IInitialized
    {
        [SerializeField] private ReactiveProperty<int> _health = new ReactiveProperty<int>(100);
        [SerializeField] private ReactiveProperty<int> _maxHealth = new ReactiveProperty<int>(100);

        public IObservable<int> HealthObservable => _health;
        public IObservable<int> MaxHealthObservable => _maxHealth;

        public UnityEvent OnDead;
        public UnityEvent<int,int> OnHealthChanged;

        public void Initialize()
        {
            SetHealth(_maxHealth.Value);
        }

        public void Damage(int damage)
        {
            _health.Value = Mathf.Clamp(_health.Value-damage,0,_maxHealth.Value);
            OnHealthChanged.Invoke(_health.Value,-damage);
            if(_health.Value == 0) OnDead.Invoke();
        }

        public void Heal(int heal)
        {
            _health.Value = Mathf.Clamp(_health.Value+heal,0,_maxHealth.Value);
            OnHealthChanged.Invoke(_health.Value,heal);
        }

        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth.Value = maxHealth;
        }
        
        public void SetHealth(int health)
        {
            _health.Value = Mathf.Clamp(health,0,_maxHealth.Value);;
        }

        public int GetHealth()
        {
            return _health.Value;
        }

        public int GetMaxHealth()
        {
            return _maxHealth.Value;
        }
        
        
        
        
    }
}