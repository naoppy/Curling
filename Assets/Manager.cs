using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private bool is_throwing;
    private int throwed_cnt;
    private const int THROW_TIME = 3;
    private const float EPS = 1E-20f;
    private const float EPS2 = 1E-40f;
    private float last_throw_time;
    List<GameObject> stones;


    // Start is called before the first frame update
    void Start()
    {
        is_throwing = false;
        throwed_cnt = 0;
        stones = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_throwing) {
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
                if (allstop)
                    ThrowStop();
            }
        }
        else if (throwed_cnt == THROW_TIME)
        {
            Invoke(nameof(CalcScore), 2.0f);
            
        }
    }

    // この辺適当に実装してる
    private void CalcScore()
    {
        Vector3 center = new Vector3(100, 0, 0);
        int score = 0;
        for (int i = 0; i < THROW_TIME; i++)
        {
            float dist = Vector3.Distance(stones[i].transform.position, center);
            if (dist < 5)
            {
                score += 100;
            }
            else {
                score += 10;
            }
        }
        Debug.Log(score);
    }

    public void ThrowStart(GameObject stone)
    {
        if ((!is_throwing) && throwed_cnt < THROW_TIME)
        {
            is_throwing = true;
            stones.Add(stone);
            last_throw_time = Time.time;
            Debug.Log("Start Throw in Manager");
        }
    }

    public void ThrowStop()
    {
        if (is_throwing)
        {
            throwed_cnt++;
            is_throwing = false;
            Debug.Log("Stop Throw in Manager");
        }
    }

    public bool CanThrow()
    {
        return (!is_throwing) && throwed_cnt < THROW_TIME;
    }
}
