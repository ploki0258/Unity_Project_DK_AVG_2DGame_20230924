using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	#region ³æ¨Ò
	public static MenuManager instance;

	private void Awake()
	{
		instance = this;
	}
	#endregion
	[SerializeField] Scene scene;

	public void StartGame()
	{
		SceneManager.LoadScene("¹CÀ¸³õ´º");
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
