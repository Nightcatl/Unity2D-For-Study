using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeekPlayer : MonoBehaviour
{
    private Enemy enemy;

    private BoxCollider2D cd;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void SeekPlayer()
    {
        cd = GetComponent<BoxCollider2D>();
        cd.enabled = true;

        if(GetComponent<CircleCollider2D>() != null)
            GetComponent<CircleCollider2D>().enabled = false;

        Collider2D[] collders = Physics2D.OverlapBoxAll(transform.position, cd.size,0);

        foreach(Collider2D collider in collders)
        {
            if(collider.GetComponent<Player>() != null)
            {
                PlayerManager.instance.player.Body.Add(GetComponentInParent<Enemy>());
                enemy.text.gameObject.SetActive(true);

                if (enemy.facingRight)
                    enemy.text.transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    enemy.text.transform.rotation = Quaternion.Euler(180, 0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            enemy.FindPlayer = true;
        }

        if(enemy.IsDead)
        {
            if(collision.CompareTag("Player"))
            {
                PlayerManager.instance.player.Body.Add(GetComponentInParent<Enemy>());
                enemy.text.gameObject.SetActive(true);

                if (enemy.facingRight)
                    enemy.text.transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    enemy.text.transform.rotation = Quaternion.Euler(180, 0, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(enemy.IsDead)
        {
            if(collision.CompareTag("Player"))
            {
                PlayerManager.instance.player.Body.Remove(GetComponentInParent<Enemy>());
                enemy.text.gameObject.SetActive(false);
            }
        }
    }
}
