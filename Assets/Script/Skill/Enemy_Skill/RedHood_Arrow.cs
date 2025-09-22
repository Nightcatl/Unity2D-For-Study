using Unity.Mathematics;
using UnityEngine;

public class RedHood_Arrow : Enemy_Skill_Controller
{
    private Rigidbody2D rb;

    private float ExitTime;

    private float angle = 0;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        ExitTime = 2;

        CanDamage = true;
    }

    public void SetAngle(float _angle)
    {
        angle = _angle;
    }

    protected override void Update()
    {
        base.Update();

        SetVector(rb, new Vector2(skillData.MoveSpeed * Mathf.Cos(angle), skillData.MoveSpeed * Mathf.Sin(angle)));

        ExitTime -= Time.deltaTime;

        if(ExitTime <= 0)
            SelfDestroy();

        if(IsHit)
            SelfDestroy();
    }

    public void SetFacingDir(int dir)
    {
        facingDir = dir;
    }
}
