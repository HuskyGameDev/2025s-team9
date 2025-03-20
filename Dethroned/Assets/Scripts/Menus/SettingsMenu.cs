using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] GameObject settingsMenu;
    [SerializeField] TMP_Text volume;
    [SerializeField] Slider volControl;
    [SerializeField] AudioMixer audio;
    private float volMast;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu.SetActive(false);
        audio.GetFloat("VolMaster", out volMast);
        volControl.SetValueWithoutNotify(Mathf.Pow(10, (20 * volMast)));
        volume.text = (Mathf.Pow(10, (20 * volMast)) * 100).ToString("0.0") + "%";
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void UpdateVolume(float vol) 
    {
        volume.text = (vol * 100).ToString("0.0") + "%";
        audio.SetFloat("VolMaster", (Mathf.Log10(vol) * 20));
    }
}
