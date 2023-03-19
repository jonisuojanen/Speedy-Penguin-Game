using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject[] m_panels;

    List<GameObject> m_ObjectsToHide = new List<GameObject>();

    #region GAMEPLAY_METHDOS
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ContinueGame()
    {
        for(int i = 0; i< m_panels.Length;i++)
        {
            m_panels[i].SetActive(false);
        }
        ShowAllObjects();
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        ShowPanel(0);
    }

    #endregion

    #region MAIN_MENU_METHODS
    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowPanel(int index)
    {
        for(int i = 0; i<m_panels.Length;i++)
        {
            if(m_panels[i].activeSelf)
            {
                m_panels[i].SetActive(false);
            }
        }
        m_panels[index].SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region HELPER_METHODS
    public void HideObject(GameObject obj)
    {
        obj.SetActive(false);
        m_ObjectsToHide.Add(obj);
    }
    public void HideObject(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(false);
            m_ObjectsToHide.Add(obj[i]);
        }
    }
    public void ShowObject(GameObject obj)
    {
        obj.SetActive(true);
        m_ObjectsToHide.Remove(obj);
    }

    public void ShowAllObjects()
    {
        foreach (GameObject obj in m_ObjectsToHide)
        {
            obj.SetActive(true);
            m_ObjectsToHide.Remove(obj);
        }
    }
    #endregion

}
