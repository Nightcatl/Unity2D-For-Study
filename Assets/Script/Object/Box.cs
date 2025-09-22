using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject text;
    public GameObject[] dropItems;

    private bool CanOpen = true;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null && CanOpen)
        {
            text.SetActive(true);

            collision.GetComponent<Player>().Box.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            text.SetActive(false);

            collision.GetComponent<Player>().Box.Remove(this);
        }
    }

    public void OpenBox()
    {
        if (!CanOpen)
            return;

        anim.SetBool("Close", false);
        anim.SetBool("Open", true);

        CanOpen = false;
    }

    private void DropItem()
    {
        foreach (var item in dropItems)
        {
            GameObject dropitem = Instantiate(item, transform.position, Quaternion.identity);

            int x = Random.Range(0, 1) < 0.5? 1 : -1;

            dropitem.GetComponent<ItemObject>().MoveItem(new Vector2(5 * x, 7));
        }
    }
}
