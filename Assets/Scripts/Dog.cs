using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Damageable;

public class Dog : MonoBehaviour
{
    Material material;
    Damageable damageable;
    
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        damageable = GetComponent<Damageable>();
        material = GetComponent<MeshRenderer>().material;
        originalColor = material.color;

        //while (damageable.invincible)
        //{
        //    material.color = Color.red;
        //    material.color = originalColor;
        //}
    }

    public void KillDog()
    {
        Destroy(this);
    }

    public void DamageFlash()
    {
        StartCoroutine(DamageFlasher());
    }

    IEnumerator DamageFlasher()
    {
        material.color = Color.red;
        yield return new WaitForSeconds(damageable.invincibleSeconds);
        material.color = originalColor;
    }
}
