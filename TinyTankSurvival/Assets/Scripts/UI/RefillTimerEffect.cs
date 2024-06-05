using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RefillTimerEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField]
    private Image fillIcon;
    [SerializeField]
    [Range(0f, 2.5f)]
    private float textFadeOutTime = 0.4f;

    private GameUpgrades gameUpgrades;
    private Coroutine textFadeAnimation;

    // Start is called before the first frame update
    void Start()
    {
        gameUpgrades = GameUpgrades.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // No event available? :(
        UpdateText(fillIcon.fillAmount);
    }

    /// <summary>
    /// Updates the text field's text and its effects
    /// </summary>
    /// <param name="progress">Progress towards completion 0-1</param>
    private void UpdateText(float progress)
    {
        if (progress >= 1)
        {
            if (textFadeAnimation != null)
                return;
            else
            {
                textFadeAnimation = StartCoroutine(FadeOutText());
            }
        } else if (progress <= 0)
        {
            if (textFadeAnimation == null)
                return;
            else
            {
                StopCoroutine(textFadeAnimation);
                textFadeAnimation = null;
                textField.alpha = 1;
            }
        }
        
        //textField.text = string.Format("{0:0.00}", Mathf.Lerp(gameUpgrades.ReloadTime, 0, progress) );
        textField.color = Color.HSVToRGB(progress * 120/360, 1, 1);
        textField.alpha = 1;
    }

    // Fading animation tutorial
    // https://www.youtube.com/watch?v=hweptxjNgV0
    /// <summary>
    /// Fades out text
    /// </summary>
    private IEnumerator FadeOutText() {
        float elapsedTime = 0.0f;
        while (elapsedTime < textFadeOutTime) {
            elapsedTime += Time.deltaTime;
            textField.alpha = Mathf.Lerp(1, 0, elapsedTime / textFadeOutTime);
            yield return null;
        }

        textField.alpha = 0;
    }
}
