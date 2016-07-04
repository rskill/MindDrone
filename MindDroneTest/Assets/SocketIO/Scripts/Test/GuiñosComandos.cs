using UnityEngine;
using System.Collections;
using SocketIO;

public class GuiñosComandos : MonoBehaviour {
	private string linea = "";
	private float promedioguiño = 0;
	private SocketIOComponent socket;
	private float tiempo = 0, tiempoaux = 0;
	private int i = 0;
	private bool estavolando = false;

	// Use this for initialization
	void Start () {
		//lee archivo con promedio intencional de guiños
		System.IO.StreamReader file = 
			new System.IO.StreamReader(@"C:\Users\Public\Documents\Unity Projects\MindDroneTest\archivo.cnf");
		while((linea = file.ReadLine()) != null){
			promedioguiño = float.Parse(linea);
		}
		file.Close ();
		//obtiene el componente de socket.io
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		//recibe evento desde node
		socket.On ("neurosky-blink", LeerGuiños);
	}

	public void LeerGuiños(SocketIOEvent e){
		JSONObject json = e.data;
		//convierte el objeto json a flotante
		float fuerzaguiño = json [0].n;
		//Debug.Log (fuerzaguiño);
		if (fuerzaguiño > promedioguiño) { //compara si el guiño esta arriba del promedio
			tiempo = Time.time; 
			//comprueba si fue un blink o doble blink
			if(estavolando){
				if((tiempo - tiempoaux) < 1){
					//emite evento aterrizaje por el puerto a node
					socket.Emit("aterrizar");
					Debug.Log("aterrizar" + (i+1));
					estavolando = false;
				}
			}else{
				//emite evento despegue por el puerto a node
				socket.Emit("despegar");
				Debug.Log ("despegar"+ (i+1));
				estavolando = true;
			}
			tiempoaux = tiempo; //0.72
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
