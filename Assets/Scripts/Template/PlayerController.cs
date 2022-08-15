using System;
using DG.Tweening;
using Template.Managers;
using UnityEngine;

namespace Template
{
    [System.Serializable]
    public class PlayerRotateHand
    {
        [SerializeField] private Transform hand;
        [SerializeField] private Transform lastPoint;
        [SerializeField] private Transform stick;
        [SerializeField] private float minY;
        [SerializeField] private float angleHandSpeed;
        [SerializeField] private float stickHeight;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private AnimationCurve curveX;
        [SerializeField] private AnimationCurve curveY;

        [SerializeField] private float force;
        private bool isStoped;
        
        private float angle;

        private float height;

        private bool isThrowed;

        [ReadOnly] [SerializeField] private Vector3 pos;
        public void Update(PlayerMover mover, Transform transform)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (!isStoped)
                {
                    angle += angleHandSpeed * Time.deltaTime;
                    hand.localEulerAngles = new Vector3(angle, 0, 0);
                    if (lastPoint.transform.position.y < minY)
                    {
                        pos = stick.position;
                        stick.parent = null;
                        mover.ChangeState(false);
                        isStoped = true;
                    }
                }
            }

            if (isStoped && !isThrowed)
            {
                stick.position = pos;
                stick.DOLocalRotate(Vector3.zero, 0.5f);
                if (height < stickHeight)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        height += stickHeight * Time.deltaTime * 2.5f;
                    }
                    
                }
                else
                {
                    ThrowPlayer();
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    ThrowPlayer();
                }
                transform.DOMove(stick.transform.position + (Vector3.up * height), 0.5f).SetEase(Ease.Linear);
            }
        }

        public void ThrowPlayer()
        {
            isThrowed = true;
            var percet = height / stickHeight;
            var force3D = new Vector3(curveX.Evaluate(percet), curveY.Evaluate(percet), 0) * force;
            rb.AddForce(force3D, ForceMode.Impulse);
        }
    }

    [System.Serializable]
    public class PlayerMover
    {
        [SerializeField] private float speed;

        private bool isMove = true;

        public void ChangeState(bool state)
        {
            isMove = state;
        }

        public void Update(Transform transform)
        {
            if (isMove)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }
    
    public class PlayerController : MonoCustom
    {
        private Rigidbody rb;
        private LevelLogic level;
        
        
        [Separator()]
        [SerializeField] private PlayerRotateHand playerHand;
        [Separator()]
        [SerializeField] private PlayerMover playerMover;
        public void Init(LevelLogic level)
        {
            rb = GetComponent<Rigidbody>();
            this.level = level;
            level.OnChangePhase.AddListener(OnChangePhase);
            OnChangePhase(level.GamePhase);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (level.GamePhase == GamePhase.Game)
            {
                playerMover.Update(transform);
                playerHand.Update(playerMover, transform);
            }
        }

        public void OnChangePhase(GamePhase phase)
        {
            rb.isKinematic = phase != GamePhase.Game;
        }
    }
}
