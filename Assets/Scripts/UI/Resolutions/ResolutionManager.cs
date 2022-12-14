using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public bool AdaptOnStart = false;
    public ResolutionsSO resolutionsSO;
    [Header("UI Components")]
    public TMP_Dropdown dropDownComponent;
    public Toggle toggleComponent;

    ResolutionsSO.ResolutionSetting current;
    bool fullScreen = true;

    private void Start()
    {
        InitDropdownComponents();
            LoadResolution(!AdaptOnStart);
    }
    void InitDropdownComponents()
    {
        if (dropDownComponent == null)
            return;
        foreach (ResolutionsSO.ResolutionSetting res in resolutionsSO.AvailableResolutions)
        {
            dropDownComponent.options.Add(new TMP_Dropdown.OptionData(res.name));
        }
    }
    void LoadResolution(bool UIonly)
    {
        int resValue = 0;
        if (PlayerPrefs.HasKey("ScreenFullscreen") && PlayerPrefs.HasKey("ScreenWidth") && PlayerPrefs.HasKey("ScreenHeight"))
        {
            int width = PlayerPrefs.GetInt("ScreenWidth");
            int height = PlayerPrefs.GetInt("ScreenHeight");

            current = resolutionsSO.AvailableResolutions[0];
            if (resolutionsSO.AvailableResolutions.Length > 1)
                for (int I = 1; I < resolutionsSO.AvailableResolutions.Length; I++)
                {
                    if (resolutionsSO.AvailableResolutions[I].width == width && resolutionsSO.AvailableResolutions[I].height == height)
                    {
                        current = resolutionsSO.AvailableResolutions[I];
                        resValue = I;
                    }
                }
            if (PlayerPrefs.HasKey("ScreenFullscreen"))
                fullScreen = PlayerPrefs.GetInt("ScreenFullscreen") == 1;
            else
                fullScreen = true;
            UpdateResolution();
        }
        else if (!UIonly)
        {
            current = resolutionsSO.AvailableResolutions[resValue];
            for (int I = 1; I < resolutionsSO.AvailableResolutions.Length; I++)
            {
                if (resolutionsSO.AvailableResolutions[I].width > current.width && resolutionsSO.AvailableResolutions[I].width < Screen.width &&
                    resolutionsSO.AvailableResolutions[I].height > current.height && resolutionsSO.AvailableResolutions[I].height < Screen.height)
                {
                    current = resolutionsSO.AvailableResolutions[I];
                    resValue = I;
                }
            }
            fullScreen = true;
        }
        if (toggleComponent != null)
            toggleComponent.SetIsOnWithoutNotify(fullScreen);
        if (dropDownComponent != null)
            dropDownComponent.SetValueWithoutNotify(resValue);
    }
    public void ChangeResolution(int value)
    {
        current = resolutionsSO.AvailableResolutions[value];
        UpdateResolution();
    }
    public void ChangeFullscreen(bool value)
    {
        fullScreen = value;
        UpdateResolution();
    }
    public void UpdateResolution()
    {
        Screen.SetResolution(current.width, current.height, fullScreen);

        PlayerPrefs.SetInt("ScreenWidth", current.width);
        PlayerPrefs.SetInt("ScreenHeight", current.height);
        PlayerPrefs.SetInt("ScreenFullscreen", fullScreen ? 1 : 0);
    }
}
