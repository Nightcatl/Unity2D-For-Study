using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private Player player;

    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected ParticleSystem[] particles;

    [Header("Screen shake FX")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultipier;
    [SerializeField] private Vector3 shakePower;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Aliment colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] dampColor;
    [SerializeField] private Color[] shockColor;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        screenShake = GetComponent<CinemachineImpulseSource>();
        originalMat = sr.material;
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    public void PlayHitFX()
    {
        StartCoroutine("FlashFX");
    }

    public void ScreenShake()
    {
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * player.facingDir, shakePower.y) * shakeMultipier;
        screenShake.GenerateImpulse();
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    public void CancelColorChange()
    {
        CancelInvoke("IgniteColorFx");
        sr.color = Color.white;
    }

    public void IgniteFxFor()
    {
        InvokeRepeating("IgniteColorFx", 0, .3f);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    public void DampFxFor()
    {
        InvokeRepeating("DampColorFx", 0, .3f);
    }

    private void DampColorFx()
    {
        if (sr.color != dampColor[0])
            sr.color = dampColor[0];
        else
            sr.color = dampColor[1];
    }

    public void ShockFxFor()
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void StartParticles(int Index, Vector2 position)
    {
        Instantiate(particles[Index], position, Quaternion.identity);
    }
}
