using UnityEngine;

public class Attackable : MonoBehaviour, IInteractable
{
    KnockBack knockback;
    Vector3 vfxPosition = new Vector3(0f, 0.9f, 0f);

    public void Interact()
    {
        Debug.Log("Attacked: " + gameObject.name);

        knockback = GetComponent<KnockBack>();
        knockback.PlayingKnockBack();
        var vfxObj = ObjectPool.Instance.GetPooledObject("HitEffect").gameObject;
        vfxObj.transform.position = this.transform.position + vfxPosition;
    }
}
