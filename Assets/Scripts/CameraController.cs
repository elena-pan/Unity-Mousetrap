using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace MouseTrap
{
    public class CameraController : MonoBehaviour {
    
        public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        private RotationAxes axes = RotationAxes.MouseXAndY;
        private float sensitivityRotate = 5F;
        private float sensitivityTranslate = 0.5F;
        //private float minimumX = -360F;
        //private float maximumX = 360F;
        private float minimumY = -90F;
        private float maximumY = 90F;
        float rotationY = -90F;

        private float lerpSpeed = 7f;

        public static bool viewContraption = false;
        public static bool viewDiceRoll = false;
        public static bool isDragging = false;

        void Update()
        {
            if (viewContraption) {
                LerpContraption();
            }
            else if (viewDiceRoll) {
                LerpDiceRoll();
            }
            else if (isDragging == false)
            {
                MouseInput();
                KeyboardInput();
            }
        }

        void LerpContraption()
        {
            CameraFollow.isFollowing = false;
            viewDiceRoll = false;
            
            Vector3 viewContraptionPos = new Vector3(10, 15, -15);
            Quaternion rotation = Quaternion.Euler(30,-30,0);
            this.transform.position = Vector3.Lerp(this.transform.position, viewContraptionPos, lerpSpeed*Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, lerpSpeed*Time.deltaTime);

            // Stop following once we reach the position
            if (this.transform.position == viewContraptionPos) {
                viewContraption = false;
            }
        }

        void LerpDiceRoll()
        {
            CameraFollow.isFollowing = false;
            viewContraption = false;
            
            Vector3 viewDicePos = new Vector3(0, 15, 4);
            Quaternion rotation = Quaternion.Euler(90,0,0);
            this.transform.position = Vector3.Lerp(this.transform.position, viewDicePos, lerpSpeed*Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, lerpSpeed*Time.deltaTime);

            // Stop following once we reach the position
            if (this.transform.position == viewDicePos) {
                viewDiceRoll = false;
            }
        }

        void KeyboardInput()
        {
            if(Input.GetKey(KeyCode.DownArrow)) 
            {
                Vector3 pos = transform.position;
                pos = pos - transform.forward*sensitivityTranslate;
                transform.position = pos;
            }
            else if(Input.GetKey(KeyCode.UpArrow)) 
            {
                Vector3 pos = transform.position;
                pos = pos + transform.forward*sensitivityTranslate;
                transform.position = pos;
            }
            else if(Input.GetKey(KeyCode.LeftArrow)) 
            {
                Vector3 pos = transform.position;
                pos = pos - transform.right*sensitivityTranslate;
                transform.position = pos;
            }
            else if(Input.GetKey(KeyCode.RightArrow)) 
            {
                Vector3 pos = transform.position;
                pos = pos + transform.right*sensitivityTranslate;
                transform.position = pos;
            }
        }

        void MouseInput()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                MouseLeftClick();
            }
            else if (Input.GetMouseButton(1))
            {
                //MouseRightClick();
            }
            else
            {
                MouseWheeling();
            }
        }

        void MouseLeftClick()
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityRotate;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityRotate;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityRotate, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityRotate;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }

        void MouseWheeling()
        {
            Vector3 pos = transform.position;
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                pos = pos - transform.forward*sensitivityRotate;
                transform.position = pos;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                pos = pos + transform.forward*sensitivityRotate;
                transform.position = pos;
            }
        }
    }
}