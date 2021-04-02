using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Tutorial : MonoBehaviour
{
    public PlayableDirector tutorial2;

    public void PlayTutorial()
    {
        //Time.timeScale = 0.5f;
        tutorial2.Play();
        StartCoroutine("ResumeTime");
    }

    IEnumerator ResumeTime()
    {
        yield return new WaitForSeconds(2.5f);
        Time.timeScale = 1;
    }
}
