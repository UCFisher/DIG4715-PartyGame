﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeeControl : MonoBehaviour
{
    public float speed = 20.0f;
    public Text timer;
    public Text honey;
    public GameObject start;
    public GameObject endWin;
    public GameObject endLose;
    public GameObject bckgMusic;
    public AudioClip collectSound;
    private float speedApplied;
    private float timeTotal;
    private int score;
    private int scoreTotal;
    private int timeSec;
    private int timePrev;
    private int timePlay;
    private Rigidbody2D rb2D;
    private AudioSource audioSource;
    private GameObject[] collectibles;
    private float vertical;
    private float horizontal;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        speedApplied = speed;
        timeTotal = 0;
        timePlay = 10;
        timePrev = 0;
        score = 0;
        scoreTotal = GameObject.FindGameObjectsWithTag("Collectible").Length;
        timer.text = timePlay.ToString();
        honey.text = score.ToString() + "/" + scoreTotal.ToString();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timeTotal += Time.deltaTime;
        timeSec = Mathf.FloorToInt(timeTotal);
        if (timeSec < 2 || timeSec > 12)
        {
            bckgMusic.SetActive(false);
            speedApplied = 0;
        }
        else
        {
            speedApplied = speed;

            start.SetActive(false);
            bckgMusic.SetActive(true);

            if (timeSec > timePrev)
            {
                timePrev = timeSec;
                timer.text = timePlay.ToString();
                timePlay--;
            }
        }

        horizontal = Input.GetAxis("Horizontal") * speedApplied;
        vertical = Input.GetAxis("Vertical") * speedApplied;


        if (timePlay <= 0 && speedApplied == 0)
        {
            collectibles = GameObject.FindGameObjectsWithTag("Collectible");
            foreach (GameObject honey in collectibles )
            {
                honey.SetActive(false);
            }
            if (score != scoreTotal)
            {
                endLose.SetActive(true);
            }
            else
            {
                endWin.SetActive(true);
            }
        }

        if (timeSec >= 14)
        {
            endWin.SetActive(false);
            endLose.SetActive(false);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKey("return"))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }

    void FixedUpdate()
    {
        rb2D.AddForce(new Vector2(horizontal, vertical));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Collectible") 
        {
            audioSource.PlayOneShot(collectSound);
            score++;
            honey.text = score.ToString() + "/" + scoreTotal.ToString();
        }
    }
}