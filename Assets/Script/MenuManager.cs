using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	#region ���
	public static MenuManager instance;

	private void Awake()
	{
		instance = this;
	}
	#endregion
	[SerializeField] Scene scene;

	public void StartGame()
	{
		SceneManager.LoadScene("�C������");
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
