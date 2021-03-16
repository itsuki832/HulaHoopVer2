using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ctrl : MonoBehaviour
{
    public enum Setting//ボタン操作の設定
    {
        AUTO,
        W_S_BUTTON
    }
    private Rigidbody rb;//  Rigidbodyを使うための変数
    private bool Grounded;//  地面に着地しているか判定する変数
    bool jump = false;//ジャンプするかどうか
    bool forward = false;//前進するかどうか
    [SerializeField] private float Jumppower;//  ジャンプ力
    [SerializeField] private float Forwardpower;//前進する力
    [SerializeField] private float Descendingpower;//下降していく力
    [SerializeField] private Vector3 velocity;              // 移動方向
    public Setting setting;

    public GameObject ring;
    public bool ani_flag;

    public GameObject setsumei;
    bool setsumeiflag;

    Animator ani;

    Vector2 stick;

    public AudioClip sound2;
    AudioSource audioSource;


    OVRInput.Controller LeftCon;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();//  rbにRigidbodyの値を代入する
        ani = ring.GetComponent<Animator>();

        LeftCon = OVRInput.Controller.LTouch;

        Jumppower = 2f;
        Forwardpower = 3f;
        Descendingpower = 5f;

        ani_flag = true;

        setsumeiflag = false;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 accLeft = OVRInput.GetLocalControllerAcceleration(LeftCon);

        stop();

        /*
        if (setting == Setting.AUTO)//自動で前に前進する設定 //現状つかわないモード
        {
            if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))//スペースキーを押したらFixedUpdateでジャンプする
            {
                jump = true;
                Debug.Log("Jump");
                Grounded = false;//  Groundedをfalseにする

               
            }


            if (Grounded == false)//  もし、Groundedがfalseなら、FixedUpdateで前進する
            {
                forward = true;
                Debug.Log("前進");
            }
        }
        else if (setting == Setting.W_S_BUTTON)//WとSボタンで動く
        {
        */      //if(Input.GetKeyDown(KeyCode.Space))
                //if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                if (accLeft.y > 6f || (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)))
                {
                    jump = true;
                    Debug.Log("Jump");
                    Grounded = false;//  Groundedをfalseにする

                  if (ani_flag == true)
                   {
                    ani_flag = false;
                    StartCoroutine("ringAni");
                    
                   }
             
                }


                W_S_Move();//移動の処理
        Debug.Log(accLeft.y);
        //}

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            setsumeiflag = !setsumeiflag; 
            setsumei.SetActive(setsumeiflag);
        }

        
    }

    private void FixedUpdate()//AddForceの処理をここで行う
    {
        if (jump == true)//ジャンプする
        {
            rb.AddForce(Vector3.up * Jumppower, ForceMode.VelocityChange);//  上にJumpPower分力をかける
          
            jump = false;
        }

        if (forward == true)//前進する
        {
            this.rb.velocity = new Vector3(0, -Descendingpower, Forwardpower);
            forward = false;
        }

    }

    private void W_S_Move()//wとsボタン（右コントローラー）で前後に動く)
    {
        stick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        velocity = Vector3.zero;

    
        if(Input.GetKey(KeyCode.W) || stick.y > 0)
        {
            //velocity.z += 1;
            velocity.z += 0.5f;
        }

       
        if (Input.GetKey(KeyCode.S) || (stick.y < 0))
        {
            //velocity.z -= 1;
            velocity.z -= 0.5f;
        }
        

        // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
        velocity = velocity.normalized * Forwardpower * Time.deltaTime;

        // いずれかの方向に移動している場合
        if (velocity.magnitude > 0)
        {
            // プレイヤーの位置(transform.position)の更新
            // 移動方向ベクトル(velocity)を足し込みます
            this.transform.position += velocity;
        }

    }

    public void stop()//高さ上限
    {
        if(this.gameObject.transform.position.y >= 100f)
        {
            this.transform.position = new Vector3(this.transform.position.x, 90, this.transform.position.z);//上限100にする
        }
    }

    IEnumerator ringAni()
    {
        //flag　true
        ani.SetBool("ring_flag", true);
        audioSource.PlayOneShot(sound2);
        //1秒停止
        yield return new WaitForSeconds(0.5f);

        //flag false
        ani.SetBool("ring_flag", false);
        ani_flag = true;
    }

    

    void OnCollisionEnter(Collision other)//  地面に触れた時の処理
    {
        if (other.gameObject.tag == "ground")//  もしGroundというタグがついたオブジェクトに触れたら、
        {
            Grounded = true;//  Groundedをtrueにする
        }

       
    }
}
