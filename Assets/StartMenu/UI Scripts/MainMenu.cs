using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TMP_Dropdown resDrop;
    Resolution[] resolutions;


    void Start () {
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();

        List<string> options = new List<string>();

        int currectRes = 0;

        for(int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height ) {
                    currectRes = i;
                }
        }

        resDrop.AddOptions(options);
        resDrop.value = currectRes;
        resDrop.RefreshShownValue();
    }


    public void setResolution(int resIndex) {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void PlayGame() {SceneManager.LoadScene("Test_1");}

    public void goToSettings() {SceneManager.LoadScene("Settings");}

    public void goToMainMenu() {SceneManager.LoadScene("MainMenu");}

    public void QuitGame() {Application.Quit();}

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
    }


    public void setFullScreen(bool flag) {
        Screen.fullScreen = flag;
    }
}
