using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MainScripts.GenericComponents
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerComponent : MonoBehaviour
    {
        public UnityEvent<Collider2D> OnTriggerEnter;
        public UnityEvent<Collider2D> OnTriggerStay;
        public UnityEvent<Collider2D> OnTriggerExit;

        [SerializeField] private List<string> _tags = new();

        private Collider2D _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            OnTriggerEnter = new UnityEvent<Collider2D>();
            OnTriggerExit = new UnityEvent<Collider2D>();
            OnTriggerStay = new UnityEvent<Collider2D>();
        }
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_tags.Contains(col.tag) || _tags.Count == 0)
                OnTriggerEnter.Invoke(col);
        }
    
        private void OnTriggerStay2D(Collider2D col)
        {
            if (_tags.Contains(col.tag)  || _tags.Count == 0)
                OnTriggerStay.Invoke(col);
        }
    
        private void OnTriggerExit2D(Collider2D col)
        {
            if (_tags.Contains(col.tag)  || _tags.Count == 0)
                OnTriggerExit.Invoke(col);
        }

        public void SetTag(string tag)
        {
            _tags = new List<string> { tag };
        }
        
        public void SetTags(List<string> tags)
        {
            _tags = tags;
        }
        
        public Collider2D[] GetOverlappedColliders()
        {
            var results = new Collider2D[1];
            _collider.OverlapCollider(new ContactFilter2D().NoFilter(), results);
            return results.Where(x => _tags.Contains(x.tag)).ToArray();
        }
    }
}
