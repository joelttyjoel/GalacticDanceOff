using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuCanvas : MonoBehaviour {
	private int height;
	private int width;

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

}
