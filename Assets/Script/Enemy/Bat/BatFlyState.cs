using UnityEngine;

public class BatFlyState : EnemyState
{
    protected Enemy_Bat enemy;

    private Vector2 targetPosition;
    private Vector2 Speed;

    private bool CanChangeY;
    private float ChangeYTimer;
    private int flyDir;

    private float X;
    private float Y;

    public BatFlyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Bat _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void AnimationFinshTrigger()
    {
        base.AnimationFinshTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();

        stateTimer = enemy.moveTime + Random.Range(-0.3f, 0.3f);

        if (enemy.transform.position.y < enemy.MinHeight)
            ReturnToSky();
        /*else if(Vector2.Distance(enemy.transform.position, PlayerManager.instance.player.transform.position) > 10)
            CloseToPlayer();*/
        else
            RandomFly();
    }

    private void ReturnToSky()
    {
        CanChangeY = false;

        X = enemy.transform.position.x;

        Y = Random.Range(enemy.MinHeight, enemy.MaxHeight);

        targetPosition = new Vector2(X, Y);

        Vector2 findir = new(targetPosition.x - enemy.transform.position.x, targetPosition.y - enemy.transform.position.y);

        float Z = Mathf.Atan2(findir.normalized.x, findir.normalized.y);

        Speed = new Vector2(enemy.moveSpeed * Mathf.Sin(Z), enemy.moveSpeed * Mathf.Cos(Z));
    }

    private void CloseToPlayer()
    {
        X = PlayerManager.instance.player.transform.position.x + 2;

        ChangeYTimer = Random.Range(0.5f, 0.7f);

        Y = Random.Range(enemy.MinHeight, enemy.MaxHeight);
    }

    private void RandomFly()
    {
        /*if (enemy.IsWallDetected())
        {
            if (enemy.facingDir == 1)
            {
                X = Random.Range(enemy.transform.position.x - 6.5f, enemy.transform.position.x - 2f);
            }
            else
            {
                X = Random.Range(enemy.transform.position.x + 2f, enemy.transform.position.x + 6.5f);
            }
        }
        else
            X = Random.Range(enemy.transform.position.x - 6.5f, enemy.transform.position.x + 6.5f);

        if (enemy.IsGroundDetected())
        {
            Y = Mathf.Min(10, Random.Range(enemy.transform.position.y + 2f, enemy.transform.position.y + 6.5f));
        }
        else
            Y = Mathf.Min(10, Random.Range(enemy.transform.position.y - 6.5f, enemy.transform.position.y + 6.5f));*/

        flyDir = Random.Range(-1, 2);
        ChangeYTimer = Random.Range(0.5f, 0.7f);
        CanChangeY = true;

        if(PlayerManager.instance.player.transform.position.x - enemy.transform.position.x < 0)
            Speed = new Vector2(enemy.moveSpeed * 0.9f * -1, enemy.moveSpeed * 0.2f * flyDir);
        else
            Speed = new Vector2(enemy.moveSpeed * 0.9f, enemy.moveSpeed * 0.2f * flyDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        ChangeYTimer -= Time.deltaTime; 

        if(ChangeYTimer <= 0 && CanChangeY)
        {
            ChangeYTimer = Random.Range(0.5f, 0.7f);

            if (Mathf.Abs(PlayerManager.instance.player.transform.position.y - enemy.transform.position.y) > 6.5)
                flyDir = -1;
            else
                flyDir = Random.Range(-1, 2);

            if (PlayerManager.instance.player.transform.position.x - enemy.transform.position.x < 0)
                Speed = new Vector2(enemy.moveSpeed * 0.9f * -1, enemy.moveSpeed * 0.2f * flyDir);
            else
                Speed = new Vector2(enemy.moveSpeed * 0.9f, enemy.moveSpeed * 0.2f * flyDir);
        }

        enemy.SetVelocity(Speed.x, Speed.y);

        if(Mathf.Abs(targetPosition.y - enemy.transform.position.y) <= 0.1 && !CanChangeY)
            stateMachine.ChangeState(enemy.idleState);

        if (Mathf.Abs(PlayerManager.instance.player.transform.position.x - enemy.transform.position.x) <= 0.1 && CanChangeY && Mathf.Abs(PlayerManager.instance.player.transform.position.y - enemy.transform.position.y) < 6.5)
            stateMachine.ChangeState(enemy.idleState);
            
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
