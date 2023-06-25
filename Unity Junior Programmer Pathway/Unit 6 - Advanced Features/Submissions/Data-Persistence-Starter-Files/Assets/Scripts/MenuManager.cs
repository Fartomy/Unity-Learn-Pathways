using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public UnityEngine.UI.InputField inputField;
    public string username;
    public int bestScore;
    public Text bestScoreText;

    private void Awake()
    {
        if (Instance != null) 
            Destroy(gameObject);
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDatas();
        bestScoreText.text = $"Best Score: {username} : {bestScore}";
    }

    public void StartGame()
    {
        if (inputField.text != "")
        {
            username = inputField.text;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            inputField.placeholder.GetComponent<Text>().text = "Please valid username!";
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    [System.Serializable]
    class SaveData
    {
        public string username;
        public int bestScore;
    }
    
    public void SaveDatas()
    {
        SaveData data = new SaveData();
        data.bestScore = this.bestScore;
        data.username = this.username;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    
    public void LoadDatas()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            this.username = data.username;
            this.bestScore = data.bestScore;
        }
    }
}
