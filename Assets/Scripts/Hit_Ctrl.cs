using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hit_Ctrl : MonoBehaviour
{
    public TextMesh text;
    public TextMesh lasttext;
    public GameObject Scoreboard;

    [SerializeField] int point;
    [SerializeField] int savepoint;
    public Vector3 startpoint;


    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        point = 0;
        savepoint = 0;
        startpoint = new Vector3(0f, 13f, 0f);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       

        if(collision.gameObject.tag == "outzone")
        {
            //スタート地点に戻る
            this.gameObject.transform.position = startpoint;
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            point = savepoint;
            text.text = point.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ring")
        {
            Debug.Log("HIT");
            point += 50;
            text.text = point.ToString();
            audioSource.PlayOneShot(sound1);

        }
        
        if(other.gameObject.tag == "ring10")
        {
            point += 10;
            text.text = point.ToString();
            audioSource.PlayOneShot(sound1);
        }
        
        if (other.gameObject.tag == "ring30")
        {
            point += 30;
            text.text = point.ToString();
            audioSource.PlayOneShot(sound1);
        }



        if (other.gameObject.tag == "checkpoint")
        {
            //チェックポイント
            startpoint = new Vector3(0f,20f, 27f);
            savepoint = point;
            audioSource.PlayOneShot(sound2);
        }


        if (other.gameObject.tag == "checkpoint2")
        {
            //チェックポイント
            startpoint = new Vector3(0f, 45f, 76f);
            savepoint = point;
            audioSource.PlayOneShot(sound2);
        }


        if (other.gameObject.tag == "goal")
        {
            //リザルト表示
            lasttext.text = point.ToString();
            Scoreboard.SetActive(true);

            audioSource.PlayOneShot(sound3);
        }

    }
}