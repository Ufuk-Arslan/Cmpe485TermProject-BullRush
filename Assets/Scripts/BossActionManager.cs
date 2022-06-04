using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BossActionManager : MonoBehaviour {
	private Warrior warrior;
	public GameObject target;
	public GameObject hero;
	public Slider healthBar;
	public float maxHp = 1000f;
	public float curHp = 1000f;
	public float attackDamage = -80f;
	public float moveSpeed = 11f;
	public float runSpeed = 40f;
	public float runTriggerDistance = 30f;
	public float stopRunTriggerDistance = 9f;
	public float rotationSpeed = 6f;
	public float attackTriggerDistance = 7f;
	public float attackTime = 0.3f;
	public float attackInitTime = 0.5f;
	public float runDuration;
	public float curRunDruation;
	public float runDelay = 20f;
	public bool isAttacking = false;
	public bool isAttackEffective = false;
	public bool isWalking = true;
	public bool runToWalk = false;
	public bool isRunning = false;
	public bool isDead = false;
	public bool isJumping = false;
	public float jumpTime = 1f;
	private Rigidbody rb;
	private Vector3 direction;
	private Vector3 runFinish;
	private Vector3 runStart;
	Animator myAnimator;
	// Use this for initialization
	void Start () {
		myAnimator = target.GetComponent<Animator>();
		warrior = hero.GetComponent<Warrior>();
		rb = target.GetComponent<Rigidbody>();
		myAnimator.SetBool("idle", true);
		maxHp *= DifficultySelection.difficultyMultiplier;
		curHp = maxHp;
		healthBar.value = maxHp;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isDead) return;
		if (curHp <= 0)
		{
			isDead = true;
			StopAllCoroutines();
			StartCoroutine(DeathAnimation());
			return;
		}
	}
	
    private void FixedUpdate()
    {
        if (isDead)
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
		}
		if (isDead || isAttacking || isJumping) return;
		direction = hero.transform.position - transform.position;
		direction.y = 0;
		rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.FromToRotation(Vector3.forward, direction), rotationSpeed * Time.deltaTime);
		if (isRunning) 
		{
			if (curRunDruation >= runDuration)
			{
				isRunning = false;
				runToWalk = true;
				rb.velocity = new Vector3(0, 0, 0);
				StartCoroutine(TrailEndWait());
				StartCoroutine(JumpAttack());
				return;
			}
			Run();
			return;
		}
		if (direction.magnitude < attackTriggerDistance)
        {
			isWalking = false;
			isRunning = false;
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
		} else if (!runToWalk)
		{
			if (!isRunning)
			{
				isRunning = true;
				isWalking = false;
				runToWalk = false;
				isAttackEffective = true;
				runFinish = hero.transform.position;
				runFinish.y = transform.position.y;
				runStart = transform.position;
				runDuration = Vector3.Distance(runFinish, runStart) / runSpeed;
				curRunDruation = 0;
				transform.GetComponentInChildren<TrailRenderer>().enabled = true;
				StartCoroutine(JumpAttack());
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && isAttackEffective)
		{
			warrior.UpdateHp(attackDamage);
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
		curRunDruation += Time.deltaTime;
		transform.position = Vector3.Lerp(runStart, runFinish, curRunDruation/runDuration);
	}

	IEnumerator TrailEndWait()
	{
		yield return new WaitForSeconds(0.5f);
		transform.GetComponentInChildren<TrailRenderer>().enabled = false;
	}

	IEnumerator AttackAnimation()
	{
		ClearAllBool();
		myAnimator.SetBool("attack_01", true);
		isAttacking = true;
		yield return new WaitForSeconds(attackInitTime);
		isAttackEffective = true;
		yield return new WaitForSeconds(attackTime - attackInitTime);
		myAnimator.SetBool("attack_02", true);
		yield return new WaitForSeconds(attackTime);
		isAttackEffective = false;
		isAttacking = false;
	}

	IEnumerator JumpAttack()
    {
		isJumping = true;
		ClearAllBool();
		myAnimator.SetBool("attack_03", true);
		yield return new WaitForSeconds(jumpTime);
		isJumping = false;
		ClearAllBool();
		myAnimator.SetBool("run", true);
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
}
