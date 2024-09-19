using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoPlayerUI_PlayPause : VideoPlayerUI_Base
{
    public Button button;

    public GameObject playIcon;
    public GameObject replayIcon;
    public GameObject pauseIcon;

    public bool allowPausing = true;

    [SerializeField]
    private bool doShowUIAtEnd = true;

    protected override void Start()
    {
        base.Start();

        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(() => OnClick());
        }
    }

    private void OnClick()
    {
        if (player == null)
            return;

        if (player.isPlaying)
        {
            player.Pause();
        }
        else
        {
            player.Play();
        }
    }

    private void Update()
    {
        if (player == null) return;
        if (playIcon == null) return;

        if (player.isPlaying)
        {
            button.interactable = allowPausing;

            playIcon.SetActive(false);

            if (pauseIcon != null)
                pauseIcon.SetActive(true);

            if (replayIcon != null)
                replayIcon.SetActive(false);
        }
        else
        {
            if (pauseIcon != null)
                pauseIcon.SetActive(false);

            if (Mathf.Abs(Duration - (float)player.time) < 0.1f)
            {
                if (doShowUIAtEnd)
                {
                    if (replayIcon != null)
                    {
                        replayIcon.SetActive(true);
                        playIcon.SetActive(false);
                        button.interactable = true;
                    }
                    else
                    {
                        playIcon.SetActive(true);
                        button.interactable = true;
                    }
                }
                else
                {
                    playIcon.SetActive(false);
                    button.interactable = false;
                }
            }
            else //paused but not at the end of the video
            {
                playIcon.SetActive(true);
                button.interactable = true;
            }

        }
    }
}
