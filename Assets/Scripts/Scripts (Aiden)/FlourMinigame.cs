using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlourMinigame : MonoBehaviour
{
    public Slider flourSlider;
    float startTimerTime = 50;
    float timerTime;
    ParticleSystem flourParticles;
    bool activeParticles = false;
    // Start is called before the first frame update
    void Start()
    {
        flourParticles = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        flourParticles.Stop();
        timerTime = startTimerTime;
        flourSlider.value = 1;
        flourSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        flourParticles.gameObject.transform.rotation = Quaternion.Euler(90, 0, 90);
        if (!gameObject.GetComponent<BoxCollider>().enabled)
        {            
            flourSlider.gameObject.SetActive(true);
            timerTime -= Time.deltaTime;
            if (timerTime < 0)
            {
                flourSlider.value = 0;
                flourParticles.Stop();
                activeParticles = false;
                for (int i = 0; i < gameObject.GetComponent<InGameItemTags>().Tags.Count; i++)
                {
                    if (gameObject.GetComponent<InGameItemTags>().Tags[i].TagName == "Flour")
                    {
                        gameObject.GetComponent<InGameItemTags>().Tags.RemoveAt(i);
                    }
                }                
            }
            else
            {
                if (!activeParticles)
                {
                    flourParticles.Play();
                    activeParticles = true;
                }
                flourSlider.value = timerTime / startTimerTime;
            }
        }
        else
        {
            flourParticles.Stop();
            flourSlider.gameObject.SetActive(false);
            activeParticles = false;
        }
    }
}
