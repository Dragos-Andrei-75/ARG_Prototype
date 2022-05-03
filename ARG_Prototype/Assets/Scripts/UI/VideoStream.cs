using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class VideoStream : MonoBehaviour
{
    private RawImage rawImage;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        rawImage = gameObject.GetComponent<RawImage>();
        videoPlayer = gameObject.GetComponent<VideoPlayer>();

        StartCoroutine(PlayVideo());
    }

    private IEnumerator PlayVideo()
    {
        WaitForSeconds wait = new WaitForSeconds(1);

        videoPlayer.Prepare();

        while (videoPlayer.isPrepared == false)
        {
            yield return wait;
            break;
        }

        rawImage.texture = videoPlayer.texture;

        videoPlayer.Play();
    }
}
