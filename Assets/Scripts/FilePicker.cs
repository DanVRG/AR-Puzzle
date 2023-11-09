using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

#if WINDOWS_UWP
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
#endif


public class FilePicker : MonoBehaviour
{
	public Texture2D customTexture;
	public GameObject lb;
	public string nameOfPic;
	public bool isPicSelected = false;

    void OnEnable()
    {
#if WINDOWS_UWP
		Debug.Log("***********************************");
		Debug.Log("File Picker start.");
		Debug.Log("***********************************");

		UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
		{
			var filepicker = new FileOpenPicker();
			filepicker.FileTypeFilter.Add(".jpg");
			filepicker.FileTypeFilter.Add(".png");
			filepicker.FileTypeFilter.Add(".jpeg");

			var file = await filepicker.PickSingleFileAsync();
			UnityEngine.WSA.Application.InvokeOnAppThread(async () => 
			{
				Debug.Log("***********************************");
				nameOfPic = (file != null) ? file.Name : "No data";
				Debug.Log("Name: " + name);
				Debug.Log("***********************************");
				string path = (file != null) ? file.Path : "No data";
				Debug.Log("Path: " + path);
				Debug.Log("***********************************");

				if(file != null)
				{
					// Ensure the stream is disposed once the image is loaded
					using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
					{
						var readStream = fileStream.AsStreamForRead();
						var byteArray = new byte[readStream.Length];
						await readStream.ReadAsync(byteArray, 0, byteArray.Length);

						customTexture = new Texture2D(2, 2);
						customTexture.LoadImage(byteArray);

						isPicSelected = true;

						lb.SetActive(true);
						GameObject[] dataLines = GameObject.FindGameObjectsWithTag("Dataline");
						foreach (GameObject d in dataLines)
						{
							Destroy(d);
						}
						lb.GetComponent<LoadLeaderboardScript>().nameOfPicture = nameOfPic;
						lb.GetComponent<LoadLeaderboardScript>().enabled = true;
						
					}
					
				}

			}, false);
		}, false);

		
		Debug.Log("***********************************");
		Debug.Log("File Picker end.");
		Debug.Log("***********************************");
		this.GetComponent<FilePicker>().enabled = false;
#else
		nameOfPic = "nature.jpg";
		string path = @"C:\Users\Dani\Pictures\"+ nameOfPic;
		var imageBytes = File.ReadAllBytes(path);
		customTexture = new Texture2D(2, 2); //placeholder texture
		customTexture.LoadImage(imageBytes);

		isPicSelected = true;

		lb.SetActive(true);
		GameObject[] dataLines = GameObject.FindGameObjectsWithTag("Dataline");
		foreach (GameObject d in dataLines)
		{
			Destroy(d);
		}
		lb.GetComponent<LoadLeaderboardScript>().nameOfPicture = nameOfPic;
		lb.GetComponent<LoadLeaderboardScript>().enabled = true;

		this.GetComponent<FilePicker>().enabled = false;		
#endif
	}
}
