using UnityEngine;

namespace Nora
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T Instance;
        [SerializeField] private bool _dontDestroyOnLoad = true;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (_dontDestroyOnLoad)
            {
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}