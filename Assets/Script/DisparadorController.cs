using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparadorController : MonoBehaviour
{

	//Declaro la variable oculta para capturar el objeto a acoultar/mostrar
	public GameObject oculta;

	void Start()
	{
		//Oculto la plataforma al arrancar el juego
		oculta.SetActive(false);

	}

	//Se ejecuta al colisionar con el objeto
	void OnCollisionEnter(Collision colision)
	{
		Debug.Log(colision.gameObject.name);
		if (colision.gameObject.name == "Player")
		{
			//Muestro la plataforma
			oculta.SetActive(true);
		}
	}

}