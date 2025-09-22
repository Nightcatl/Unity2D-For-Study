using System;
using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected TextMeshProUGUI text;

    [SerializeField] public SceneField currentScene;

    private Player player;
    private bool CanSetActive;
    public bool active;
    public string checkPointId;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    public void UseCheckPoint()
    {
        if(CanSetActive)
        {
            CheckPointManager.instance.EnableCheckPoint(this);
        }
    }

    public void SetAnim()
    {
        anim.SetBool("Active", active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            player = collision.gameObject.GetComponent<Player>();

            text.gameObject.SetActive(true);
            CanSetActive = true;

            collision.gameObject.GetComponent<Player>().NearCheckPoint = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            text.gameObject.SetActive(false);
            CanSetActive = false;

            player.NearCheckPoint = null;
        }
    }

    [ContextMenu("SetId")]
    private void SetId()
    {
        checkPointId = Guid.NewGuid().ToString();
    }
}
