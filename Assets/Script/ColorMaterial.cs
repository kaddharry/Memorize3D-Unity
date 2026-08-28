using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMaterial : MonoBehaviour
{

    //variables para el material, el color del material y la dirección
    Material material;
    Color color;
    int direccion = 1;

    void Start()
    {

        //Capturo del objeto el material y el color del material
        material = GetComponent<Renderer>().material;
        color = material.color;

    }

    void Update()
    {

        //Compruebo en qué dirección debo ir
        if (color.b >= 1.0f)
        { //azul a 1 (fucsia)
            direccion = -1;
        }
        else if (color.b <= 0.0f)
        { //azul a 0 (naranja)
            direccion = 1;
        }

        //Incremento o decremento cantidad de azul
        color.b += direccion * Time.deltaTime;

        //Asigno el color al material
        material.color = color;

    }

}