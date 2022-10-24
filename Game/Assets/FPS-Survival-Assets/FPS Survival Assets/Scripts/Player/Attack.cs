using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 2f;
    public float radius = 1f;
    public LayerMask layerMask;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius,layerMask);

        if(hits.Length > 0)
        {
            print("We touched: " + hits[0].gameObject.tag);

            gameObject.SetActive(false);
        }
    }
}
