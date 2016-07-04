using UnityEngine;
using System.Collections;
using SocketIO;
using System.IO;

public class Streaming : MonoBehaviour
{
	private SocketIOComponent socket;
	private Texture2D videoTexture;
	private int width = 640;
	private int height = 360;

	public void Start() 
	{
		videoTexture = new Texture2D (width, height);
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On("canvas", PonerTextura);
	}
	
	public void PonerTextura(SocketIOEvent e)
	{
		JSONObject json = e.data;
		byte[] bytes = System.Convert.FromBase64String(json[0].str);
		videoTexture.LoadImage(bytes);
		GetComponent<Renderer>().material.mainTexture = videoTexture;
	}

}

