using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownUI : MonoBehaviour
{
   
    //[SerializeField] private Image lightningStrikeCooldownImage;
    //[SerializeField] private LightingStrike lightningStrikeAbility;

    //[SerializeField] private Image meteorShowerCooldownImage;
    //[SerializeField] private MeteorShower meteorShowerAbility;

    //[SerializeField] private Image KnifeRainCooldownImage;
    //[SerializeField] private KnifeRain KnifeAbility;

    // Start is called before the first frame update
    void Start()
    {
        //if (lightningStrikeCooldownImage == null || meteorShowerCooldownImage == null || KnifeRainCooldownImage == null)
        //{
        //    Debug.LogError("One or more ability or UI image is missing.");
        //    enabled = false;
        //    return;
        //}

        //lightningStrikeCooldownImage.fillAmount = 1.0f;
        //meteorShowerCooldownImage.fillAmount = 1.0f;
        //KnifeRainCooldownImage.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateLightningCooldown(lightningStrikeCooldownImage, lightningStrikeAbility);
        //UpdateMeteorCooldown(meteorShowerCooldownImage, meteorShowerAbility);
        //UpdateKnifeCooldown(KnifeRainCooldownImage, KnifeAbility);
    }

    //private void UpdateLightningCooldown(Image cooldownImage, LightingStrike LightDown)
    //{
    //    if (LightDown.isLightningActive == false)
    //    {
    //        StartCoroutine(UpdateCooldownFill(cooldownImage, LightDown.CoolDownDuration));
    //    }
    //}
    //private void UpdateMeteorCooldown(Image cooldownImage, MeteorShower meteor)
    //{
    //    if (meteor.isMeteorActive == false)
    //    {
    //        StartCoroutine(UpdateCooldownFill(cooldownImage, meteor.CoolDownDuration));
    //    }
    //}
    //private void UpdateKnifeCooldown(Image cooldownImage, KnifeRain rain)
    //{
    //    if (rain.isKnifeActive == false)
    //    {
    //        StartCoroutine(UpdateCooldownFill(cooldownImage, rain.KnifeTimerCooldown));
    //    }
    //}

    //private IEnumerator UpdateCooldownFill(Image cooldownImage, float fillDuration)
    //{
    //    cooldownImage.fillAmount = 1.0f;

    //    float timer = 0.0f;

    //    while (timer < fillDuration)
    //    {
    //        timer += Time.deltaTime;

    //        // Update the fill amount based on the remaining cooldown time
    //        cooldownImage.fillAmount = 1.0f - (timer / fillDuration);

    //        yield return null;
    //    }
       
    //    // Ensure the fill amount is set to 0 when cooldown is complete
    //    cooldownImage.fillAmount = 0.0f;
    //}
}
