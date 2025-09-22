using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSquare_Controller: MonoBehaviour
{
    public static AimSquare_Controller instance;

    [SerializeField] private Vector2 launchForce;
    [SerializeField] private GameObject AimSquare;
    private Player player;
    private int facingdir = 1;

    public float Z;
    public Vector2 finalDir;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(instance);

        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if(facingdir != player.facingDir)
        {
            transform.Rotate(0, 180, 0);
            facingdir = player.facingDir;
        }

        finalDir = new Vector2(AimDirection().normalized.x, AimDirection().normalized.y);

        SquareDirection();
    }
    public Vector2 AimDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if ((mousePosition.x < player.transform.position.x && player.facingDir == 1) || (mousePosition.x > player.transform.position.x && player.facingDir == -1))
        {
            player.Flip();
        }

        Vector2 direction = mousePosition - (Vector2)transform.position;

        return direction;
    }

    private void SquareDirection()
    {
        Z = Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg;

        Vector3 currentEulerAngles = transform.localEulerAngles;

        currentEulerAngles.z = Z;

        transform.localEulerAngles = currentEulerAngles;

    }
}
