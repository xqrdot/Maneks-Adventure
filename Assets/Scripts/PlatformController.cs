using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] int id = 0;
    [SerializeField] Vector2 targetPosition = Vector2.zero;
    [SerializeField] float timeToMove = 10f;

    Vector3 initialPosition = Vector3.zero;


    void Start()
    {
        initialPosition = transform.position;

        StartCoroutine(Move(targetPosition));
    }

    IEnumerator Move()
    {
        LeanTween.moveLocal(gameObject, initialPosition, timeToMove).setEaseLinear();
        yield return new WaitForSeconds(timeToMove);
        StartCoroutine(Move(targetPosition));
        yield return null;
    }

    IEnumerator Move(Vector3 position)
    {
        LeanTween.moveLocal(gameObject, new Vector3(initialPosition.x + position.x, initialPosition.y + position.y), timeToMove).setEaseLinear();
        yield return new WaitForSeconds(timeToMove);
        StartCoroutine(Move());
        yield return null;
    }
}
