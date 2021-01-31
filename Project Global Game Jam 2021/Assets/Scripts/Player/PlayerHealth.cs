using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class PlayerHealth : EntityHealth
{
    public static PlayerHealth current;
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private TextMeshProUGUI healthText;

    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private Sequence damageFX;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        damageFX.Rewind();
        damageFX.Play();
        
        UpdateHealthText();
    }

    public override void TakeHealing(int heal)
    {
        base.TakeHealing(heal);

        UpdateHealthText();
    }

    protected override void Start()
    {
        base.Start();

        current = this;
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);

        damageFX = DOTween.Sequence();
        damageFX.Append(DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, 1.0f, 0.1f));
        damageFX.Insert(0.0f, DOTween.To(()=> vignette.color.value, x=> vignette.color.value = x, Color.red, 0.1f));
        damageFX.Insert(0.0f, DOTween.To(()=> chromaticAberration.intensity.value, x=> chromaticAberration.intensity.value = x, 1.0f, 0.1f));
        damageFX.Append(DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, 0.5f, 0.9f));
        damageFX.Insert(0.1f, DOTween.To(()=> vignette.color.value, x=> vignette.color.value = x, Color.black, 0.9f));
        damageFX.Insert(0.1f, DOTween.To(()=> chromaticAberration.intensity.value, x=> chromaticAberration.intensity.value = x, 0.0f, 0.9f));
        damageFX.Pause();

        UpdateHealthText();
    }

    protected override void Death()
    {
        Debug.Log("You dieded!");
    }

    protected void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth;
    }
}