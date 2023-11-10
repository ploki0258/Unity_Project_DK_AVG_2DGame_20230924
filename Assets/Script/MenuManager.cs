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

	public void StartGame()
    {
        SceneManager.LoadScene("�C������");
	}

    public void Quit()
    {
        Application.Quit();
    }
}
