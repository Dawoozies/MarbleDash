using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class BoxTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerStay;
    public UnityEvent onTriggerExit;
    public string[] validTags;
    private void OnTriggerEnter(Collider col)
    {
        string tag = col.tag;
        if (validTags.Contains(tag))
        {
            onTriggerEnter?.Invoke();
        }
    }
    private void OnTriggerStay(Collider col)
    {
        string tag = col.tag;
        if (validTags.Contains(tag))
        {
            onTriggerStay?.Invoke();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        string tag = col.tag;
        if (validTags.Contains(tag))
        {
            onTriggerExit?.Invoke();
        }
    }
}
