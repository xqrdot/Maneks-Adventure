using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public string actionDescription = "Взаимодействовать";
    public int id;

    [Header("Behaviour")]
    [SerializeField] bool controlsDoor = false;
    [SerializeField] bool controlsGame = false;
    [Space(20)]
    public bool requiresInput = true;
    public bool canBeActivated = true;
    public bool canBeDeactivated = true;
    public bool m_canBeActivated, m_canBeDeactivated = false;

    [Header("SFX/GFX")]
    [SerializeField] AudioClip sfx_activation = null;
    [SerializeField] AudioClip sfx_deactivation = null;
    [SerializeField] Sprite sprite_true;
    [SerializeField] Sprite sprite_false;
    Sprite sprite_default;

    AudioSource source;

    private void Awake()
    {
        if (canBeActivated) { m_canBeActivated = true; }
    }

    private void Start()
    {
        source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
        if (GetComponent<SpriteRenderer>() != null)
        {
            sprite_default = GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!requiresInput)
        {
            Activate(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!requiresInput)
        {
            Deactivate(collision);
        }
    }


    public void Activate(Collider2D collision)
    {
        if (m_canBeActivated)
        {
            source.PlayOneShot(sfx_activation);
            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().sprite = sprite_true;
                GetComponent<SpriteRenderer>().material.SetFloat("_OutlineOffset", 0);
            }

            if (controlsDoor)
            {
                ManagerEvents.current.DoorwayTriggerEnter(id);
            }

            if (controlsGame)
            {
				if (collision.gameObject.CompareTag("Player") == true)
				{
					ManagerEvents.current.GlobalEvent(2); // End game
				}
            }

            m_canBeActivated = false;
            if (canBeDeactivated) { m_canBeDeactivated = true; }
        }
    }


    public void Deactivate(Collider2D collision)
    {
        if (m_canBeDeactivated)
        {
            source.PlayOneShot(sfx_deactivation);

            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().sprite = sprite_default;
            }

            if (controlsDoor)
            {
                ManagerEvents.current.DoorwayTriggerExit(id);
            }

            m_canBeDeactivated = false;
            if (canBeActivated == true) { m_canBeActivated = true; }
        }
    }
}
