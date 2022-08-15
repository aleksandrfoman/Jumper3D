using System;
using Template.Tweaks;
using UnityEngine;

namespace Template
{
    public class CameraController : MonoCustom
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private Vector3 offcet;
        [SerializeField] private float speed;
        private Shaker shaker;

        private PlayerController player;
        
        public void Init(PlayerController player)
        {
            this.player = player;
            shaker = new Shaker(camera.transform);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offcet, speed * Time.deltaTime);
        }
        
    }
}
