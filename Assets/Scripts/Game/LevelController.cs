using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Text _killCountTextView;
    [SerializeField] Text _fpsTextView;
    [Header("KeyBinds")]
    [SerializeField] KeyCode _pauseKey;
    [SerializeField] KeyCode _respawnKey;

    [HideInInspector]
    public UnityEvent ToggleMenu;
    [HideInInspector]
    public UnityEvent RespawnEnemy;

    int _killCount = 0;
    float deltaTime;

    private void Start()
    {
        _killCount = PlayerPrefs.GetInt("KillCount");
        _killCountTextView.text = "SKELETONS SLAIN: " + _killCount.ToString();

        StartCoroutine(UpdateFPS());
    }

    // Update is called once per frame
    void Update()
    {
        // Open in-game menu
        if (Input.GetKeyDown(_pauseKey))
        {
            ToggleMenu.Invoke();
        }

        if (Input.GetKeyDown(_respawnKey))
        {
            Resummon();
        }
    }

    IEnumerator UpdateFPS()
    {
        yield return new WaitForSeconds(0.2f);
        deltaTime = (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        _fpsTextView.text = Mathf.Ceil(fps).ToString() + " FPS";
        StartCoroutine(UpdateFPS());
    }

    public void Resummon()
    {
        RespawnEnemy.Invoke();
    }

    public void IncreaseKillCount()
    {
        _killCount++;
        PlayerPrefs.SetInt("KillCount", _killCount);
        _killCountTextView.text = "SKELETONS SLAIN: " + _killCount.ToString();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
