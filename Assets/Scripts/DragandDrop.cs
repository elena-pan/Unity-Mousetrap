using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MouseTrap {
    public class DragandDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Sprite baseA;
        [SerializeField]
        private Sprite gearSupport;
        [SerializeField]
        private Sprite gear3;
        [SerializeField]
        private Sprite crank;
        [SerializeField]
        private Sprite gear5;
        [SerializeField]
        private Sprite stopSign;
        [SerializeField]
        private Sprite lampPost;
        [SerializeField]
        private Sprite shoe;
        [SerializeField]
        private Sprite stairway;
        [SerializeField]
        private Sprite bucket;
        [SerializeField]
        private Sprite baseB;
        [SerializeField]
        private Sprite chute;
        [SerializeField]
        private Sprite pipes;
        [SerializeField]
        private Sprite hand;
        [SerializeField]
        private Sprite thingamajig;
        [SerializeField]
        private Sprite bathtub;
        [SerializeField]
        private Sprite ball2;
        [SerializeField]
        private Sprite divingBoard;
        [SerializeField]
        private Sprite diver;
        [SerializeField]
        private Sprite baseC;
        [SerializeField]
        private Sprite tub;
        [SerializeField]
        private Sprite post;
        [SerializeField]
        private Sprite cage;
        
        private Vector2 lastMousePosition;
        private static Sprite[] images;
        private static Image image;
        private RectTransform rect;
        private Vector3 initialPos;

        void Start()
        {
            image = GetComponent<Image>();
            rect = GetComponent<RectTransform>();
            initialPos = rect.position;
            SetUpImages();
        }
        public static void UpdateImage(int num)
        {
            image.sprite = images[num];
        }

        /// <summary>
        /// This method will be called on the start of the mouse drag
        /// </summary>
        /// <param name="eventData">mouse pointer event data</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            GameManager.instance.popUpWindow.SetActive(false);
            GameManager.instance.buildArrowPanel.SetActive(false);
            //Debug.Log("Begin Drag");
            CameraController.isDragging = true;
            lastMousePosition = eventData.position;
        }

        /// <summary>
        /// This method will be called during the mouse drag
        /// </summary>
        /// <param name="eventData">mouse pointer event data</param>
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - lastMousePosition;

            Vector3 newPosition = rect.position +  new Vector3(diff.x, diff.y, transform.position.z);
            Vector3 oldPos = rect.position;
            rect.position = newPosition;
            if(!IsRectTransformInsideScreen(rect))
            {
                rect.position = oldPos;
            }
            lastMousePosition = currentMousePosition;
        }

        /// <summary>
        /// This method will be called at the end of mouse drag
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("End Drag");
            rect.position = initialPos;
            CameraController.isDragging = false;
            GameManager.instance.build.PlaceBuild();
        }

        /// <summary>
        /// This methods will check is the rect transform is inside the screen or not
        /// </summary>
        /// <param name="rectTransform">Rect Trasform</param>
        /// <returns></returns>
        private bool IsRectTransformInsideScreen(RectTransform rectTransform)
        {
            bool isInside = false;
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            int visibleCorners = 0;
            Rect rect = new Rect(0,0,Screen.width, Screen.height);
            foreach(Vector3 corner in corners)
            {
                if(rect.Contains(corner))
                {
                    visibleCorners++;
                }
            }
            if(visibleCorners == 4)
            {
                isInside = true;
            }
            return isInside;
        }

        private void SetUpImages()
        {
            images = new Sprite[] {baseA, gearSupport, gear3, crank, gear5, stopSign, lampPost, shoe, stairway, bucket, baseB, chute, pipes, hand, thingamajig, bathtub, ball2, divingBoard, diver, baseC, tub, post, cage};
        }
    }
}