using UnityEngine;
using System.Collections;

public class Action_Manage : MonoBehaviour {
	public GameObject Target;
	public Transform hero;
	public float maxHp = 1000f;
	public float curHp = 1000f;
	public float attackDamage = 100f;
	public float moveSpeed = 9.5f;
	public float rotationSpeed = 6f;
	public float attackDistance = 8f;
	public float attackTime = 1f;
	public bool isAttacking = false;
	public bool isWalking = false;
	private Rigidbody rb;
	private Vector3 direction;
	Animator myAnimator;
	// Use this for initialization
	void Start () {
		myAnimator = Target.GetComponent<Animator>();
		rb = Target.GetComponent<Rigidbody>();
		myAnimator.SetBool("idle", true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		direction = hero.position - transform.position;
		direction.y = 0;
		rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.FromToRotation(Vector3.forward, direction), rotationSpeed * Time.deltaTime);
	}
	
    private void FixedUpdate()
    {
		if (isAttacking) return;
		if (direction.magnitude < attackDistance)
        {
			isWalking = false;
			StartCoroutine(Attack());
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

	private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && isAttacking)
            {
                WarriorController.UpdateHp(-attackDamage);
            }
        }

	public void UpdateHp(float updateValue) {
		curHp = maxHp < curHp+updateValue ? maxHp : curHp+updateValue;
		Debug.Log(curHp);
		if (curHp < 0) {
			ClearAllBool();
			myAnimator.SetBool ("die", true);
			// Congratulations
		}
	}

    void Walk (Vector3 movement_direction)
    {
		rb.MovePosition(transform.position + (movement_direction * moveSpeed * Time.deltaTime));
    }

	IEnumerator Attack()
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
