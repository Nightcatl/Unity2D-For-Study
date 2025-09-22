using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireMark : MonoBehaviour
{
    public AnimationCurve curve;

    [SerializeField] private Sprite[] fireMarkImage;
    [SerializeField] private GameObject fireExplosionPrefab;
    private Transform parent;
    private FireExplosion fireExplosion;

    private bool CanInfect;
    private int imageType;
    [SerializeField] private float duration;
    [SerializeField] private float changeSpeed;
    [SerializeField] private float changeTime;

    [SerializeField] private float currentChangeTime;
    private float Timer;

    private void Start()
    {
        parent = transform.parent;
        currentChangeTime = changeTime;
        Timer = currentChangeTime;
        imageType = 0;

        parent.GetComponent<CharacterStats>().SetMark(MarkType.Fire);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        Timer -= Time.deltaTime;
            
        if(duration > 0)
        {
            if (Timer <= 0)
            {
                ChangeImage();
                float progress = duration / 4f;
                currentChangeTime = changeTime * curve.Evaluate(progress);
                Timer = currentChangeTime;
            }
        }
        else
        {
            Explosion();
        }
    }

    public void Explosion()
    {
        fireExplosion = Instantiate(fireExplosionPrefab, parent.transform.position, Quaternion.identity, parent).GetComponent<FireExplosion>();
        fireExplosion.SetUpMark(CanInfect);
        parent.GetComponent<CharacterStats>().SetMark(MarkType.None);
        Destroy(gameObject);
    }

    private void ChangeImage()
    {
        if(imageType == 3)
        {
            parent.GetComponent<SpriteRenderer>().sprite = fireMarkImage[imageType];
            imageType = 0;
        }
        else
        {
            parent.GetComponent<SpriteRenderer>().sprite = fireMarkImage[imageType + 1];
            imageType++;
        }
            
    }

    public void SetUpMark(bool _CanInfect)
    {
        CanInfect = _CanInfect;
    }
}
