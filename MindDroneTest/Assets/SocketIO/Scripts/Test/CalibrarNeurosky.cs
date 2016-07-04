using UnityEngine;
using System.Collections;
using SocketIO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CalibrarNeurosky : MonoBehaviour {

	private float promedioguiño = 0;
	private SocketIOComponent socket;
	private int lecturas = 0; 
	private float tiempo = 0, tiempoupdate = 0;
	private float tiempoaux = 0, tiempoupdateaux = 0;
	private bool  guiño = false;
	private GameObject goc;
	private MeshRenderer mc, mt1, mt2;
	private GameObject got1, got2;


	void Start () {

		goc = GameObject.Find ("Blink2");
		mc = goc.GetComponent<MeshRenderer>();

		got1 = GameObject.Find ("Titulo1");
		got2 = GameObject.Find ("Titulo2");

		mt1 = got1.GetComponent<MeshRenderer> ();
		mt2 = got2.GetComponent<MeshRenderer> ();

		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On ("neurosky-blink", LeerGuiños); //recibe guiños
		socket.On ("neurosky-eSense", LeerSensor); //recibe concentracion y relajacion
		socket.On ("neurosky-signal", MonitorearSeñal); //recibe intensidad de señal
	}
	//mediciones de intensidad de guiños 
	public void LeerGuiños(SocketIOEvent e){
		JSONObject json = e.data;
		//convierte al objeto json en un flotante
		float fuerzaguiño = json [0].n;
		Debug.Log (fuerzaguiño);
		//Toma lecturas cada 5 segundos, las demas seran ignoradas
		tiempo = Time.time;
		if ((tiempo - tiempoaux) > 5 || tiempo == 0) {
			//muestreo de 10 lecturas
			if(lecturas<10){
				CalibrarGuiños(fuerzaguiño);
				tiempoaux = tiempo;
			}
			else{
				//calcular promedio de guiño intensional
				promedioguiño/=lecturas;
				print("promedio guiño intencional: " + promedioguiño);
				string promedio = promedioguiño.ToString();
				System.IO.File.WriteAllText(@"C:\Users\Public\Documents\Unity Projects\MindDroneTest\archivo.cnf", promedio);
				//carga escena de video
				Application.LoadLevel("video"); 
			}	
		}
	}

	//metodo que detecta guiños
	public void CalibrarGuiños(float fuerzaguiño){
		mc.enabled = true;
		promedioguiño += fuerzaguiño;
		lecturas++;
		Debug.Log (fuerzaguiño + "intencional" + Time.time);
		guiño = true;
	}

	public void LeerSensor(SocketIOEvent e){
		//Debug.Log (e.data);
		//acciones de acuerdo a los eSenses
	}

	public void MonitorearSeñal(SocketIOEvent e){
		//Debug.Log (e.data);
		//acciones de acuerdo a la señal

	}
	void Update(){
		//cambios en la GUI de acuerdo al tiempo
		tiempoupdate = Time.time;
		if (guiño == true) {
			tiempoupdateaux = tiempoupdate;
			guiño = false;
			mt1.enabled = true;
			mt2.enabled = false;
		}
		if (tiempoupdate - tiempoupdateaux > 0.4) {
				mc.enabled = false;
		}
		if (tiempoupdate - tiempoaux > 5) {
			Debug.Log("entra");
			mt1.enabled = false;
			mt2.enabled = true;
		}
	}
}
