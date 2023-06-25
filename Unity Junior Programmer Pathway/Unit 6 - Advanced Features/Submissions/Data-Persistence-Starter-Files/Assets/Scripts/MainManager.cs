using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    private int m_Points = 0;
    private bool m_GameOver = false;
    private bool m_Started = false;
    
    public Rigidbody Ball;
    public Brick BrickPrefab;
    public Text ScoreText;
    public Text UsernameInfoText;
    public GameObject GameOverText;
    public int LineCount = 6;
    public string newUsername;
    void Awake()
    {
        newUsername = MenuManager.Instance.username;
        MenuManager.Instance.LoadDatas();
        UsernameInfoText.text = $"Best Score : {MenuManager.Instance.username} : {MenuManager.Instance.bestScore}";
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        
        if (m_Points > MenuManager.Instance.bestScore)
        {
            MenuManager.Instance.bestScore = m_Points;
            MenuManager.Instance.username = newUsername;
            MenuManager.Instance.SaveDatas();
            UsernameInfoText.text = $"Best Score : {MenuManager.Instance.username} : {MenuManager.Instance.bestScore}";
        }
    }
}
