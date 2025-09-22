using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private Slider healthbar;
    [SerializeField] private Slider healthbar_boss;
    [SerializeField] private Slider vitalitybar;
    [SerializeField] private GameObject magicPointParent;
    [SerializeField] private GameObject magicPointPrefab;

    private EnemyStat boss;
    
    private List<GameObject> magicPoint;
    private float magicpoint_lost;

    void Start()
    {
        magicPoint = new List<GameObject>();

        if(playerStat != null)
        {
            playerStat.onHealthChanged += UpdateHealthUI;
            playerStat.onVitalityChanged += UpdateVitalityUI;
            playerStat.onMagicpointChanged += UpdateMagicPointUI;
            playerStat.onMagicpointAmountChanged += UpdateMagicPointUI;
        }

        UpdateMagicPointAmount();

        UpdateHealthUI();
        UpdateVitalityUI();
        UpdateMagicPointUI();
    }

    void Update()
    {
        
    }

    private void UpdateHealthUI()
    {
        healthbar.maxValue = playerStat.MaxHealth.GetValue();
        healthbar.value = playerStat.currentHealth;
    }

    private void UpdateHealthUI_Boss()
    {
        healthbar_boss.maxValue = boss.MaxHealth.GetValue();
        healthbar_boss.value = boss.currentHealth;
    }

    private void UpdateVitalityUI()
    {
        vitalitybar.maxValue = playerStat.Vitality.GetValue();
        vitalitybar.value = playerStat.currentVitality;
    }

    private void UpdateMagicPointUI()
    {
        if (magicPoint.Count == 0)
            return;

        RectMask2D mark;

        for (int i = 0; i < magicPoint.Count; i++)
        {
            if(i < (int)playerStat.currentMagicpoint)
            {
                magicPoint[i].SetActive(true);
                magicPoint[i].GetComponent<RectMask2D>().softness = new Vector2Int(0, 0);
                magicPoint[i].GetComponent<RectMask2D>().padding = Vector4.zero;
            }
            else if(i == (int) playerStat.currentMagicpoint)
            {
                magicPoint[i].SetActive(true);
            }
            else
                magicPoint[i].SetActive(false);
        }

        if(playerStat.currentMagicpoint > magicPoint.Count)
            playerStat.currentMagicpoint = magicPoint.Count;

        mark = magicPoint[Mathf.CeilToInt(playerStat.currentMagicpoint) - 1].GetComponent<RectMask2D>();

        float right;

        if((playerStat.currentMagicpoint % 1) != 0)
            right = 50 - (playerStat.currentMagicpoint % 1) * 50;
        else
            right = 0;

        if (right != 0)
            mark.softness = new Vector2Int(100, 0);
        else
            mark.softness = new Vector2Int(0, 0);

        mark.padding = new Vector4(0, 0, right, 0);
    }

    private void UpdateMagicPointAmount()
    {
        if(magicPoint.Count < playerStat.Magicpoint.GetValue())
        {
            for(int i = magicPoint.Count; i < playerStat.Magicpoint.GetValue(); i++)
            {
                GameObject newMagicpoint = Instantiate(magicPointPrefab, magicPointParent.transform);

                newMagicpoint.GetComponent<RectMask2D>().padding = new Vector4(0,0,0,0);

                magicPoint.Add(newMagicpoint);
            }
        }

        if(magicPoint.Count > playerStat.Magicpoint.GetValue())
        {
            for (int i = playerStat.Magicpoint.GetValue(); i < magicPoint.Count; i++)
            {
                GameObject oldMagicpoint = magicPoint[i];

                magicPoint.Remove(magicPoint[i]);

                Destroy(oldMagicpoint);
            }
        }
    }

    public void ShowBossHealth(EnemyStat _boss)
    {
        healthbar_boss.gameObject.SetActive(true);

        boss = _boss;

        boss.onHealthChanged += UpdateHealthUI_Boss;
    }

    public void HideBossHealth()
    {
        healthbar_boss.gameObject.SetActive(false);

        boss.onHealthChanged -= UpdateHealthUI_Boss;

        boss = null;
    }
}
