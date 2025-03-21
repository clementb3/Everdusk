using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuGraphiqueManager : MonoBehaviour
{

    public GameObject panelMenuPrincipal; // Référence au panel principal
    public GameObject panelGraphique;     // Référence au panel graphique

    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Toggle vSyncToggle;
    //public TMP_Dropdown fpsDropdown;
    public Slider shadowDistanceSlider;

    void Start()
    {
        // Qualité graphique globale //
        InitializeQualityDropdown();

        // Résolution de l’écran
        InitializeResolutionDropdown();

        // Mode Plein Écran
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Synchronisation verticale
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vSyncToggle.onValueChanged.AddListener(SetVSync);

        // Limitation des FPS
        //InitializeFPSDropdown();

        // Distance d’affichage des ombres
        shadowDistanceSlider.value = QualitySettings.shadowDistance;
        shadowDistanceSlider.onValueChanged.AddListener(SetShadowDistance);
    }

    private void InitializeQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        List<string> qualityLevels = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityLevels);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    private void InitializeResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> resolutions = new List<string>();
        foreach (var resolution in Screen.resolutions)
        {
            resolutions.Add($"{resolution.width}x{resolution.height} @ {resolution.refreshRateRatio}Hz");
        }
        resolutionDropdown.AddOptions(resolutions);
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private int GetCurrentResolutionIndex()
    {
        Resolution current = Screen.currentResolution;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width == current.width &&
                Screen.resolutions[i].height == current.height &&
                Screen.resolutions[i].refreshRateRatio.numerator == current.refreshRateRatio.numerator &&
                Screen.resolutions[i].refreshRateRatio.denominator == current.refreshRateRatio.denominator)
            {
                return i;
            }
        }
        return 0; // Par défaut, retourne la première résolution
    }


    /*private void InitializeFPSDropdown()
    {
        fpsDropdown.ClearOptions();
        List<string> fpsOptions = new List<string> { "30 FPS", "60 FPS", "120 FPS", "Unlimited" };
        fpsDropdown.AddOptions(fpsOptions);
        fpsDropdown.value = GetCurrentFPSIndex();
        fpsDropdown.RefreshShownValue();
        fpsDropdown.onValueChanged.AddListener(SetFPS);
    }*/

    private int GetCurrentFPSIndex()
    {
        int fps = Application.targetFrameRate;
        if (fps == 30) return 0;
        if (fps == 60) return 1;
        if (fps == 120) return 2;
        return 3; // Par défaut, "Unlimited"
    }

    public void SetQuality(int index)
    {
        Debug.Log("Qualité sélectionnée : " + QualitySettings.names[index]);
        QualitySettings.SetQualityLevel(index);
    }

    public void SetResolution(int index)
    {
        Resolution resolution = Screen.resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log($"Résolution définie : {resolution.width}x{resolution.height} @ {resolution.refreshRateRatio}Hz");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Mode plein écran : " + isFullscreen);
    }

    public void SetVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        Debug.Log("Synchronisation verticale : " + (isEnabled ? "Activée" : "Désactivée"));
    }

    /*public void SetFPS(int index)
    {
        int[] fpsValues = { 30, 60, 120, -1 }; 
        Application.targetFrameRate = fpsValues[index];
        Debug.Log("FPS définis à : " + (fpsValues[index] == -1 ? "Unlimited" : fpsValues[index] + " FPS"));
    }*/

    public void SetShadowDistance(float distance)
    {
        QualitySettings.shadowDistance = distance;
        Debug.Log("Distance des ombres définie à : " + distance);
    }

    public void OpenGraphiqueMenu()
    {
        panelMenuPrincipal.SetActive(false);
        
        panelGraphique.SetActive(true);
    }

    public void CloseGraphiqueMenu()
    {
        panelMenuPrincipal.SetActive(true);
        
        panelGraphique.SetActive(false);
    }

    
}
