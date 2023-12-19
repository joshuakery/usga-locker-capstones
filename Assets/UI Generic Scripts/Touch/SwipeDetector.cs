using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

//https://stackoverflow.com/questions/41491765/detect-swipe-gesture-direction
namespace JoshKery.TouchModule
{
    public class SwipeDetector : MonoBehaviour
    {
        public enum Swipe
        {
            NoSwipe = 0,
            Up = 1,
            Down = 2,
            Left = 3,
            Right = 4
        }

        private Vector2 fingerStart;
        private Vector2 fingerDown;
        private Vector2 fingerUp;

        public bool detectSwipeOnlyAfterRelease = false;

        public float SWIPE_THRESHOLD = 20f;

        public RectTransform swipeUpBounds;
        public RectTransform swipeDownBounds;
        public RectTransform swipeLeftBounds;
        public RectTransform swipeRightBounds;

        public RectTransform[] noPanRTs;
        private Canvas[] noPanCanvases;
        private bool canSwipe = false;

        public float timeout = 60;
        private DateTime fingerDownTime;
        private DateTime fingerUpTime;

        public delegate void SwipeUp();
        public SwipeUp swipeUp;

        public delegate void SwipeDown();
        public SwipeDown swipeDown;

        public delegate void SwipeRight();
        public SwipeRight swipeRight;

        public delegate void SwipeLeft();
        public SwipeLeft swipeLeft;

        public UnityEvent onSwipeUp;
        public UnityEvent onSwipeDown;
        public UnityEvent onSwipeRight;
        public UnityEvent onSwipeLeft;

        private void Awake()
        {
            UpdateNoPanRTs();
        }

        public void UpdateNoPanRTs()
        {
            noPanRTs = gameObject.GetComponentsInChildrenWithTag<RectTransform>("No Pan");

            List<Canvas> canvases = new List<Canvas>();

            if (noPanRTs != null)
                foreach (RectTransform rt in noPanRTs)
                {
                    Canvas canvas = rt.gameObject.GetComponent<Canvas>();
                    canvases.Add(canvas);
                }

            noPanCanvases = canvases.ToArray();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                fingerStart = Input.mousePosition;
                fingerDownTime = DateTime.Now;

                fingerDown = Input.mousePosition;
                fingerUp = Input.mousePosition;

                canSwipe = !OverVisibleNoPanRTs();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                fingerUpTime = DateTime.Now;

                fingerDown = Input.mousePosition;
                CheckSwipe();
            }
            else if (Input.GetMouseButton(0))
            {
                fingerDown = Input.mousePosition;

                if (!detectSwipeOnlyAfterRelease)
                {
                    CheckSwipe();
                }
            }

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerStart = touch.position;
                    fingerDownTime = DateTime.Now;

                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    fingerDown = touch.position;

                    if (!detectSwipeOnlyAfterRelease)
                    {
                        CheckSwipe();
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerUpTime = DateTime.Now;

                    fingerDown = touch.position;
                    CheckSwipe();
                }
            }
        }

        public Swipe GetSwipe()
        {
            //Check if Vertical swipe
            if (VerticalMove() > SWIPE_THRESHOLD && VerticalMove() >= HorizontalValMove())
            {
                //Debug.Log("Vertical");
                if (fingerDown.y - fingerUp.y > 0)//up swipe
                {
                    if (InBounds(swipeUpBounds) && canSwipe && InTimeout())
                        return Swipe.Up;
                }
                else if (fingerDown.y - fingerUp.y < 0)//Down swipe
                {
                    if (InBounds(swipeDownBounds) && canSwipe && InTimeout())
                        return Swipe.Down;
                }
            }

            //Check if Horizontal swipe
            else if (HorizontalValMove() > SWIPE_THRESHOLD && HorizontalValMove() > VerticalMove())
            {
                //Debug.Log("Horizontal");
                if (fingerDown.x - fingerUp.x > 0)//Right swipe
                {
                    if (InBounds(swipeRightBounds) && canSwipe && InTimeout())
                        return Swipe.Right;
                }
                else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                {
                    if (InBounds(swipeLeftBounds) && canSwipe && InTimeout())
                        return Swipe.Left;
                }
            }

            //No Movement at-all
            return Swipe.NoSwipe;
        }

        private void CheckSwipe()
        {
            Swipe swipe = GetSwipe();

            switch (swipe)
            {
                case Swipe.Up:
                    OnSwipeUp();
                    fingerUp = fingerDown;
                    break;
                case Swipe.Down:
                    OnSwipeDown();
                    fingerUp = fingerDown;
                    break;
                case Swipe.Left:
                    OnSwipeLeft();
                    fingerUp = fingerDown;
                    break;
                case Swipe.Right:
                    OnSwipeRight();
                    fingerUp = fingerDown;
                    break;
                default:
                    break;
            }
        }

        float VerticalMove()
        {
            return Mathf.Abs(fingerDown.y - fingerUp.y);
        }

        float HorizontalValMove()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }

        private bool InBounds(RectTransform rt)
        {
            if (rt == null)
                return true;

            Vector2 localStartPosition = rt.InverseTransformPoint(fingerStart);
            if (rt.rect.Contains(localStartPosition))
                return true;
            else
                return false;
        }

        private bool InTimeout()
        {
            float duration = (float)fingerUpTime.Subtract(fingerDownTime).TotalSeconds;
            return (duration < timeout);
        }

        private bool OverVisibleNoPanRTs()
        {
            if (noPanRTs != null)
                for (int i = 0; i < noPanRTs.Length; i++)
                {
                    RectTransform rt = noPanRTs[i];
                    if (rt == null) { continue; }

                    Canvas canvas = noPanCanvases[i];
                    bool invisible = (canvas != null && !canvas.enabled);

                    if (!invisible && InBounds(rt))
                        return true;
                }

            return false;
        }

        //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
        void OnSwipeUp()
        {
            Debug.Log("Swipe UP");
            if (swipeUp != null)
                swipeUp();

            if (onSwipeUp != null)
                onSwipeUp.Invoke();
        }

        void OnSwipeDown()
        {
            Debug.Log("Swipe Down");
            if (swipeDown != null)
                swipeDown();

            if (onSwipeDown != null)
                onSwipeDown.Invoke();
        }

        void OnSwipeLeft()
        {
            Debug.Log("Swipe Left");
            if (swipeLeft != null)
                swipeLeft();

            if (onSwipeLeft != null)
                onSwipeLeft.Invoke();
        }

        void OnSwipeRight()
        {
            Debug.Log("Swipe Right");
            if (swipeRight != null)
                swipeRight();

            if (onSwipeRight != null)
                onSwipeRight.Invoke();
        }
    }
}


