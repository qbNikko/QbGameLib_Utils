using UnityEngine;
using UnityEngine.Events;

namespace QbGameLib_Utils.Component.Mb
{
    /**
     * Единицы измерения
     */
    public enum TimeUnit
    {
        Millisecond, 
        Second, 
        Minute, 
        Hour
    }
    /**
     * Реализация таймера
     */
    public class TimerComponent : MonoBehaviour
    {
        /**
         * Событие прохождения таймера
         */
        public UnityEvent OnTimeout;
        /**
         * Единицы измерения для установки таймера
         */
        [SerializeField] TimeUnit _timeUnit;
        /**
         * Время таймера
         */
        [SerializeField, Range(0,1000)] private float timer;
        /**
         * Зацикленный таймер
         */
        [SerializeField] public bool Loop;

        private float _currentTime;
        private float _waitTime;

        private void Awake()
        {
            InitTimer();
        }

        public TimeUnit TimeUnit
        {
            get => _timeUnit;
            set
            {
                _timeUnit = value;
                InitTimer();
            }
        }

        public float Timer
        {
            get => timer;
            set
            {
                timer = value;
                InitTimer();
            }
        }
        
        /**
         * Инициализация времени таймера в зависимости от единиц измерения
         */
        private void InitTimer()
        {
            if (_timeUnit == TimeUnit.Second) _waitTime = timer;
            else if (_timeUnit == TimeUnit.Millisecond) _waitTime = timer/1000;
            else if (_timeUnit == TimeUnit.Minute) _waitTime = timer*60;
            else if (_timeUnit == TimeUnit.Hour) _waitTime = timer*60*60;
        }

        public void Reset()
        {
            _currentTime = 0;
        }

        private void Update()
        {
            _currentTime+=Time.deltaTime;
            if (_currentTime >= _waitTime)
            {
                _currentTime = 0;
                OnTimeout.Invoke();
                if(!Loop) enabled = false;
            }
        }
    }
}