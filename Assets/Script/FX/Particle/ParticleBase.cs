using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBase : MonoBehaviour
{
    [SerializeField] private float ExitTime;

    private void Update()
    {
        Debug.Log(1);

        ExitTime -= Time.deltaTime;

        if(ExitTime < 0)
            Destroy(gameObject);
    }
}
