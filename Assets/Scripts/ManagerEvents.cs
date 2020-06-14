using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEvents : MonoBehaviour
{
  public static ManagerEvents current;

  private void Awake() {
    current = this;
  }

  public event Action<int> onDoorwayTriggerEnter;
  public event Action<int> onDoorwayTriggerExit;
  public event Action<int> onGlobalEventChange;
  public event Action<bool> onTeleportActivate;

  public void DoorwayTriggerEnter(int id) { onDoorwayTriggerEnter?.Invoke(id); }
  public void DoorwayTriggerExit(int id) { onDoorwayTriggerExit?.Invoke(id); }
  public void GlobalEvent(int id) { onGlobalEventChange?.Invoke(id); }
  public void TeleportActivate(bool boolean) { onTeleportActivate?.Invoke(boolean); }
}
