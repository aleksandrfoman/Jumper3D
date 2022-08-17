using System;
using DG.Tweening;
using Template.Managers;
using UnityEngine;

namespace Template
{
    
    public class PlayerController : MonoCustom
    {
        private Rigidbody rb;
        private LevelLogic level;

        [SerializeField] private float speed;

        private bool isMove = true;

        private float percet;
        public float Percet => percet;

        [SerializeField] private Transform hand;
        [SerializeField] private Transform lastPoint;
        [SerializeField] private Transform stick;
        [SerializeField] private float minY;
        [SerializeField] private float angleHandSpeed;
        [SerializeField] private float stickHeight;
        [SerializeField] private AnimationCurve curveX;
        [SerializeField] private AnimationCurve curveY;
        [SerializeField] private float stickModifySpeed;
        [SerializeField] private float force;

        private bool isStoped;
        public bool IsStoped => isStoped;

        private float angle;

        private float height;

        private bool isThrowed;


        [ReadOnly][SerializeField] private Vector3 pos;

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
                            ChangeState(false);
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
                            height += stickHeight * Time.deltaTime * stickModifySpeed;
                        }
                    }
                    //else
                    //{
                    //    ThrowPlayer(level);
                    //}

                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        ThrowPlayer(level);
                    }
                    transform.DOMove(stick.transform.position + (Vector3.up * height), 0.5f).SetEase(Ease.Linear);
                }
               
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if(level.GamePhase == GamePhase.Game)
            {
                if (isMove)
                {
                    transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
                }
            }
        }

        public void ThrowPlayer(LevelLogic level)
        {
            isThrowed = true;
            percet = height / stickHeight;
            var force3D = new Vector3(curveX.Evaluate(percet), curveY.Evaluate(percet), 0) * force;
            rb.AddForce(force3D, ForceMode.Impulse);
        }

        public void ChangeState(bool state)
        {
            isMove = state;
        }

        public void OnChangePhase(GamePhase phase)
        {
            rb.isKinematic = phase != GamePhase.Game;
        }
    }
}
