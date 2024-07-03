using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake(){
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            //if pause screen already active unpause
            if(pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }
    }

    #region Game Over

    //Activate game over screen
    public void GameOver(){
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    //Game over functions
    public void Restart(){
        //Тут должно быть Resume, я не знаю какого хуя этот чел в туторе написал такой код
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }

    public void Quit(){
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Will only be executed in the editor
        #endif
    }

    #endregion

    #region Pause

    public void PauseGame(bool status){
        //if status == true pause | if status == false unpause
        pauseScreen.SetActive(status);

        if(status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
}
