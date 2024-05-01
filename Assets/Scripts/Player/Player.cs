using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// TODO Implement Tower Placement
    
// TODO Implement Movement I think either thumbstick should move the player or Teleportation should be implemented
// TODO Attach HUD to the player so it moves with the player and views game state(Health, Score, Money, etc)

namespace Player
{
    public enum PlayerState
    {
        FirstPerson,
        BuildView
    }
    public class Player : MonoBehaviour
    {
        // Singleton instance
        public static Player Instance { get; private set; }

        public XRController leftController;
        public XRController rightController;
        public float movementSpeed = 1.0f;
        public XRRig rig;

        public int health = 100;
        public int score = 0;
        public int money = 0;
        private PlayerState _currentState;
        
        

        void Awake()
        {

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject); //might just destroy instead of returning
            }
        }

        void Update()
        {
            HandleInput();
            MovePlayer();
        }

        private void HandleInput()
        {
            // Example: Process input from the controllers
            // Implement logic to read from the controllers
        }

        private void MovePlayer()
        {
            // Implement movement logic here based on inputAxis
        }
        
        public void getReward(int reward)
        {
            score += reward;
        }
        
        public void takeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                // Respawn 
                // Reduce Level health 
                // Possibly implement 
            }
        }
        
        private void ToggleState()
        {
            if (_currentState == PlayerState.FirstPerson)
            {
                _currentState = PlayerState.BuildView;
            }
            else
            {
                _currentState = PlayerState.FirstPerson;
            }
        }
        
        public PlayerState GetCurrentState()
        {
            return _currentState;
        }
        
        private void SpawnTower()
        {
            // Set the tower at the pointers location
            // Reduce money
        }

       
    }
}
