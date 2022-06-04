using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour {
	private Warrior warrior;
	public GameObject target;
	public GameObject hero;
	public GameObject collectible;
	private Rigidbody rb;
	Animator myAnimator;
	private Vector3 direction;
	public bool isAppearing = true;
	public Vector3 startPos;
	public Vector3 endPos;
	private float curAppearDruation;
	private float appearDuration = 3f;
	public float rotationSpeed = 10f;
	public float moveSpeed = 6f;
	public float attackDamage = -50f;
	public float collectibleChance = 50f;
	// Use this for initialization
	void Start () {
		myAnimator = target.GetComponent<Animator> ();
		rb = target.GetComponent<Rigidbody>();
		hero = GameObject.FindWithTag("Player");
		warrior = hero.GetComponent<Warrior>();
		myAnimator.SetBool("Taunt", true);
		startPos = target.transform.position;
		endPos = new Vector3(startPos.x, 0.8f, startPos.z);
		collectibleChance /= DifficultySelection.difficultyMultiplier;
	}

	// Update is called once per frame
	void Update () {

	}
    private void FixedUpdate()
    {
		if (isAppearing)
		{
			Appear();
			return;
		}
		direction = hero.transform.position - transform.position;
		direction.y = 0;
		rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.FromToRotation(Vector3.forward, direction), rotationSpeed * Time.deltaTime);
		Walk(direction.normalized);
	}

	public void Appear()
    {
		curAppearDruation += Time.deltaTime;
		transform.position = Vector3.Lerp(startPos, endPos, curAppearDruation / appearDuration);
		if (curAppearDruation >= appearDuration)
        {
			isAppearing = false;
			ClearAllBool();
			myAnimator.SetBool("WalkFWD", false);
		}
	}
	void Walk(Vector3 movementDirection)
	{
		rb.MovePosition(transform.position + (movementDirection * moveSpeed * Time.deltaTime));
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			warrior.UpdateHp(attackDamage);
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Projectile")
		{
			int chance = Random.Range(0, 100);
			if (chance >= collectibleChance)
            {
				Vector3 collectiblePos = target.transform.position;
				collectiblePos.y += 2f;
				Instantiate(collectible, collectiblePos, Quaternion.identity);
            }
			Destroy(target);
		}
	}

	void ClearAllBool(){
		myAnimator.SetBool ("IdleNormal", false);
		myAnimator.SetBool ("WalkFWD",  false);
		myAnimator.SetBool ("Attack01", false);
		myAnimator.SetBool ("Die", false);
		myAnimator.SetBool ("Taunt", false);
	}
}
