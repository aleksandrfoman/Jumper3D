using System;
using Template.Managers;
using Template.Tweaks;
using UnityEngine;

namespace Template
{
    public class CameraController : MonoCustom
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private Vector3 offsetStart;
        [SerializeField] private Vector3 offsetTap;
        private Vector3 offset;
        [SerializeField] private float speed;
        private Shaker shaker;
        private LevelLogic level;
        private PlayerController player;
        
        public void Init(PlayerController player, LevelLogic logic)
        {
            level = logic;
            this.player = player;
            shaker = new Shaker(camera.transform);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            var target = player.transform.position + offset;

            transform.position = Vector3.Lerp(transform.position,target, speed * Time.fixedDeltaTime);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (player.IsStoped)
            {
                Debug.Log("TapCam");
                offset = offsetTap;
            }
            else
            {
                Debug.Log("StartCam");
                offset = offsetStart;
            }

            //if (level.GamePhase == GamePhase.StartWait)
            //{
            //    //transform.LookAt(level.FinishLevel.position);
            //}
            //else if(level.GamePhase == GamePhase.Game)
            //{
            //    //transform.LookAt(player.transform.position);
            //    transform.localEulerAngles = Vector3.zero;
            //}
            
        }
    }
}
