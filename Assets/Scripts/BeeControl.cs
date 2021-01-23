using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeeControl : MonoBehaviour
{
    public float speed = 1.0f;
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
        // Debug.Log(Mathf.FloorToInt(timeTotal)); // Remove Later
        if (timeSec < 2 || timeSec > 12)
        {
            bckgMusic.SetActive(false);
            speedApplied = 0;
        }
        else
        {
            start.SetActive(false);
            bckgMusic.SetActive(true);

            speedApplied = speed;
            if (timeSec > timePrev)
            {
                timePrev = timeSec;
                timer.text = timePlay.ToString();
                timePlay--;
            }
        }

        float horizontal = Input.GetAxis("Horizontal") * speedApplied;
        float vertical = Input.GetAxis("Vertical") * speedApplied;


        rb2D.AddForce(new Vector2(horizontal, vertical));

        if (timePlay <= 0 && speedApplied == 0)
        {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collectible") 
        {
            audioSource.PlayOneShot(collectSound);
            score++;
            honey.text = score.ToString() + "/" + scoreTotal.ToString();
        }
    }
}