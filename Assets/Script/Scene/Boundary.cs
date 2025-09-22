using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boundary : MonoBehaviour
{
    [Header("Spawn To")]
    [SerializeField] private SceneField sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneSwapManager.instance.SceneSwap(sceneToLoad);
        }
    }
}
