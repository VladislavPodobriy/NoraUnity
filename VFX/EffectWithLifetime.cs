using System.Collections;
using UnityEngine;

namespace MainScripts.VFX
{
    public class EffectWithLifetime : MonoBehaviour
    {
        [SerializeField] private float _lifetimeSeconds;

        private void Start()
        {
            StartCoroutine(DestroyAfterLifetime());
        }
    
        private IEnumerator DestroyAfterLifetime()
        {
            yield return new WaitForSeconds(_lifetimeSeconds);
            Destroy(gameObject);
        }
    }
}
