using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRefresh : MonoBehaviour
{
    public float spinSpeed;
    Vector3 eulerAngle;
    public float collectionCooldownTime;
    float t;
    public Material[] materials;
    MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        eulerAngle.x = spinSpeed * Time.deltaTime;
        transform.Rotate(eulerAngle);

        if(t > 0)
        {
            meshRenderer.material = materials[1];
            t -= Time.deltaTime;
        }
        else
        {
            meshRenderer.material = materials[0];
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && t <= 0)
        {
            other.GetComponent<Player>().DashRefresh();
            t = collectionCooldownTime;
        }
    }
}
