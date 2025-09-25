using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider2D cd;

    [Header("Spawn To")]
    [SerializeField] private SceneField sceneToLoad;
    [SerializeField] private SceneField sceneToUnload;
    private void Awake()
    {
        cd = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - cd.bounds.center).normalized;

            if(exitDirection.y < 0)
            {
                if (sceneToLoad.SceneName != "")
                    SceneSwapManager.instance.SceneToLoad(sceneToLoad);

                if (sceneToUnload.SceneName != "")
                    SceneManager.UnloadSceneAsync(sceneToUnload);

                SceneSwapManager.instance.CurrentScene(sceneToLoad);

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - cd.bounds.center).normalized;

            if(exitDirection.y < 0)
            {
                if (sceneToLoad.SceneName != "")
                    SceneManager.UnloadSceneAsync(sceneToLoad);

                if (sceneToUnload.SceneName != "")
                    SceneSwapManager.instance.SceneToLoad(sceneToUnload);

                SceneSwapManager.instance.RemoveScene(sceneToLoad);
            }
        }
    }
}
