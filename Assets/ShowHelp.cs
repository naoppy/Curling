using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHelp : MonoBehaviour
{
    public TextMeshProUGUI helpText;

    // Start is called before the first frame update
    void Start()
    {
        helpText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            // ヘルプテキストを表示
            helpText.enabled = true;
        }
        else
        {
            // ヘルプテキストを非表示
            helpText.enabled = false;
        }
    }
}
