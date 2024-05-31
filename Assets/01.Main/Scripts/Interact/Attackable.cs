using UnityEngine;

public class Attackable : MonoBehaviour, IInteractable
{
    KnockBack knockback;

    public void Interact()
    {
        Debug.Log("Attacked: " + gameObject.name);

        knockback = GetComponent<KnockBack>();
        knockback.PlayingKnockBack();
    }
}
