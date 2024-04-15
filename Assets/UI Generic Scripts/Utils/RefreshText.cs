using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;
using DG.Tweening;
using TMPro;

public class RefreshText : MonoBehaviour
{
    [SerializeField]
    private LoadingScreen loadingScreen;

    [SerializeField]
    private BaseWindow[] genericWindows;

    [SerializeField]
    private Canvas[] canvases;

    [SerializeField]
    private CanvasGroup[] canvasGroups;

    private TMP_Text[] texts;

    [SerializeField]
    private UISequenceManager mainSequenceManager;

    private void SetBaseWindows()
    {
        genericWindows = FindObjectsOfType<BaseWindow>();
        texts = FindObjectsOfType<TMP_Text>();
        canvases = FindObjectsOfType<Canvas>();
        canvasGroups = FindObjectsOfType<CanvasGroup>();
    }

    public void RefreshBaseWindows()
    {
        StopAllCoroutines();
        StartCoroutine(RefreshBaseWindowsCo());
    }

    /// <summary>
    /// Goes through all BaseWindows and over time,
    /// Opens them all, toggle their SetActive, and resets their Open/Close state
    /// </summary>
    /// <returns></returns>
    public IEnumerator RefreshBaseWindowsCo(float restLength = 0.5f)
    {
        WaitForSeconds restInterval = new WaitForSeconds(restLength);

        if (loadingScreen != null)
            loadingScreen.gameObject.SetActive(true);

        SetBaseWindows();

        // Because some ongoing Unsequenced / untracked animations can complicate this
        // the path of least resistance is to wait a duration presumptively longer than any animation
        yield return new WaitForSeconds(5f);

        if (mainSequenceManager != null)
            mainSequenceManager.CompleteCurrentSequence();

        yield return restInterval;

        bool[] activeStates = new bool[genericWindows.Length];
        bool[] openStates = new bool[genericWindows.Length];
        Tween[] tweens = new Tween[genericWindows.Length];

        bool[] textActiveStates = new bool[texts.Length];

        //First Open all windows
        for (int i = 0; i < genericWindows.Length; i++)
        {
            BaseWindow window = genericWindows[i];
            if (window != null && window.gameObject != null)
            {
                openStates[i]   = window.isOpen;
                tweens[i]       = window.Open(SequenceType.UnSequenced);
            }
        }

        //This rest is necessary as trying to complete immediately results in it not working
        yield return restInterval;

        //Then complete window Opens
        foreach (Tween tween in tweens)
        {
            tween.Complete();
        }

        yield return null;

        //Open again...
        for (int i = 0; i < genericWindows.Length; i++)
        {
            BaseWindow window = genericWindows[i];
            if (window != null && window.gameObject != null && !window.isOpen)
            {
                window.Open(SequenceType.CompleteImmediately);
            }
        }

        yield return restInterval;

/*        for (int c = 0; c < canvases.Length; c++)
        {
            Canvas canvas = canvases[c];
            if (canvas != null)
                canvas.enabled = true;
        }

        for (int cg = 0; cg < canvasGroups.Length; cg++)
        {
            CanvasGroup canvasGroup = canvasGroups[cg];
            if (canvasGroup != null)
                canvasGroup.alpha = 1;
        }

        

        yield return restInterval;*/

        //Then toggle off the active states of all texts
        for (int t=0; t < texts.Length; t++)
        {
            TMP_Text text = texts[t];
            if (text != null && text.gameObject != null)
            {
                textActiveStates[t] = text.gameObject.activeSelf;
                text.gameObject.SetActive(false);
            }
        }



        yield return restInterval;

        //Then toggle on the active states of all texts where appropriate
        for (int t = 0; t < texts.Length; t++)
        {
            TMP_Text text = texts[t];
            if (text != null && text.gameObject != null)
            {
                text.gameObject.SetActive(textActiveStates[t]);
            }
        }

        yield return restInterval;

/*        //Then de-activate all window gameobjects
        for (int i = 0; i < genericWindows.Length; i++)
        {
            BaseWindow window = genericWindows[i];
            if (window != null && window.gameObject != null)
            {
                activeStates[i] = window.gameObject.activeSelf;
                window.gameObject.SetActive(false);
            }

        }*/

        yield return restInterval;

        //Then re-activate and close windows
        List<Tween> closeTweens = new List<Tween>();
        for (int i = 0; i < genericWindows.Length; i++)
        {
            BaseWindow window = genericWindows[i];
            if (window != null && window.gameObject != null)
            {
                //window.gameObject.SetActive(activeStates[i]);
                if (!openStates[i])
                {
                    closeTweens.Add(window.Close(SequenceType.UnSequenced));
                }
            }

        }

        yield return restInterval;

        //Complete close tweens
        foreach (Tween tween in closeTweens)
        {
            tween.Complete();
        }

        yield return restInterval;

        //Finally, double-check my work
        for (int i = 0; i < genericWindows.Length; i++)
        {
            BaseWindow window = genericWindows[i];
            if (window != null && window.gameObject != null)
            {
                //window.gameObject.SetActive(activeStates[i]);
                if (!openStates[i]) window.Close(SequenceType.CompleteImmediately);
                else window.Open(SequenceType.CompleteImmediately);
            }

        }

        if (loadingScreen != null)
            loadingScreen.FadeOut();
    }
}
