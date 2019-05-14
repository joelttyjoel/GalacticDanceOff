using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuCanvas : MonoBehaviour {
	private int height;
	private int width;

	public Button[] buttons;

	public void SetHeight(int Height)
	{
		height = Height;
	}

	public void SetWidth(int Width)
	{
		width = Width;
	}

	public void SetFullscreen(bool Fullscreen)
	{
		Screen.SetResolution (height, width, Fullscreen);
	}

	public void Fullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	void Update()
	{
		if (Input.GetButtonDown ("Button B")) 
		{
			for (int i = 0; i < buttons.Length; i++) 
			{
				if (buttons [i].gameObject.activeInHierarchy && buttons [i].IsInteractable ()) 
				{
					buttons [i].onClick.Invoke ();
				}
			}
		}
	}

}
