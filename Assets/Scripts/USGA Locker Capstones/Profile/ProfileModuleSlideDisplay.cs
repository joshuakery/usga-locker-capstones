using System;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileModuleSlideDisplay : SlideDisplay
    {
        public static float timeout = 1f;

        /// <summary>
        /// How much 'padding' the limit detection has.
        /// Measured in pixels.
        /// Used because the scroll rect doesn't settle its contents to exactly the size of its RectTransform,
        /// but hovers, usually within 1px, of that size.
        /// </summary>
        public static float limitPadding = 2f;

        [SerializeField]
        private float limit = 0f;

        [SerializeField]
        private Vector2 lastScrollRectPosition;

        [SerializeField]
        private bool isMovingThisFrame = false;

        [SerializeField]
        private bool wasStillLastFrame = true;

        private DateTime hittingLimitStart;

        private bool readyToFire = true;

        public float timeDisplay = 0f;

        [SerializeField]
        private ScrollRect scrollRect;

        private RectTransform scrollRectRT;

        /// <summary>
        /// Controlling carousel.
        /// </summary>
        [SerializeField]
        private Carousel carousel;

        #region Monobehaviour Methods
        protected override void Awake()
        {
            base.Awake();

            if (scrollRect != null)
                scrollRectRT = scrollRect.gameObject.GetComponent<RectTransform>();
        }

        private void Start()
        {
            hittingLimitStart = DateTime.Now;
            lastScrollRectPosition = new Vector2(0f, 0f);
        }
        #endregion

        /// <summary>
        /// Detecting two things:
        /// (1) Whether the user has scrolled beyond the viewport limits and held the scrollrect there
        /// for longer than the timeout
        /// or
        /// (2) Whether the user has scrolled beyond the viewport limits and the scrollrect was already
        /// at the limits of the viewport
        /// </summary>
        private void Update()
        {
            if (scrollRectRT != null)
            {
                limit = (scrollRect.content.rect.height - scrollRectRT.rect.height) / 2f;
            }

            isMovingThisFrame = Mathf.Round(lastScrollRectPosition.y) != Mathf.Round(scrollRect.content.anchoredPosition.y);

            //If beyond upper limit
            if (scrollRect.content.anchoredPosition.y <= (-1f * limit) - limitPadding)
            {
                //If this is the first frame beyond the limit, start counting from here
                if (lastScrollRectPosition.y > (-1f * limit) - limitPadding)
                    hittingLimitStart = DateTime.Now;

                //If the last position was at the limit, and we are moving after holding still
                if (lastScrollRectPosition.y < (-1f * limit) + limitPadding &&
                    lastScrollRectPosition.y > (-1f * limit) - limitPadding &&
                    isMovingThisFrame &&
                    wasStillLastFrame
                )
                {
                    if (readyToFire)
                    {
                        OnMoveUp();

                        readyToFire = false;
                    }
                }

                //If timeout
                if (readyToFire && HasEnoughTimePassed())
                {
                    OnMoveUp();

                    readyToFire = false;

                    hittingLimitStart = DateTime.Now;
                }
            }
            //If beyond lower limit
            else if (scrollRect.content.anchoredPosition.y >= limit + limitPadding)
            {
                //If this is the first frame beyond the limit, start counting from here
                if (lastScrollRectPosition.y < limit + limitPadding)
                    hittingLimitStart = DateTime.Now;
                
                //If the last position was the limit, and we are moving after holding still
                if (lastScrollRectPosition.y > limit - limitPadding &&
                    lastScrollRectPosition.y < limit + limitPadding &&
                    wasStillLastFrame &&
                    isMovingThisFrame
                )
                {
                    if (readyToFire)
                    {
                        OnMoveDown();

                        readyToFire = false;
                    }
                }

                //If timeout
                if (readyToFire && HasEnoughTimePassed())
                {
                    OnMoveDown();

                    readyToFire = false;

                    hittingLimitStart = DateTime.Now;
                }
            }
            //Else reset the count
            else
            {
                hittingLimitStart = DateTime.Now;

                readyToFire = true;
            }

            lastScrollRectPosition = (scrollRect.content.anchoredPosition);

            wasStillLastFrame = !isMovingThisFrame;

            timeDisplay = (float)(DateTime.Now - hittingLimitStart).TotalSeconds;
        }

        private bool HasEnoughTimePassed()
        {
            return ((float)(DateTime.Now - hittingLimitStart).TotalSeconds > timeout);
        }

        private void OnMoveUp()
        {
            Debug.Log("up");

            /*if (carousel != null)
                carousel.NextSlide();*/
        }

        private void OnMoveDown()
        {
            Debug.Log("down");

            /*if (carousel != null)
                carousel.PrevSlide();*/
        }
    }
}


