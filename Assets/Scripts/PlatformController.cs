using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
  Vector3 initialPosition;
  [SerializeField] int id;
  [SerializeField] Vector2 targetPosition;
  [SerializeField] float timeToMove = 10f;

  // Start is called before the first frame update
  void Start()
  {
    initialPosition = transform.position;
    // ManagerEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
    // ManagerEvents.current.onDoorwayTriggerExit += OnDoorwayClose;

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

  // private void OnDoorwayOpen(int id)
  // {
  //   if (id == this.id)
  //   {
  //     LeanTween.moveLocal(gameObject, new Vector3(initialPosition.x + targetDirection.x, initialPosition.y + targetDirection.y), timeToMove).setEaseOutQuad();
  //     //LeanTween.moveLocal(gameObject, new Vector3(initialPosition.x + openDirection.x, initialPosition.y + openDirection.y), timeToOpen).setEaseLinear();
  //   }
  // }

  // private void OnDoorwayClose(int id)
  // {
  //   if (id == this.id)
  //   {
  //     LeanTween.moveLocal(gameObject, new Vector3(initialPosition.x, initialPosition.y), timeToMove).setEaseOutQuad();
  //   }
  // }

  // private void OnDestroy()
  // {
  //   ManagerEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
  //   ManagerEvents.current.onDoorwayTriggerExit -= OnDoorwayClose;
  // }
}
