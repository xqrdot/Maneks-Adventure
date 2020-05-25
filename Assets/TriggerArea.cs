using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
  public string actionDescription = "Взаимодействовать";
  public int id;

  [Header("Behaviour")]
  [SerializeField] bool controlsDoor = false;
  [Space(20)]
  public bool requiresInput = true;
  public bool canBeActivated = true;
  public bool canBeDeactivated = true;

  [Header("SFX/GFX")]
  [SerializeField] AudioClip sfx_activation = null;
  [SerializeField] AudioClip sfx_deactivation = null;
  [SerializeField] Sprite sprite_true;
  [SerializeField] Sprite sprite_false;
                   Sprite sprite_default;


  AudioSource source;

  private void Start()
  {
    source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
    sprite_default = GetComponent<SpriteRenderer>().sprite;
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (!requiresInput) {
      Activate();
    }
  }

  private void OnTriggerExit2D(Collider2D collision) {
    if (!requiresInput) {
      Deactivate();
    }
  }


  public void Activate()
  {
    if (canBeActivated)
    {
      source.PlayOneShot(sfx_activation);
      GetComponent<SpriteRenderer>().sprite = sprite_true;
      GetComponent<SpriteRenderer>().material.SetFloat("_OutlineOffset", 0);


      if (controlsDoor)
      {
        ManagerEvents.current.DoorwayTriggerEnter(id);
      }

      // TODO: Implement a better solution
      canBeActivated = false;
    }
  }


  public void Deactivate()
  {
    if (canBeDeactivated)
    {
      source.PlayOneShot(sfx_deactivation);
      GetComponent<SpriteRenderer>().sprite = sprite_default;

      if (controlsDoor)
      {
        ManagerEvents.current.DoorwayTriggerExit(id);
      }
    }
  }
}
