using UnityEngine;

public class SaveManager
{
	public static SaveManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SaveManager();
			}
			return _instance;
		}
	}
	static SaveManager _instance = null;
}
