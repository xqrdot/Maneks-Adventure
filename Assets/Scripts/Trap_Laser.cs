using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Trap_Laser : MonoBehaviour
{
    [SerializeField] private int damage = 2;

    DamageTriggers damageTrigger;
    Light2D light2D;

    private void Start() {

        if (gameObject.GetComponent<DamageTriggers>() == null)
            gameObject.AddComponent<DamageTriggers>();
        if (gameObject.GetComponent<Light2D>() == null)
            gameObject.AddComponent<Light2D>();

        damageTrigger = gameObject.GetComponent<DamageTriggers>();
        light2D = gameObject.GetComponent<Light2D>();

        // Vector3[] shapePath = new [] { new Vector3() };

        light2D.lightType = Light2D.LightType.Freeform;
        // light2D. = shapePath;
    }
}