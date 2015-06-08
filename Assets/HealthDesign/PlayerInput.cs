using UnityEngine;
using UnityEngine.UI;

class PlayerMovementBloodSugarAffector : IBloodSugarAffector
{
    private int _energyOutput = 0;

    //This adds bad state.. redo
    public float GetAlterationForTick(double tick, IDiabetesPatient patient)
    {
        var energyOutput = -_energyOutput;
        _energyOutput = 0;
        return energyOutput *10;
    }

    public bool IsExpired(double tick)
    {
        return false;
    }

    public string Name
    {
        get { return "Movement"; }
    }

    public void QueueEnergyOutput(int bloodSugarAffect)
    {
        _energyOutput += bloodSugarAffect;
    }
}
	

public class PlayerInput : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
    public int moveBloodSugarAffect = 1;
    public int jumpBloodSugarAffect = 5;
	public Text text;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private PlayerPhysics _playerPhysics;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    private GifAnimation _gifAnimation;
    private DiabetesSimulator _simulator;
    private readonly PlayerMovementBloodSugarAffector _bloodSugarAffector = new PlayerMovementBloodSugarAffector();

    void Awake()
	{
		text = FindObjectOfType<Text>();
	    _gifAnimation = GetComponent<GifAnimation>();
        _playerPhysics = GetComponent<PlayerPhysics>();
	    _simulator = GetComponent<DiabetesSimulator>();
        _simulator.addAffector(_bloodSugarAffector);
		// listen to some events for illustration purposes
		_playerPhysics.onControllerCollidedEvent += onControllerCollider;
		_playerPhysics.onTriggerEnterEvent += onTriggerEnterEvent;
		_playerPhysics.onTriggerExitEvent += onTriggerExitEvent;
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	    Food food = col.GetComponent<Food>();
	    if (food)
	    {
	        food.Eat(this);
		}
		var story = col.GetComponent<GifStory>();
		if(col.GetComponent<GifStory>()){
			story.Bump(this);
		}
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .5f);
    }

	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );

	}

	#endregion

    void PlayAnimation(PlayerAnimState state)
    {
        _gifAnimation.CurrentState = state;
    }

	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		// grab our current _velocity to use as a base for all calculations
		_velocity = _playerPhysics.velocity;
	    int currentBloodSugarAffect = 0;

		if( _playerPhysics.isGrounded )
			_velocity.y = 0;

		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

		    if (_playerPhysics.isGrounded)
		    {
                PlayAnimation(PlayerAnimState.MOVE);
		        currentBloodSugarAffect += moveBloodSugarAffect;
		    }
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

            if (_playerPhysics.isGrounded)
            {
                PlayAnimation(PlayerAnimState.MOVE);
                currentBloodSugarAffect += moveBloodSugarAffect;

            }
        }
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _playerPhysics.isGrounded )
                PlayAnimation(PlayerAnimState.IDLE);
        }


		// we can only jump whilst grounded
		if( _playerPhysics.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
            PlayAnimation(PlayerAnimState.JUMP);
            currentBloodSugarAffect += jumpBloodSugarAffect;
        }


		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _playerPhysics.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		_playerPhysics.move( _velocity * Time.deltaTime );
        _bloodSugarAffector.QueueEnergyOutput(currentBloodSugarAffect);

	}

}
