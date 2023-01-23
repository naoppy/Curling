using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum State {
    Prepare,
    Speaking,
    Throwing,
    Result,
}

public class Manager : MonoBehaviour
{
    private State state;
    private int throwed_cnt;
    private const int THROW_TIME = 3;
    private const float EPS = 1E-20f;
    private const float EPS2 = 1E-40f;
    private float last_throw_time;
    List<GameObject> stones;
    private bool camera_locked = false;

    public TextMeshProUGUI resultText;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Prepare;

        resultText.enabled = false;
        
        throwed_cnt = 0;
        stones = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Throwing) {
            var stone_pos = stones[throwed_cnt].transform.position;
            // カメラの位置を動かす
            if (!camera_locked && stone_pos.x <= 65) {
                Camera mc = Camera.main;
                var pos = stones[throwed_cnt].transform.position
                + new Vector3(-1.0f, 3.0f, 0.0f);
                mc.transform.position = Vector3.Lerp(mc.transform.position, pos, Time.deltaTime * 2.0f);
            }
            else
            {
                camera_locked = true;
                Invoke(nameof(ResetCamera), 1);
            }

            // 終了条件：少なくとも2秒は経過＆全ての石が止まればよい
            if (Time.time - last_throw_time > 2) {
                bool allstop = true;
                for (int i = throwed_cnt; i >= 0 && allstop; i--)
                {
                    Rigidbody rb = stones[i].GetComponent<Rigidbody>();
                    float speed = rb.velocity.sqrMagnitude;
                    if (speed < EPS2)
                    {
                        rb.velocity = Vector3.zero;
                    }
                    else
                    {
                        allstop = false;
                    }
                }
                if (allstop) {
                    ThrowStop();
                    ResetCamera();
                }
            }
        }
        else if (throwed_cnt == THROW_TIME && state != State.Result)
        {
            Invoke(nameof(CalcScore), 2.0f);
            state = State.Result;
        }
    }

    void ResetCamera()
    {
        Camera mc = Camera.main;
        mc.transform.localPosition = new Vector3(4.36f, 13.0f, 0.0f);
    }

    private void CalcScore()
    {
        Vector3 center = new Vector3(73.7f, 0, 0);
        int score = 0;
        for (int i = 0; i < THROW_TIME; i++)
        {
            float dist = Vector3.Distance(stones[i].transform.position, center);
            if (dist <= 1)//74.7
            {
                score += 100;
            }
            else if (dist <= 2.6)//76.3
            {
                score += 50;
            }
            else if (dist <= 4.8)//78.5
            {
                score += 30;
            }
            else if (dist <= 7.3)//81
            {
                score += 10;
            }
        }
        resultText.text = $"Score: {score}\nThank you for playing!";
        resultText.enabled = true;
        Debug.Log(score);
    }

    public void SpeakStart()
    {
        if (state == State.Prepare)
        {
            state = State.Speaking;
        }
    }

    public void ThrowStart(GameObject stone)
    {
        if (state == State.Speaking && throwed_cnt < THROW_TIME)
        {
            state = State.Throwing;
            stones.Add(stone);
            last_throw_time = Time.time;
        }
    }

    public void ThrowStop()
    {
        if (state == State.Throwing)
        {
            throwed_cnt++;
            state = State.Prepare;
            camera_locked = false;
        }
    }

    public bool CanThrow()
    {
        return state == State.Prepare && throwed_cnt < THROW_TIME;
    }

    public State GetState()
    {
        return this.state;
    }
}
