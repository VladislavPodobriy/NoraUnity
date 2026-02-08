using System.Collections;
using UnityEngine;

namespace MainScripts.VFX
{
    public class SoundWithLifetime : MonoBehaviour
    {
        private float _lifetimeSeconds;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    
        private IEnumerator DestroyAfterLifetime()
        {
            yield return new WaitForSeconds(_lifetimeSeconds);
            Destroy(gameObject);
        }
    
        public void PlayOneShot(AudioClip clip, float volume = 1f)
        {
            _audioSource.PlayOneShot(clip, volume);
            _lifetimeSeconds = clip.length;
            StartCoroutine(DestroyAfterLifetime());
        }
    }
}
