using UnityEngine;
using UnityEngine.AI;

public class NavMeshEnemyChase : MonoBehaviour
{
    // references to the player's posistion
    public Transform player;
    // Enemy point of return, if the player is caught
    public Transform enemyWalkBackPoint;
    
    // Enemy chase range
    // Set high to ensure player is always chased
    public float chaseRange = 500f;
    // Distance to catch the player
    public float catchDistance = 1;

    // Reference for components
    private NavMeshAgent _agent;
    private Animator _animator;
    private AudioSource _audio;
    
    // Track if player is caught
    private bool _playerCaught = false;
    
    private void Start()
    {
        // Get the NavMeshAgent, Animator, and AudioSource components
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Chase player logic
        
        // If the player is not caught
        if (!_playerCaught)
        {
            // Calculate distance to player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Chase player if it is within chase range
            if (distanceToPlayer <= chaseRange)
            {
                // Running animation
                _animator.SetBool("isRunning", true);
                // Chase player
                _agent.SetDestination(player.position);

                // If player is caught
                if (distanceToPlayer <= catchDistance)
                {
                    // Player caught
                    _playerCaught = true;
                    // Walk audio speed
                    _audio.pitch = 0.5f;
                    // Walk animation
                    _animator.SetBool("isRunning", false);
                    // Enemy walk speed
                    _agent.speed = 3.5f; 
                }
            }
        }
        
        // Once _playerCaught is true
        else
        {
            // Enemy walks back to the enemyWalkBackPoint
            _agent.SetDestination(enemyWalkBackPoint.position);
        }
    }
}