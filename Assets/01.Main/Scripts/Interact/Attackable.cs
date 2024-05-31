using UnityEngine;

public class Attackable : MonoBehaviour, IInteractable
{
    KnockBack knockback;
    Vector3 vfxPosition = new Vector3(0f, 0.9f, 0f);
    Enemy enemy;

    public void Interact()
    {
        Debug.Log("Attacked: " + gameObject.name);

        knockback = GetComponent<KnockBack>();
        enemy = GetComponent<Enemy>();
        knockback.PlayingKnockBack();
        var vfxObj = ObjectPool.Instance.GetPooledObject("HitEffect").gameObject;
        vfxObj.transform.position = this.transform.position + vfxPosition;
        enemy.TakeDamage(50f);
    }
}
