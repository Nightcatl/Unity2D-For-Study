using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer_NPC : MonoBehaviour
{
    [SerializeField] private NPC npc;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().NpcCandidates.Add(npc);

            if (collision.GetComponent<Player>().NPC == null)
            {
                collision.GetComponent<Player>().NPC = npc;
                npc.SetCanTalk(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().NpcCandidates.Remove(npc);
            npc.SetCanTalk(false);

            if (collision.GetComponent<Player>().NPC == npc)
            {
                collision.GetComponent<Player>().ChangeNPC();
            }
        }
    }
}
