using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    public float height = 7.0f;

    private const float timeBetweenSwitches = 2f;
    private float timeToSwitch;

    [Header("Animation")]
    public List<Sprite> frames = new List<Sprite>();
    private int spriteIndex = 0;
    private const float timeBetweenFrames = 0.0625f;
    private float timeTillNextFrame;

    [Header("References")]
    public GameObject actualLaser;
    public SpriteRenderer laserSR;
    public Transform laserIndicator;

    private void OnValidate() {
        actualLaser.transform.localScale = new Vector3(1, height, 1);
        laserIndicator.localScale = new Vector3(1, height, 1);
        laserSR.size = new Vector2(1, height);
    }

    private void Update() { 

        bool allowedToSwitch = !actualLaser.activeSelf;
        if(!allowedToSwitch)
        {
            if(timeTillNextFrame > 0.0f)
            {
                timeTillNextFrame -= Time.deltaTime;
            }
            else
            {
                allowedToSwitch = true;
                timeTillNextFrame = timeBetweenFrames;

                spriteIndex++;
                if(spriteIndex == frames.Count)
                {
                    spriteIndex = 0;
                }

                laserSR.sprite = frames[spriteIndex];
            }
        }

        if(timeToSwitch > 0.0f)
        {
            timeToSwitch -= Time.deltaTime;
        }
        else if(allowedToSwitch)
        {
            timeToSwitch = timeBetweenSwitches;
            SetLaserActive(!actualLaser.activeSelf);
        }
    }

    public void SetLaserActive(bool _bool)
    {
        actualLaser.SetActive(_bool);
        laserSR.gameObject.SetActive(_bool);
    }
}
