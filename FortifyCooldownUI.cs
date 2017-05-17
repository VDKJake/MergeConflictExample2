
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortifyCooldownUI : MonoBehaviour {

    private bool isModActive;
    private bool isCooldownActive;
    private bool isSet;
    private float activeTime;
    private float modMaxTime = GLOBAL_VALUES.FORTIFY_DURATION;
    private float modCooldownMaxTime = GLOBAL_VALUES.FORTIFY_COOLDOWN;
    private Timer modTimer = new Timer();
    private float scaleMult;
    private float baseScaleMult = 0.0025f;
    private Vector3 baseScale;

    public Timer cooldownTimer;
    public Image cooldownImage;
    public Image cooldownBackgroundImage;
    public Image centerImage;

    // Sets up the bar so that it is full
    // and sets them as inactive.
	void Start ()
    {
        cooldownImage.fillAmount = 1.0f;
        cooldownImage.gameObject.SetActive(false);
        cooldownBackgroundImage.gameObject.SetActive(false);
        centerImage.gameObject.SetActive(false);

        scaleMult = baseScaleMult;
        baseScale = gameObject.transform.localScale;
    }
	
	// Updates the timer every frame, as well as
    // checking each timer.
	void Update ()
    {
        modTimer.Update();
        
        // If both the duration and cooldown are complete
        // then hide the slider. Else, if the image isn't active
        // (and one of the timers is running) then show the slider.\
        if(isSet)
        { 
            if (modTimer.isComplete() && cooldownTimer.isComplete())
            {
                //cooldownImage.gameObject.SetActive(false);
                //cooldownBackgroundImage.gameObject.SetActive(false);
                //centerImage.gameObject.SetActive(false);

                if (cooldownImage.GetComponent<CanvasRenderer>().GetAlpha() == 1.0f)
                {

                    Fade();
                }
                if (cooldownImage.GetComponent<CanvasRenderer>().GetAlpha() > 0.0f)
                {
                    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + scaleMult, gameObject.transform.localScale.y + scaleMult, gameObject.transform.localScale.z + scaleMult);
                    centerImage.transform.localScale = new Vector3(centerImage.transform.localScale.x + scaleMult, centerImage.transform.localScale.y + scaleMult, centerImage.transform.localScale.z + scaleMult);
                    scaleMult += baseScaleMult;
                }
            }
            else if (!cooldownImage.IsActive())
            {
                cooldownImage.gameObject.SetActive(true);
                cooldownBackgroundImage.gameObject.SetActive(true);
                centerImage.gameObject.SetActive(true);
            }
        }

        print("adding some code here");
        // If the duration is over then tell the script it is over.
        if (modTimer.isComplete())
        {
            isModActive = false;
        }

        // If the mod is active and the timer isn't complete then
        // create a 'percentage' out of the leftover time on the timer
        // and the max duration of the mod, then reduce the fill amount
        // based on this.
        // Else, if the cooldown is active and the mod duration is over
        // then do the same for the cooldown, except increase it instead.
        if (isModActive && !modTimer.isComplete())
        {
            gameObject.transform.localScale = baseScale;
            scaleMult = baseScaleMult;
            centerImage.transform.localScale = baseScale;

            cooldownImage.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            cooldownBackgroundImage.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            centerImage.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            float percent = modTimer.Get_Time() / modMaxTime;
            cooldownImage.fillAmount = Mathf.Lerp(0, 1, percent);
        } else if (isCooldownActive && modTimer.isComplete())
        {
            gameObject.transform.localScale = baseScale;
            centerImage.transform.localScale = baseScale;
            scaleMult = baseScaleMult;

            cooldownImage.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            cooldownBackgroundImage.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            centerImage.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            float percent = cooldownTimer.Get_Time() / modCooldownMaxTime;
            cooldownImage.fillAmount = Mathf.Lerp(1, 0, percent);
        }

        // If the cooldown timer has been set and the cooldown timer is
        // complete then tell the script that it is over.
        if (isSet == true)
        {
            if (cooldownTimer.isComplete())
            {
                isCooldownActive = false;
            }
        }
	}

    public void Fade()
    {
        cooldownImage.CrossFadeAlpha(0.0f, 0.5f, false);
        cooldownBackgroundImage.CrossFadeAlpha(0.0f, 0.5f, false);
        centerImage.CrossFadeAlpha(0.0f, 0.5f, false);
    }

    // Starts the duration timer and sets the fill images as active.
    public void StartDurationTimer()
    {
        
        modTimer.Add(GLOBAL_VALUES.FORTIFY_DURATION, true);
        isModActive = true;
        cooldownImage.gameObject.SetActive(true);
        cooldownBackgroundImage.gameObject.SetActive(true);
        centerImage.gameObject.SetActive(true);
    }

    // Takes in the cooldown timer and lets the script know that it
    // is active and has been set.
    public void SetTimer(Timer cTimer)
    {
        cooldownTimer = cTimer;
        isCooldownActive = true;
        isSet = true;
    }
}
