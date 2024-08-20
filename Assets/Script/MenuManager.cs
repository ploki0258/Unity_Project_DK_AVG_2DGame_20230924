using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField] Scene scene;

	#region 單例
	public static MenuManager instance;

	private void Awake()
	{
		instance = this;
	}
	#endregion

	public void StartGame()
	{
		SceneManager.LoadScene("遊戲場景");
	}

	public void RestartGame()
    {
		scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

    public void Quit()
    {
        Application.Quit();
    }
}
