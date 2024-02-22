using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

namespace AquaCulture
{
    [RequireComponent(typeof(CharacterController2D))]
    public class SubmarineCharacter : MonoBehaviour
    {
        static protected SubmarineCharacter s_SubmarineInstance;
        static public SubmarineCharacter PlayerInstance { get { return s_SubmarineInstance; } }

        public InventoryController inventoryController
        {
            get { return m_InventoryController; }
        }

        public UnityEvent OnTakingControl;
        public UnityEvent OnReleasingControl;

        public UnityEvent OnHidingInternals;
        public UnityEvent OnUnhidingInternals;

        [HideInInspector]
        public PlayerCharacter m_ControllingCharacter;

        public SpriteRenderer spriteRenderer;
        public Damageable damageable;
        public Damager meleeDamager;
        public BulletPool bulletPool;
        public Transform cameraFollowTarget;
        public ReplenishOxygenSupply OxygenSupplyObject;

        public float maxSpeed = 10f;
        public float groundAcceleration = 100f;
        public float groundDeceleration = 100f;
        [Range(0f, 1f)] public float pushingSpeedProportion;

        [Range(0f, 1f)] public float airborneAccelProportion;
        [Range(0f, 1f)] public float airborneDecelProportion;
        public float gravity = 50f;
        public float jumpSpeed = 20f;
        public float jumpAbortSpeedReduction = 100f;

        [Range(k_MinHurtJumpAngle, k_MaxHurtJumpAngle)] public float hurtJumpAngle = 45f;
        public float hurtJumpSpeed = 5f;
        public float flickeringDuration = 0.1f;

        public RandomAudioPlayer footstepAudioPlayer;
        public RandomAudioPlayer landingAudioPlayer;
        public RandomAudioPlayer hurtAudioPlayer;
        public RandomAudioPlayer meleeAttackAudioPlayer;
        public RandomAudioPlayer rangedAttackAudioPlayer;

        public float shotsPerSecond = 1f;
        public float bulletSpeed = 5f;
        public float holdingGunTimeoutDuration = 10f;
        public bool rightBulletSpawnPointAnimated = true;

        public float cameraHorizontalFacingOffset;
        public float cameraHorizontalSpeedOffset;
        public float cameraVerticalInputOffset;
        public float maxHorizontalDeltaDampTime;
        public float maxVerticalDeltaDampTime;
        public float verticalCameraOffsetDelay;

        public bool spriteOriginallyFacesLeft;


        protected CharacterController2D m_SubmarineController2D;

        protected CapsuleCollider2D m_Capsule;
        protected Transform m_Transform;
        protected Vector2 m_MoveVector;
        protected List<Pushable> m_CurrentPushables = new List<Pushable>(4);
        protected Pushable m_CurrentPushable;
        protected float m_TanHurtJumpAngle;
        protected WaitForSeconds m_FlickeringWait;
        protected Coroutine m_FlickerCoroutine;
        protected float m_ShotSpawnGap;
        protected WaitForSeconds m_ShotSpawnWait;
        protected Coroutine m_ShootingCoroutine;
        protected float m_NextShotTime;
        protected bool m_IsFiring;
        protected float m_ShotTimer;
        protected float m_HoldingGunTimeRemaining;
        protected TileBase m_CurrentSurface;
        protected float m_CamFollowHorizontalSpeed;
        protected float m_CamFollowVerticalSpeed;
        protected float m_VerticalCameraOffsetTimer;
        protected InventoryController m_InventoryController;

        protected Checkpoint m_LastCheckpoint = null;
        protected Vector2 m_StartingPosition = Vector2.zero;
        protected bool m_StartingFacingLeft = false;

        protected bool m_InPause = false;

        protected const float k_MinHurtJumpAngle = 0.001f;
        protected const float k_MaxHurtJumpAngle = 89.999f;
        protected const float k_GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.

        bool functionGateSurface = true;
        bool functionGateDive = false;

        //used in non alloc version of physic function
        protected ContactPoint2D[] m_ContactsBuffer = new ContactPoint2D[16];

        // MonoBehaviour Messages - called by Unity internally.
        void Awake()
        {
            s_SubmarineInstance = this;

            m_SubmarineController2D = GetComponent<CharacterController2D>();
            m_Capsule = GetComponent<CapsuleCollider2D>();
            m_Transform = transform;
            m_InventoryController = GetComponent<InventoryController>();
        }

        void Start()
        {
            hurtJumpAngle = Mathf.Clamp(hurtJumpAngle, k_MinHurtJumpAngle, k_MaxHurtJumpAngle);
            m_TanHurtJumpAngle = Mathf.Tan(Mathf.Deg2Rad * hurtJumpAngle);
            m_FlickeringWait = new WaitForSeconds(flickeringDuration);

            meleeDamager.DisableDamage();

            m_ShotSpawnGap = 1f / shotsPerSecond;
            m_NextShotTime = Time.time;
            m_ShotSpawnWait = new WaitForSeconds(m_ShotSpawnGap);

            if (!Mathf.Approximately(maxHorizontalDeltaDampTime, 0f))
            {
                float maxHorizontalDelta = maxSpeed * cameraHorizontalSpeedOffset + cameraHorizontalFacingOffset;
                m_CamFollowHorizontalSpeed = maxHorizontalDelta / maxHorizontalDeltaDampTime;
            }

            if (!Mathf.Approximately(maxVerticalDeltaDampTime, 0f))
            {
                float maxVerticalDelta = cameraVerticalInputOffset;
                m_CamFollowVerticalSpeed = maxVerticalDelta / maxVerticalDeltaDampTime;
            }

            m_StartingPosition = transform.position;
            m_StartingFacingLeft = GetFacing() < 0.0f;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Pushable pushable = other.GetComponent<Pushable>();
            if (pushable != null)
            {
                m_CurrentPushables.Add(pushable);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Pushable pushable = other.GetComponent<Pushable>();
            if (pushable != null)
            {
                if (m_CurrentPushables.Contains(pushable))
                    m_CurrentPushables.Remove(pushable);
            }
        }

        void Update()
        {
        if ((gameObject.GetComponent<Transform>().position.y > 25) && (functionGateSurface == true))
            {
                OxygenSupplyObject.GetComponent<ReplenishOxygenSupply>().HasReachedSurface();
                functionGateSurface = false;
                functionGateDive = true;
            }

           if ((gameObject.GetComponent<Transform>().position.y < 25) && (functionGateDive == true))
            {
                OxygenSupplyObject.GetComponent<ReplenishOxygenSupply>().HasDived();
                functionGateDive = false;
                functionGateSurface = true;
            }
        }


        void FixedUpdate()
        {
            if ( PlayerInput.Instance.Jump.Down && SubmarineCharacter.PlayerInstance.m_ControllingCharacter != null )
            {
                ReleaseControl();
                return;
            }

            //AirborneHorizontalMovement();
            AirborneVerticalMovement();
            m_SubmarineController2D.Move(m_MoveVector * Time.deltaTime);
            UpdateCameraFollowTargetPosition();
        }

        public void TakeControl( Damager xUser, Damageable xUsed )
        {
            TakeControl( xUser.gameObject.GetComponent<PlayerCharacter>() );
        }

        public void TakeControl( PlayerCharacter xController )
        {
            OnTakingControl.Invoke();
            OnHidingInternals.Invoke();

            // Ellen effects
            xController.MakePhysicsStatic();
            xController.TogglePhysicsSimulation( false );
            xController.gameObject.transform.parent = gameObject.transform;
            xController.OnReleasingControl.Invoke();

            // My effects
            this.MakePhysicsDynamic();
            m_ControllingCharacter = xController;
        }

        public void ReleaseControl()
        {
            OnReleasingControl.Invoke();
            OnUnhidingInternals.Invoke();

            // Ellen effects
            m_ControllingCharacter.MakePhysicsDynamic();
            m_ControllingCharacter.TogglePhysicsSimulation( true );
            m_ControllingCharacter.OnTakingControl.Invoke();

            // My effects
            this.MakePhysicsStatic();
            m_ControllingCharacter = null;
        }

        private void MakePhysicsStatic()
        {
            m_SubmarineController2D.Rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        private void MakePhysicsDynamic()
        {
            m_SubmarineController2D.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        protected void UpdateCameraFollowTargetPosition()
        {
            if ( cameraFollowTarget == null )
            {
                Debug.LogWarning( "Cannot update camera follow target if one is not set." );
                return;
            }

            float newLocalPosX;
            float newLocalPosY = 0f;

            float desiredLocalPosX = (spriteOriginallyFacesLeft ^ spriteRenderer.flipX ? -1f : 1f) * cameraHorizontalFacingOffset;
            desiredLocalPosX += m_MoveVector.x * cameraHorizontalSpeedOffset;
            if (Mathf.Approximately(m_CamFollowHorizontalSpeed, 0f))
                newLocalPosX = desiredLocalPosX;
            else
                newLocalPosX = Mathf.Lerp(cameraFollowTarget.localPosition.x, desiredLocalPosX, m_CamFollowHorizontalSpeed * Time.deltaTime);

            bool moveVertically = false;
            if (!Mathf.Approximately(PlayerInput.Instance.Vertical.Value, 0f))
            {
                m_VerticalCameraOffsetTimer += Time.deltaTime;

                if (m_VerticalCameraOffsetTimer >= verticalCameraOffsetDelay)
                    moveVertically = true;
            }
            else
            {
                moveVertically = true;
                m_VerticalCameraOffsetTimer = 0f;
            }

            if (moveVertically)
            {
                float desiredLocalPosY = PlayerInput.Instance.Vertical.Value * cameraVerticalInputOffset;
                if (Mathf.Approximately(m_CamFollowVerticalSpeed, 0f))
                    newLocalPosY = desiredLocalPosY;
                else
                    newLocalPosY = Mathf.MoveTowards(cameraFollowTarget.localPosition.y, desiredLocalPosY, m_CamFollowVerticalSpeed * Time.deltaTime);
            }

            cameraFollowTarget.localPosition = new Vector2(newLocalPosX, newLocalPosY);
        }

        protected IEnumerator Shoot()
        {
            while (PlayerInput.Instance.RangedAttack.Held)
            {
                if (Time.time >= m_NextShotTime)
                {
                    //SpawnBullet();
                    m_NextShotTime = Time.time + m_ShotSpawnGap;
                }
                yield return null;
            }
        }

        // Public functions - called mostly by StateMachineBehaviours in the character's Animator Controller but also by Events.
        public void SetMoveVector(Vector2 newMoveVector)
        {
            m_MoveVector = newMoveVector;
        }

        public void SetMovementMaxSpeed( float newMaxSpeed )
        {
            maxSpeed = newMaxSpeed;
        }

        public void SetAirborneGravity( float newAirborneGravity )
        {
            gravity = newAirborneGravity;
        }

        public void SetAirborneJumpSpeed( float newAirborneJumpSpeed )
        {
            jumpSpeed = newAirborneJumpSpeed;
        }

        public void SetHorizontalMovement(float newHorizontalMovement)
        {
            m_MoveVector.x = newHorizontalMovement;
        }

        public void SetVerticalMovement(float newVerticalMovement)
        {
            m_MoveVector.y = newVerticalMovement;
        }

        public void IncrementMovement(Vector2 additionalMovement)
        {
            m_MoveVector += additionalMovement;
        }

        public void IncrementHorizontalMovement(float additionalHorizontalMovement)
        {
            m_MoveVector.x += additionalHorizontalMovement;
        }

        public void IncrementVerticalMovement(float additionalVerticalMovement)
        {
            m_MoveVector.y += additionalVerticalMovement;
        }

        public void GroundedVerticalMovement()
        {
            m_MoveVector.y -= gravity * Time.deltaTime;

            if (m_MoveVector.y < -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier)
            {
                m_MoveVector.y = -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
            }
        }

        public Vector2 GetMoveVector()
        {
            return m_MoveVector;
        }

        public bool IsFalling()
        {
            return m_MoveVector.y < 0f;
        }

        public void UpdateFacing()
        {
            //bool faceLeft = PlayerInput.Instance.Horizontal.Value < 0f;
            //bool faceRight = PlayerInput.Instance.Horizontal.Value > 0f;
        }

        public void UpdateFacing(bool faceLeft)
        {
        }

        public float GetFacing()
        {
            return spriteRenderer.flipX != spriteOriginallyFacesLeft ? -1f : 1f;
        }

        public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f)
        {
            float desiredSpeed = useInput ? PlayerInput.Instance.Horizontal.Value * maxSpeed * speedScale : 0f;
            float acceleration = useInput && PlayerInput.Instance.Horizontal.ReceivingInput ? groundAcceleration : groundDeceleration;
            m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }

        public void FindCurrentSurface()
        {
            Collider2D groundCollider = m_SubmarineController2D.GroundColliders[0];

            if (groundCollider == null)
                groundCollider = m_SubmarineController2D.GroundColliders[1];

            if (groundCollider == null)
                return;

            TileBase b = PhysicsHelper.FindTileForOverride(groundCollider, transform.position, Vector2.down);
            if (b != null)
            {
                m_CurrentSurface = b;
            }
        }

        public void CheckForPushing()
        {
            bool pushableOnCorrectSide = false;
            Pushable previousPushable = m_CurrentPushable;

            m_CurrentPushable = null;

            if (m_CurrentPushables.Count > 0)
            {
                bool movingRight = PlayerInput.Instance.Horizontal.Value > float.Epsilon;
                bool movingLeft = PlayerInput.Instance.Horizontal.Value < -float.Epsilon;

                for (int i = 0; i < m_CurrentPushables.Count; i++)
                {
                    float pushablePosX = m_CurrentPushables[i].pushablePosition.position.x;
                    float playerPosX = m_Transform.position.x;
                    if (pushablePosX < playerPosX && movingLeft || pushablePosX > playerPosX && movingRight)
                    {
                        pushableOnCorrectSide = true;
                        m_CurrentPushable = m_CurrentPushables[i];
                        break;
                    }
                }

                if (pushableOnCorrectSide)
                {
                    Vector2 moveToPosition = movingRight ? m_CurrentPushable.playerPushingRightPosition.position : m_CurrentPushable.playerPushingLeftPosition.position;
                    moveToPosition.y = m_SubmarineController2D.Rigidbody2D.position.y;
                    m_SubmarineController2D.Teleport(moveToPosition);
                }
            }

            if(previousPushable != null && m_CurrentPushable != previousPushable)
            {//we changed pushable (or don't have one anymore), stop the old one sound
                previousPushable.EndPushing();
            }
        }

        public void MovePushable()
        {
            //we don't push ungrounded pushable, avoid pushing floating pushable or falling pushable.
            if (m_CurrentPushable && m_CurrentPushable.Grounded)
                m_CurrentPushable.Move(m_MoveVector * Time.deltaTime);
        }

        public void StartPushing()
        {
            if (m_CurrentPushable)
                m_CurrentPushable.StartPushing();
        }

        public void StopPushing()
        {
            if(m_CurrentPushable)
                m_CurrentPushable.EndPushing();
        }

        public void UpdateJump()
        {
            if (!PlayerInput.Instance.Jump.Held && m_MoveVector.y > 0.0f)
            {
                m_MoveVector.y -= jumpAbortSpeedReduction * Time.deltaTime;
            }
        }

        public void AirborneHorizontalMovement()
        {
            float desiredSpeed = PlayerInput.Instance.Horizontal.Value * maxSpeed;

            float acceleration;

            if (PlayerInput.Instance.Horizontal.ReceivingInput)
                acceleration = groundAcceleration * airborneAccelProportion;
            else
                acceleration = groundDeceleration * airborneDecelProportion;

            m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }

        public void AirborneVerticalMovement()
        {
            // Previous movement logic
            //if ( Mathf.Approximately( m_MoveVector.y, 0f ) )
            //{
            //    m_MoveVector.y = 0f;
            //}
            //m_MoveVector.y -= gravity * Time.deltaTime;

            float desiredSpeed = PlayerInput.Instance.Vertical.Value * maxSpeed;

            float acceleration;

            if ( PlayerInput.Instance.Vertical.ReceivingInput )
                acceleration = groundAcceleration * airborneAccelProportion;
            else
                acceleration = groundDeceleration * airborneDecelProportion;

            m_MoveVector.y = Mathf.MoveTowards( m_MoveVector.y, desiredSpeed, acceleration * Time.deltaTime );
        }

        public bool CheckForJumpInput()
        {
            return PlayerInput.Instance.Jump.Down;
        }

        public bool CheckForFallInput()
        {
            return PlayerInput.Instance.Vertical.Value < -float.Epsilon && PlayerInput.Instance.Jump.Down;
        }

        IEnumerator FallThroughtInvincibility()
        {
            damageable.EnableInvulnerability(true);
            yield return new WaitForSeconds(0.5f);
            damageable.DisableInvulnerability();
        }
        
        public void EnableInvulnerability()
        {
            damageable.EnableInvulnerability();
        }

        public void DisableInvulnerability()
        {
            damageable.DisableInvulnerability();
        }

        public Vector2 GetHurtDirection()
        {
            Vector2 damageDirection = damageable.GetDamageDirection();

            if (damageDirection.y < 0f)
                return new Vector2(Mathf.Sign(damageDirection.x), 0f);

            float y = Mathf.Abs(damageDirection.x) * m_TanHurtJumpAngle;

            return new Vector2(damageDirection.x, y).normalized;
        }

        public void OnHurt(Damager damager, Damageable damageable)
        {
            //if the player don't have control, we shouldn't be able to be hurt as this wouldn't be fair
            if (!PlayerInput.Instance.HaveControl)
                return;

            UpdateFacing(damageable.GetDamageDirection().x > 0f);
            damageable.EnableInvulnerability();

            hurtAudioPlayer.PlayRandomSound();

            //if the health is < 0, mean die callback will take care of respawn
            if(damager.forceRespawn && damageable.CurrentHealth > 0)
            {
                StartCoroutine(DieRespawnCoroutine(false, true));
            }
        }

        public void OnDie()
        {
            StartCoroutine(DieRespawnCoroutine(true, false));
        }

        IEnumerator DieRespawnCoroutine(bool resetHealth, bool useCheckPoint)
        {
            PlayerInput.Instance.ReleaseControl(true);
            yield return new WaitForSeconds(1.0f); //wait one second before respawing
            yield return StartCoroutine(ScreenFader.FadeSceneOut(useCheckPoint ? ScreenFader.FadeType.Black : ScreenFader.FadeType.GameOver));
            if(!useCheckPoint)
                yield return new WaitForSeconds (2f);
            Respawn(resetHealth, useCheckPoint);
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(ScreenFader.FadeSceneIn());
            PlayerInput.Instance.GainControl();
        }

        public bool CheckForMeleeAttackInput()
        {
            return PlayerInput.Instance.MeleeAttack.Down;
        }

        public void EnableMeleeAttack()
        {
            meleeDamager.EnableDamage();
            meleeDamager.disableDamageAfterHit = true;
            meleeAttackAudioPlayer.PlayRandomSound();
        }

        public void DisableMeleeAttack()
        {
            meleeDamager.DisableDamage();
        }

        public void TeleportToColliderBottom()
        {
            Vector2 colliderBottom = m_SubmarineController2D.Rigidbody2D.position + m_Capsule.offset + Vector2.down * m_Capsule.size.y * 0.5f;
            m_SubmarineController2D.Teleport(colliderBottom);
        }

        public void PlayFootstep()
        {
            footstepAudioPlayer.PlayRandomSound(m_CurrentSurface);
            var footstepPosition = transform.position;
            footstepPosition.z -= 1;
            VFXController.Instance.Trigger("DustPuff", footstepPosition, 0, false, null, m_CurrentSurface);
        }

        public void Respawn(bool resetHealth, bool useCheckpoint)
        {
            if (resetHealth)
                damageable.SetHealth(damageable.startingHealth);

            if (useCheckpoint && m_LastCheckpoint != null)
            {
                UpdateFacing(m_LastCheckpoint.respawnFacingLeft);
                GameObjectTeleporter.Teleport(gameObject, m_LastCheckpoint.transform.position);
            }
            else
            {
                UpdateFacing(m_StartingFacingLeft);
                GameObjectTeleporter.Teleport(gameObject, m_StartingPosition);
            }
        }

        public void SetChekpoint(Checkpoint checkpoint)
        {
            m_LastCheckpoint = checkpoint;
        }

        //This is called by the inventory controller on key grab, so it can update the Key UI.
        public void KeyInventoryEvent()
        {
            if (KeyUI.Instance != null) KeyUI.Instance.ChangeKeyUI(m_InventoryController);
        }
    }
}
