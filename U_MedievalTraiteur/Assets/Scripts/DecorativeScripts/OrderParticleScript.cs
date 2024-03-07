using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OrderParticleScript : MonoBehaviour
{
    private ParticleSystemRenderer _particleSystemRenderer = null;
    [SerializeField] private int sortingAddition = 0;

    // Start is called before the first frame update
    void Start()
    {
        _particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Order();
    }

    private void Order()
    {
        _particleSystemRenderer.sortingOrder = -(int)(transform.position.z*100f)+sortingAddition;
    }
}
