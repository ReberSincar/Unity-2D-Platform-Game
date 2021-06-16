using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Text timeText;
    public float time;
    bool isGameActive;
    void Start()
    {
        isGameActive = true;
        timeText.text = timeText.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            time -= Time.deltaTime;
            timeText.text = ((int)time).ToString();
        }
        if(time < 0)
        {
            time = 60;
            isGameActive = false;
            GetComponent<PlayerController>().Die();
        }
    }
}
