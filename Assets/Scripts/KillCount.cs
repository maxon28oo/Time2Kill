using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//simple script to show the kill count
public class KillCount : MonoBehaviour
{
    private GlobalGameSettings GlobalGameSettings;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        GlobalGameSettings = GameObject.Find("GlobaGameSettings").GetComponent<GlobalGameSettings>();
        text = GetComponent<TextMeshProUGUI>();
        Debug.Log(text);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GlobalGameSettings.killCount + "/" + GlobalGameSettings.KillTarget;
    }
}
