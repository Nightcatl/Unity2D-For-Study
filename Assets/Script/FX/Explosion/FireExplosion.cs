using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    [SerializeField] private GameObject fireMarkPrefab;
    private CapsuleCollider2D cd;

    private bool CanInfect;
    [SerializeField] private float totalMagicDamage;
    [SerializeField] private Vector2 knockPower;
    [SerializeField] private float poisedamage;
    [SerializeField] private float probability;
    [SerializeField] private AttributeType magicType;
    

    private void Start()
    {
        cd = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        AnimationExplodeEvent();
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(new Vector2(cd.bounds.center.x + cd.offset.x, cd.bounds.center.y + cd.offset.y), cd.size, cd.direction, 0);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (CanInfect && hit.GetComponent<CharacterStats>().mark == MarkType.None)
                {
                    FireMark fireMark = Instantiate(fireMarkPrefab, new Vector3(hit.transform.position.x, hit.transform.position.y + 1, 0), Quaternion.identity, hit.transform).GetComponent<FireMark>();
                    fireMark.SetUpMark(false);
                }

                hit.GetComponent<EnemyStat>().TakeMagicDamage(totalMagicDamage, knockPower, poisedamage, probability, magicType);
            }
        }
    }

    public void SetUpMark(bool _CanInfect)
    {
        CanInfect = _CanInfect;
    }

    private void SelfDestroy() => Destroy(gameObject);
}
