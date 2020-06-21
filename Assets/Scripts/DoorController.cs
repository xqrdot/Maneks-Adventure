using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
  Vector3 initialPosition;
  [SerializeField] int id = 0;
  [SerializeField] Vector2 openDirection = Vector2.zero;
  [SerializeField] float timeToOpen = 1.5f;
  [SerializeField] float timeToClose = 1.5f;

  // Start is called before the first frame update
  void Start()
  {
    initialPosition = transform.position;
    ManagerEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
    ManagerEvents.current.onDoorwayTriggerExit += OnDoorwayClose;
  }

  private void OnDoorwayOpen(int id)
  {
    if (id == this.id)
    {
      LeanTween.moveLocal(gameObject, new Vector3(initialPosition.x + openDirection.x, initialPosition.y + openDirection.y), timeToOpen).setEaseOutQuad();
      //LeanTween.moveLocal(gameObject, new Vector3(initialPosition.x + openDirection.x, initialPosition.y + openDirection.y), timeToOpen).setEaseLinear();
    }
  }

  private void OnDoorwayClose(int id)
  {
    if (id == this.id)
    {
      LeanTween.moveLocal(gameObject, new Vector3(initialPosition.x, initialPosition.y), timeToClose).setEaseOutQuad();
    }
  }

  private void OnDestroy()
  {
    ManagerEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
    ManagerEvents.current.onDoorwayTriggerExit -= OnDoorwayClose;
  }
}
