using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected ItemData itemData;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatisGround;

    private bool IsGround;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
    }

    protected virtual void Start()
    {
    }

    private void Update()
    {
        if(rb.velocity.y <= 0 && !IsGround && IsGroundDetected())
        {
            rb.gravityScale = 0;
            IsGround = true;

            rb.velocity = Vector2.zero;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);

            switch (itemData.itemType)
            {
                case ItemType.Material:
                    Inventory.instance.AddStash(itemData);
                    break;
                case ItemType.Equipment:
                    Inventory.instance.PickUpEquipment(itemData as ItemData_Equipment);
                    break;
            }
        }
    }

    public void MoveItem(Vector2 moveSpeed)
    {
        Debug.Log(moveSpeed);

        rb.velocity = moveSpeed; 
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatisGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}

