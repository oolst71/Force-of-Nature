using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingEnableTrigger : MonoBehaviour
{

    [SerializeField] GameObject elm;
    OnboardingElementBehaviour eb;

    private void Start()
    {
        eb = elm.GetComponent<OnboardingElementBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eb.ActivateElement();
        }
    }
}
