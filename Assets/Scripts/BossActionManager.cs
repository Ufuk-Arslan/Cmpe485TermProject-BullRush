using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossActionManager : MonoBehaviour {
	private Warrior warrior;
	public GameObject target;
	public GameObject hero;
	public Slider healthBar;
	public float maxHp = 1000f;
	public float curHp = 1000f;
	public float attackDamage = 100f;
	public float moveSpeed = 11f;
	public float runSpeed = 40f;
	public float runTriggerDistance = 30f;
	public float stopRunTriggerDistance = 9f;
	public float rotationSpeed = 6f;
	public float attackTriggerDistance = 8f;
	public float attackTime = 0.5f;
	public float attackInitTime = 0.4f;
	public float runDelay = 8f;
	public bool isAttacking = false;
	public bool isAttackEffective = false;
	public bool isWalking = true;
	public bool runToWalk = false;
	public bool isRunning = false;
	public bool isDead = false;
	private Rigidbody rb;
	private Vector3 direction;
	private Vector3 runDirection;
	private Vector3 runStart;
	Animator myAnimator;
	// Use this for initialization
	void Start () {
		myAnimator = target.GetComponent<Animator>();
		warrior = hero.GetComponent<Warrior>();
		rb = target.GetComponent<Rigidbody>();
		myAnimator.SetBool("idle", true);
		healthBar.value = maxHp;
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
	}
	
    private void FixedUpdate()
    {
		if (isDead || isAttacking) return;
		direction = hero.transform.position - transform.position;
		direction.y = 0;
		rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.FromToRotation(Vector3.forward, direction), rotationSpeed * Time.deltaTime);
		if (isRunning) 
		{
			float x = transform.position.x;
			float y = transform.position.y;
			if ((runStart.x < x && runDirection.x < x) || (runStart.x > x && runDirection.x > x) || (runStart.y < y && runDirection.y < y) || (runStart.y > y && runDirection.y > y))
			{
				isRunning = false;
				runToWalk = true;
				return;
			}
			Run();
			return;
		}
		if (direction.magnitude < attackTriggerDistance)
        {
			isWalking = false;
			isRunning = false;
			runToWalk = false;
			StartCoroutine(AttackAnimation());
        } else if (runToWalk || direction.magnitude < runTriggerDistance) 
		{
			if (!isWalking)
            {
				ClearAllBool();
				myAnimator.SetBool("walk", true);
				StartCoroutine(DelayRunToWalk());
				isRunning = false;
				isWalking = true;
			}
			Walk(direction.normalized);
		} else
		{
			if (!isRunning)
			{
				ClearAllBool();
				myAnimator.SetBool("run", true);
				isRunning = true;
				isWalking = false;
				runToWalk = false;
				isAttackEffective = true;
				runDirection = direction;
				runStart = transform.position;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && isAttackEffective)
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
        healthBar.value = curHp / maxHp;
	}

    void Walk (Vector3 movementDirection)
    {
		rb.MovePosition(transform.position + (movementDirection * moveSpeed * Time.deltaTime));
    }

	void Run () {
		rb.MovePosition(transform.position + (runDirection.normalized * runSpeed * Time.deltaTime));
	}

	IEnumerator AttackAnimation()
	{
		ClearAllBool();
		myAnimator.SetBool("attack_01", true);
		isAttacking = true;
		yield return new WaitForSeconds(attackInitTime);
		isAttackEffective = true;
		yield return new WaitForSeconds(attackTime);
		myAnimator.SetBool("attack_01", false);
		myAnimator.SetBool("attack_02", true);
		yield return new WaitForSeconds(attackTime);
		isAttackEffective = false;
		isAttacking = false;
	}

	IEnumerator DelayRunToWalk() 
	{
		yield return new WaitForSeconds(runDelay);
		runToWalk = false;
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
