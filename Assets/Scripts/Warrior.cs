using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warrior : MonoBehaviour
{
    public static float maxHp = 1000f;
	public static float curHp = 1000f;
    public float attackDamage = -10f;
    public Slider healthBar;
    public float invincibilityTime = 1f;
    public bool isInvincible = false;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHp < 0) {
           // Defeat
           Debug.Log("Defeated");
        }
    }
    
    public void UpdateHp(float updateValue) {
        if (isInvincible) return;
        curHp = maxHp < curHp+updateValue ? maxHp : curHp+updateValue; // Mathf.Clamp(val, min, max)
        if (updateValue < 0) { // Damage taken, start invincibility
            StartCoroutine(StartInvincibility());
        }
        healthBar.value = curHp / maxHp;
    }

    IEnumerator StartInvincibility() {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    } 
}
