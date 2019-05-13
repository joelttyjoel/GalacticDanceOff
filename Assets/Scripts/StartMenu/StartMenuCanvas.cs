using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuCanvas : MonoBehaviour {
	private int height;
	private int width;

	public Button bButton1, bButton2, bButton3, bButton4;

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
			if (bButton1.gameObject.activeInHierarchy && bButton1.IsInteractable()) 
			{
				bButton1.onClick.Invoke ();
			} else if ((bButton2.gameObject.activeInHierarchy) && bButton2.IsInteractable()) 
			{
				bButton2.onClick.Invoke ();
			}
			else if ((bButton3.gameObject.activeInHierarchy) && bButton3.IsInteractable()) 
			{
				bButton3.onClick.Invoke ();
			}
			else if ((bButton4.gameObject.activeInHierarchy) && bButton4.IsInteractable()) 
			{
				bButton4.onClick.Invoke ();
			}
		}
	}

}
