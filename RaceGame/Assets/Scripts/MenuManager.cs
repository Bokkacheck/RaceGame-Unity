using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu_Canvas;
    [SerializeField]
    private GameObject settings_Canvas;
    [SerializeField]
    private GameObject carSelection_Canvas;

    void Start()
    {
        settings_Canvas.SetActive(false);
        carSelection_Canvas.SetActive(false);
        menu_Canvas.SetActive(true);
    }

    public void LoadLevel(int a)
    {
        SceneManager.LoadScene(a);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void loadMenu()
    {
        menu_Canvas.SetActive(true);
        settings_Canvas.SetActive(false);
        carSelection_Canvas.SetActive(false);
    }
    public void loadCarSelection()
    {
        carSelection_Canvas.SetActive(true);
        menu_Canvas.SetActive(false);
        settings_Canvas.SetActive(false);
    }

    public void loadSettings()
    {
        menu_Canvas.SetActive(false);
        settings_Canvas.SetActive(true);
        carSelection_Canvas.SetActive(false);
    }
}
