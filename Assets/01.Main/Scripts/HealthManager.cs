using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public UIHealthBar healthBar;

    private HealthSystem healthSystem;

    private void Start()
    {
        healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Comma))
        {
            healthSystem.Damage(10);
        }
        if(Input.GetKeyDown(KeyCode.Period))
        {
            healthSystem.Heal(10);
        }
    }
}
