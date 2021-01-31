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
    private Sequence healingFX;

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

        healingFX.Rewind();
        healingFX.Play();

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

        healingFX = DOTween.Sequence();
        healingFX.Append(DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, 1.0f, 0.1f));
        healingFX.Insert(0.0f, DOTween.To(()=> vignette.color.value, x=> vignette.color.value = x, Color.green, 0.1f));
        healingFX.Append(DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, 0.5f, 0.9f));
        healingFX.Insert(0.1f, DOTween.To(()=> vignette.color.value, x=> vignette.color.value = x, Color.black, 0.9f));
        healingFX.Pause();

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