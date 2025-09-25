using System.Collections;
using UnityEngine;

public class BreakFireMark : MonoBehaviour
{
    private SpriteRenderer sr;
    private CharacterStats stat;
    private Transform parent;
    private int currentProcess = 0;

    [SerializeField] private float ExitTime;

    private void Start()
    {
        parent = transform.parent;

        sr = GetComponent<SpriteRenderer>();
        stat = parent.GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if(stat.mark == MarkType.BreakFire)
        {
            StartCoroutine(RemoveFireResistance());
        }else if (currentProcess != stat.progress)
        {
            Color newColor = new Color(255, 0, 0, stat.progress * 2.55f);

            sr.color = newColor;

            currentProcess = stat.progress;
        }
    }

    private IEnumerator RemoveFireResistance()
    {
        int defaultValue = stat.Resistance[0].GetValue();
        stat.Resistance[0].SetDefaultValue(0);

        yield return new WaitForSeconds(ExitTime);

        stat.SetMark(MarkType.None);
        stat.Resistance[0].SetDefaultValue(defaultValue);
        Destroy(gameObject);
    }    
}
