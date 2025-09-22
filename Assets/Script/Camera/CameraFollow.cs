using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField] private float flipYRotatrionTime = 0.5f;

    private Coroutine _turnCoroutine;

    private Player player;

    private bool facingRight;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        player = PlayerManager.instance.player;

        facingRight = player.facingRight;
    }

    private void Update()
    {
        transform.position = player.transform.position;
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < flipYRotatrionTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotatrionTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        facingRight = !facingRight;

        if(!facingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
