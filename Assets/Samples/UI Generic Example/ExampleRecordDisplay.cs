using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AirtableUnity.PX.Model;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.GenericUI.Example
{
    public class ExampleRecordDisplay : BaseWindow
    {
        [SerializeField]
        private ExampleMediaState mediaState;

        [SerializeField]
        private TMP_Text nameField;

        [SerializeField]
        private TMP_Text yearField;

        [SerializeField]
        private RawImage carouselBackgroundImage;

        public void SetContent(BaseRecord<ExampleFields> record)
        {
            SetTextContent(record);
            SetImageContent(record);
        }

        public void RefreshContent(BaseRecord<ExampleFields> record, ContentLoader.RefreshLimit refreshLimit = ContentLoader.RefreshLimit.AllContent)
        {
            switch(refreshLimit)
            {
                case (ContentLoader.RefreshLimit.AllContent):
                    SetTextContent(record);
                    SetImageContent(record);
                    break;
                case (ContentLoader.RefreshLimit.TextOnly):
                    SetTextContent(record);
                    break;
                case (ContentLoader.RefreshLimit.ImagesOnly):
                    SetImageContent(record);
                    break;
                case (ContentLoader.RefreshLimit.VideoOnly):
                    break;
            }
        }

        private void SetTextContent(BaseRecord<ExampleFields> record)
        {
            if (nameField != null)
                nameField.text = record.fields.Name;

            if (yearField != null)
                yearField.text = record.fields.Year.ToString();
        }

        private void SetImageContent(BaseRecord<ExampleFields> record)
        {
            if (mediaState != null)
            {
                if (carouselBackgroundImage != null)
                {
                    string mediaPath = record.fields.CarouselBackgroundMediaPath;
                    Texture2D tex = mediaState.GetMediaTexture(mediaPath);

                    carouselBackgroundImage.texture = tex;
                    carouselBackgroundImage.GetComponent<LayoutElement>()?.SizeToWidth(tex);
                }

            }

        }
    }
}


