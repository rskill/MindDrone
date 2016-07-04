using UnityEngine;
using System.Collections;
using SocketIO;
using UnityEngine.UI;

public class eSense : MonoBehaviour {


	public float velocidad = 0.1f;
	//public Rigidbody cuerpo;
	public CardboardHead cabeza;

	private SocketIOComponent socket;
	private float atencion = 0f;
	private float meditacion = 0f;
	public Scrollbar barraConcentracion;
	private int contador;


	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On ("neurosky-eSense", LeerSensor); //recibe concentracion y relajacion
	}
	public void LeerSensor(SocketIOEvent e){

		JSONObject json = e.data;
		JSONObject JSONatencion = json [0];
		atencion = JSONatencion [0].n;
		meditacion = JSONatencion [1].n;
		Debug.Log ("Atencion" + atencion);
		//Debug.Log ("Meditacion" + meditacion);

		//mover barra
		barraConcentracion.size = (atencion*2) / 200;
		//acciones de acuerdo a los eSenses


	}

	// Update is called once per frame
	void FixedUpdate () {
		if (atencion > 60) {
			transform.position += velocidad * cabeza.transform.forward;
		}


	}
}