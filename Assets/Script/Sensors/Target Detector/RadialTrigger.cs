using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    public float radius = 1f;
    public Transform target;
    [SerializeField] Transform center;
    public bool inside = false;
    public bool showGiz;

    private void Start()
    {
        center = transform;
    }

    private void Update()
    {
        RadialDetection();
    }

    void RadialDetection()
    {
        float dist = Vector3.Distance(center.position, target.position);
        inside= dist <= radius;
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if(!showGiz) return;
        if(center == null) return;

        Gizmos.color = inside ? Color.red : Color.white;   
        Gizmos.DrawWireSphere(center.position, radius);


    }



}
