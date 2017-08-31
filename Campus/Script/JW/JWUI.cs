using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JWUI : JWMonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Exit()
    {
        Destroy(transform.parent.gameObject);
    }

    //Game 
    public void PauseGame(GameObject pausePanel)
    {
        SetAllButtonsActive(false);
        Time.timeScale = 0;
        InstantiateUI(pausePanel);
    }
    public void ResumeGame(GameObject pausePanel)
    {
        SetAllButtonsActive(true);
        Time.timeScale = 1;
        Destroy(pausePanel.gameObject);
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Scene
    public virtual void LoadScene(string scenename)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scenename);
    }
    public void LoadNextScene()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            print("Scene index out of range");            
    }

    //Show and hide other UI
    void SetAllButtonsActive(bool b)
    {
        Button[] allbuttons = FindObjectsOfType<Button>();
        foreach (Button button in allbuttons)
            button.enabled = b;
    }
    public void InstantiateUI(GameObject uiPrefab)
    {
        InstantiateUI(uiPrefab, uiPrefab.transform.position);
    }
    void InstantiateUI(GameObject uiPrefab, Vector3 position)
    {
        JWInstantiate(uiPrefab, GameObject.Find("Canvas"), position);
    }
    public void ToggleUI(GameObject uiObject) {
        if (!uiObject.activeSelf)
            uiObject.SetActive(true);
        else
            uiObject.SetActive(false);
    }
    public void ShowText()
    {
        GetComponentInChildren<Text>().enabled = true;
    }
    public void HideText()
    {
        GetComponentInChildren<Text>().enabled = false;
    }
}
