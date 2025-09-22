using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollideCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<PlayerStat>().TakeDamage(1, new Vector2(10,5), 0, -collision.GetComponent<Player>().facingDir);
        }
    }
}
