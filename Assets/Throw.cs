using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject ballPrefab;
    private Manager manager;
    public float shotSpeed;

    private bool select_pos;


    // Start is called before the first frame update
    void Start()
    {
        select_pos = true;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.CanThrow()) {
            // クリックで発射
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject ball = (GameObject)Instantiate(ballPrefab, transform.position, Quaternion.identity);
                Rigidbody ballRb = ball.GetComponent<Rigidbody>();
                ballRb.AddForce(transform.right * shotSpeed/*, ForceMode.Impulse*/);
                manager.ThrowStart(ball);
                Debug.Log("Throw!");
            }
            // Rでリスタート?
            else if (Input.GetKeyDown(KeyCode.R))
            {
                // TODO:後で書く
            }
            // 右クリで位置の方向設定入れ替え
            else if (Input.GetKeyDown(KeyCode.Mouse1)) {
                select_pos = !select_pos;
            }
            // 角度とか位置を弄る部分
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (select_pos && this.transform.position.z <= 8)
                    this.transform.Translate(0.0f,0.0f,1.0f);
                else if (!select_pos && this.transform.localEulerAngles.y > -20)
                    this.transform.Rotate(new Vector3(0.0f, -1.5f, 0.0f));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (select_pos && this.transform.position.z >= -8)
                    this.transform.Translate(0.0f,0.0f,-1.0f);
                else if (!select_pos && this.transform.localEulerAngles.y < 20)
                    this.transform.Rotate(new Vector3(0.0f, 1.5f, 0.0f));
            }
        }
        else
        {

        }
    }
}
