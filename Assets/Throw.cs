using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Throw : MonoBehaviour
{
    public GameObject ballPrefab;
    private Manager manager;
    private const float COUNT_TIME = 5.0f;
    private float countTime;

    private bool select_pos;

    public GameObject arrowOG;
    public GameObject arrowPL;
    public TextMeshProUGUI countDown;
    public GameObject Meter;
    private Slider slider;

    private const int MAX_ANGLE = 10;
    private const int MAX_Z = 7;
    private const float ANGLE_DELTA = 5f;
    private const float Z_DELTA = 5f;


    // Start is called before the first frame update
    void Start()
    {
        select_pos = true;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        arrowOG.SetActive(true);
        arrowPL.SetActive(false);
        countDown.enabled = false;
        countTime = COUNT_TIME;
        slider = Meter.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.CanThrow()&& Input.GetKeyDown(KeyCode.Mouse0))
        {
            countTime = COUNT_TIME;
            manager.SpeakStart();
        }
        else if (manager.GetState() == State.Prepare)
        {
            // 右クリで位置の方向設定入れ替え
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                select_pos = !select_pos;
                if (select_pos)
                {
                    arrowOG.SetActive(true);
                    arrowPL.SetActive(false);
                }
                else
                {
                    arrowOG.SetActive(false);
                    arrowPL.SetActive(true);
                }
            }
            // 角度とか位置を弄る部分
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (select_pos && this.transform.position.z <= MAX_Z)
                    this.transform.Translate(0.0f, 0.0f, Z_DELTA * Time.deltaTime);
                else if (!select_pos && Range() != 2)
                {
                    this.transform.Rotate(new Vector3(0.0f, -ANGLE_DELTA * Time.deltaTime, 0.0f));
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (select_pos && this.transform.position.z >= -MAX_Z)
                    this.transform.Translate(0.0f, 0.0f, -Z_DELTA * Time.deltaTime);
                else if (!select_pos && Range() != 1)
                {
                    this.transform.Rotate(new Vector3(0.0f, ANGLE_DELTA * Time.deltaTime, 0.0f));
                }
            }
        }
        else if (manager.GetState() == State.Speaking)
        {
            countDown.enabled = true;
            countTime -= Time.deltaTime;
            if (countTime <= 0)
            {
                GameObject ball = (GameObject)Instantiate(ballPrefab, transform.position, Quaternion.identity);
                Rigidbody ballRb = ball.GetComponent<Rigidbody>();
                int base_speed = 1500;
                float max_speed = slider.maxValue;
                float speed = base_speed + (max_speed - base_speed) * slider.value / max_speed;
                ballRb.AddForce(transform.right * speed/*, ForceMode.Impulse*/);
                manager.ThrowStart(ball);
                countDown.enabled = false;
            }
            else {
                countDown.text = $"Time: {countTime:f2}\nHARAKARA  KOEDASE!";
                
            }
        }
    }

    private int Range()
    {
        float theta = this.transform.localEulerAngles.y;
        if (360 - MAX_ANGLE <= theta && theta <= 360)
        {
            return 0;
        }
        else if (0 <= theta && theta <= MAX_ANGLE)
        {
            return 0;
        }
        else if (MAX_ANGLE <= theta && theta <= 180)
        {
            return 1;
        }
        else if (180 <= theta && theta <= 360 - MAX_ANGLE)
        {
            return 2;
        }
        else {
            // UNREACHABLE CODE
            return 3;
        }
    }
}
