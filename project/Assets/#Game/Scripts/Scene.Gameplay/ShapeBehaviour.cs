using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;


namespace Scene.Gameplay
{
    public class ShapeBehaviour : BaseMonoBehaviour
    {
        // Variables
        private Quaternion fixedRotation { set; get; }
        private Vector3 nextPosition { set; get; }
        private Vector3 cameraOldPos { set; get; }
        private MeshCollider playfield { set; get; }
        private new Camera camera { set; get; }
        private float speed = 3;

        // Machine States
        

        public void Initialize(MeshCollider mCollider)
        {
            playfield = mCollider;

            fixedRotation = transform.rotation;

            nextPosition = transform.position;

            transform.SetParent(Camera.main.transform);

            //print("Name of Mesh COllider = " + mCollider.name);
        }

        override protected void Awake()
        {
            camera = Camera.main;

            cameraOldPos = camera.transform.position;
        }

        protected override void Update()
        {
            //FollowCameraAxis();

            FollowCamera();

            if (playfield.bounds.Contains(new Vector3(nextPosition.x, playfield.transform.position.y, nextPosition.z)))
                transform.position = Vector3.Lerp(transform.position, nextPosition, 0.08f);
            else
                transform.position = Vector3.Lerp(transform.position, new Vector3(playfield.bounds.center.x, transform.position.y, playfield.bounds.center.z), 0.08f);

        }

        void FollowCameraAxis()
        {
            if (Vector3.Distance(cameraOldPos, camera.transform.position) > speed)
            {
                nextPosition = cameraOldPos - camera.transform.position;

                nextPosition = new Vector3(transform.position.x - (nextPosition.x * speed), transform.position.y, transform.position.z - (nextPosition.z * speed));

                cameraOldPos = camera.transform.position;
            }
        }

        void FollowCamera()
        {
            transform.rotation = fixedRotation;

            nextPosition = new Vector3(transform.position.x, nextPosition.y, transform.position.z);
        }

        void OnDisable()
        {
            transform.SetParent(playfield.transform.parent);
        }
    }
}