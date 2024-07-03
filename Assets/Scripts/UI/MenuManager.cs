using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    #region Buttons in Main Menu
    public void Quit(){
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Will only be executed in the editor
        #endif
    }

    //Starts the game
    public void LetsSuckSomeDick(){
        SceneManager.LoadScene(1);
    }
    
    /* Unused at the moment
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    */
    #endregion
}
