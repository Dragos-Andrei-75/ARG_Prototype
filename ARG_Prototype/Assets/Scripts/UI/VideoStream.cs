using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class VideoStream : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;

    private void Awake()
    {
        rawImage = gameObject.GetComponent<RawImage>();
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        videoPlayer.clip = videoClip;

        StartCoroutine(PlayVideo());
    }

    private IEnumerator PlayVideo()
    {
        Time.timeScale = 1;

        WaitForSeconds wait = new WaitForSeconds(1);

        videoPlayer.Prepare();

        while (videoPlayer.isPrepared == false)
        {
            yield return wait;
            break;
        }

        rawImage.texture = videoPlayer.texture;

        videoPlayer.Play();

        yield break;
    }
}
