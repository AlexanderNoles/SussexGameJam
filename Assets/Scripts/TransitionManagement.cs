using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManagement : MonoBehaviour
{
    private static TransitionManagement _instance;

    public RectTransform swipeRect;
    public RectTransform logo;
    public AnimationCurve logoCurve;
    public AnimationCurve logoRotationCurve;

    private float introTransitionT = 2.0f;
    private const float transitionSpeed = 2.0f;

    public static void PlayOutro()
    {
        _instance.introTransitionT = 0.0f;
        _instance.outroTransitionT = 2.0f;
        _instance.swipeRect.gameObject.SetActive(true);

        _instance.swipeRect.offsetMin = Vector2.zero;
        _instance.swipeRect.offsetMax = new Vector2(-Screen.width, 0.0f);
    }

    private float outroTransitionT;

    public static bool OutroFinished()
    {
        return _instance.outroTransitionT <= 0.0f;
    }

    private void Awake() 
    {
        _instance = this;
    }

    private void Update() {
        //Intro transition
        if(introTransitionT > 0.0f)
        {
            introTransitionT -= Time.deltaTime * transitionSpeed;
            UpdateLogo(Mathf.Clamp01(introTransitionT / 2.0f));

            swipeRect.offsetMin = new Vector2(Mathf.Lerp(Screen.width, 0.0f, introTransitionT), 0.0f);

            if(introTransitionT <= 0.0f)
            {
                swipeRect.gameObject.SetActive(false);
            }
        }

        //Outro Transition
        if(!OutroFinished())
        {
            outroTransitionT -= Time.deltaTime * transitionSpeed;
            UpdateLogo(outroTransitionT / 2.0f);

            swipeRect.offsetMax = new Vector2(Mathf.Lerp(0.0f, -Screen.width, outroTransitionT), 0.0f);
        }
    }

    private void UpdateLogo(float t)
    {
        logo.localScale = Vector3.one * (logoCurve.Evaluate(t) * 2.0f);
        logo.localRotation = Quaternion.Euler(0, 0, logoRotationCurve.Evaluate(t) * 360.0f);
    }
}
