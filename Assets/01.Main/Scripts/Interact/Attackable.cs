using System.Collections;
using UnityEngine;

public class Attackable : MonoBehaviour, IInteractable
{
    KnockBack knockback;
    Vector3 vfxPosition = new Vector3(0f, 0.9f, 0f);
    Enemy enemy;
    Renderer objRenderer;
    Color originColor;

    public void Interact()
    {
        Debug.Log("Attacked: " + gameObject.name);

        knockback = GetComponent<KnockBack>();
        enemy = GetComponent<Enemy>();
        knockback.PlayingKnockBack();
        var vfxObj = ObjectPool.Instance.GetPooledObject("HitEffect").gameObject;
        vfxObj.transform.position = this.transform.position + vfxPosition;
        enemy.TakeDamage(50f);
        //StartCoroutine(HitColorChange());
    }

    public IEnumerator HitColorChange()
    {
        objRenderer = GetComponent<Renderer>();
        originColor = objRenderer.material.color;

        objRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        objRenderer.material.color = originColor;
    }
}
