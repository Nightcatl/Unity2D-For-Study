using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    [SerializeField] protected GameObject Magic;
    private Player player;
    private AttributeType magicType;
    public bool isOver;
    public int facingDir;
    [SerializeField] private SkillLevelType skillType;

    [Header("Flame Rays")]
    public float flameRays_duration;
    private float flameRays_Damage;
    private float flameRays_Probability;
    private float flameRays_Poisedamage;
    private Vector2 flameRays_KnockPower;
    private GameObject flameRays;
    [SerializeField] private GameObject flameRaysPrefab;

    public void setUpClone(float _magicdamage, float _probability, float _poisedamage, Vector2 _knockPower, Player _player, AttributeType _magicType, float _flameRays_duration, int _facingDir, SkillLevelType _skillType)
    {
        flameRays_Damage = _magicdamage;
        flameRays_Probability = _probability;
        flameRays_Poisedamage = _poisedamage;
        flameRays_KnockPower = _knockPower;
        player = _player;
        magicType = _magicType;
        flameRays_duration = _flameRays_duration;
        facingDir = _facingDir;
        skillType = _skillType;
    }

    private void Start()
    {
        if(transform.localScale.x != facingDir)
            transform.Rotate(0, 180, 0);

        flameRays = Instantiate(flameRaysPrefab, Magic.transform.position, Quaternion.identity, transform);

        FlameRays_Skill_Controller fireball_Skill_Controller = flameRays.GetComponent<FlameRays_Skill_Controller>();

        fireball_Skill_Controller.SetupSkill(flameRays_Damage, flameRays_Probability, flameRays_Poisedamage, flameRays_KnockPower, true, 0, player, magicType, skillType);
    }

    private void Update()
    {
        if(isOver)
            Destroy(gameObject);
    }

}
