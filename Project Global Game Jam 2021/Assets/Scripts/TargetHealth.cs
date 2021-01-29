using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealth : EntityHealth
{
    [SerializeField] protected Color colorHit;
    [SerializeField] protected float colorDuration;

    protected MeshRenderer selfMeshRenderer;
    protected Color colorOriginal;

    public override void TakeDamage(int damage)
    {
        StartCoroutine(ColorTimer());
    }

    protected override void Start()
    {
        base.Start();

        selfMeshRenderer = GetComponent<MeshRenderer>();
        colorOriginal = selfMeshRenderer.material.color;
    }

    protected IEnumerator ColorTimer()
    {
        selfMeshRenderer.material.color = colorHit;
        yield return new WaitForSeconds(colorDuration);
        selfMeshRenderer.material.color = colorOriginal;
    }
}
