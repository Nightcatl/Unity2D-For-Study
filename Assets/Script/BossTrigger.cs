using System.Collections;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject BossPrefab;
    [SerializeField] private GameObject Wall;
    [SerializeField] private Transform BossPosition;
    [SerializeField] private string LevelName;
    [SerializeField] private string[] context_Start;
    [SerializeField] private string[] context_End;

    private Enemy boss;
    private BoxCollider2D cd;

    private void Awake()
    {
        cd = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Wall.SetActive(true);

            boss = Instantiate(BossPrefab, BossPosition.position, Quaternion.identity, this.transform).GetComponent<Enemy>();

            LevelManager.instance.StartBossFight(LevelName,boss, context_Start);

            boss.GetComponent<Enemy_RedHood>().bossManager = this;

            cd.enabled = false;
        }
    }

    public void WinTheFight()
    {
        Wall.SetActive(false);

        LevelManager.instance.EndBossFight(LevelName, boss, context_End);

        Destroy(boss.gameObject);
    }
}
