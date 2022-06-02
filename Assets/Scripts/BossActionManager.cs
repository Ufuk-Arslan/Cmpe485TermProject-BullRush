using UnityEngine;
using System.Collections;

public class BossActionManager : MonoBehaviour {
	private Warrior warrior;
	public GameObject target;
	public GameObject hero;
	public float maxHp = 1000f;
	public float curHp = 1000f;
	public float attackDamage = 100f;
	public float moveSpeed = 10f;
	public float rotationSpeed = 6f;
	public float attackDistance = 8f;
	public float attackTime = 0.5f;
	public bool isAttacking = false;
	public bool isWalking = false;
	public bool isDead = false;
	private Rigidbody rb;
	private Vector3 direction;
	Animator myAnimator;
	// Use this for initialization
	void Start () {
		myAnimator = target.GetComponent<Animator>();
		warrior = hero.GetComponent<Warrior>();
		rb = target.GetComponent<Rigidbody>();
		myAnimator.SetBool("idle", true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isDead) return;
		if (curHp < 0) {
			isDead = true;
			StartCoroutine(DeathAnimation());
			return;
		}
		direction = hero.transform.position - transform.position;
		direction.y = 0;
		rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.FromToRotation(Vector3.forward, direction), rotationSpeed * Time.deltaTime);
	}
	
    private void FixedUpdate()
    {
		if (isDead || isAttacking) return;
		if (direction.magnitude < attackDistance)
        {
			isWalking = false;
			StartCoroutine(AttackAnimation());
        } else
        {
			if (!isWalking)
            {
				ClearAllBool();
				myAnimator.SetBool("walk", true);
				isWalking = true;
			}
			Walk(direction.normalized);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && isAttacking)
		{
			warrior.UpdateHp(-attackDamage);
		}
	}

	private void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Projectile") 
		{
			UpdateHp(warrior.attackDamage);
		}
	}

	public void UpdateHp(float updateValue) {
		curHp = maxHp < curHp+updateValue ? maxHp : curHp+updateValue;
		Debug.Log(curHp);
	}

    void Walk (Vector3 movementDirection)
    {
		rb.MovePosition(transform.position + (movementDirection * moveSpeed * Time.deltaTime));
    }

	IEnumerator AttackAnimation()
	{
		ClearAllBool();
		myAnimator.SetBool("attack_01", true);
		isAttacking = true;
		yield return new WaitForSeconds(attackTime);
		myAnimator.SetBool("attack_01", false);
		myAnimator.SetBool("attack_02", true);
		yield return new WaitForSeconds(attackTime);
		isAttacking = false;
	}

	IEnumerator DeathAnimation()
	{
		isAttacking = false;
		isWalking = false;
		ClearAllBool();
		myAnimator.SetBool ("die", true);
		yield return new WaitForSeconds(attackTime * 6);
		// Congratulations
	}

	void ClearAllBool(){
		myAnimator.SetBool ("defy", false);
		myAnimator.SetBool ("idle",  false);
		myAnimator.SetBool ("dizzy", false);
		myAnimator.SetBool ("walk", false);
		myAnimator.SetBool ("run", false);
		myAnimator.SetBool ("jump", false);
		myAnimator.SetBool ("die", false);
		myAnimator.SetBool ("jump_left", false);
		myAnimator.SetBool ("jump_right", false);
		myAnimator.SetBool ("attack_01", false);
		myAnimator.SetBool ("attack_03", false);
		myAnimator.SetBool ("attack_02", false);
		myAnimator.SetBool ("damage", false);
	}
	public void Pressed_damage(){
		ClearAllBool();
		myAnimator.SetBool ("damage", true);
	}
	public void Pressed_idle(){
		ClearAllBool();
		myAnimator.SetBool ("idle", true);
	}
	public void Pressed_defy(){
		ClearAllBool();
		myAnimator.SetBool ("defy", true);
	}
	public void Pressed_dizzy(){
		ClearAllBool();
		myAnimator.SetBool ("dizzy", true);
	}
	public void Pressed_run(){
		ClearAllBool();
		myAnimator.SetBool ("run", true);
	}
	public void Pressed_walk(){
		ClearAllBool();
		myAnimator.SetBool ("walk", true);
	}
	public void Pressed_die(){
		ClearAllBool();
		myAnimator.SetBool ("die", true);
	}
	public void Pressed_jump(){
		ClearAllBool();
		myAnimator.SetBool ("jump", true);
	}
	public void Pressed_jump_left(){
		ClearAllBool();
		myAnimator.SetBool ("jump_left", true);
	}
	public void Pressed_jump_right(){
		ClearAllBool();
		myAnimator.SetBool ("jump_right", true);
	}
	public void Pressed_attack_01(){
		ClearAllBool();
		myAnimator.SetBool ("attack_01", true);
	}
	public void Pressed_attack_02(){
		ClearAllBool();
		myAnimator.SetBool ("attack_02", true);
	}
	public void Pressed_attack_03(){
		ClearAllBool();
		myAnimator.SetBool ("attack_03", true);
	}
}
