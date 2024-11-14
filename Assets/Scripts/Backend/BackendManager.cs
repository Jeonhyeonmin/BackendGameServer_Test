using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		BackendSetup();
	}

	private void BackendSetup()
	{
		var bro = Backend.Initialize();

		if (bro.IsSuccess())
		{
			Debug.Log($"�ʱ�ȭ ���� : {bro}");
		}
        else
        {
			Debug.LogError($"�ʱ�ȭ ���� : {bro}");
        }
    }
}
