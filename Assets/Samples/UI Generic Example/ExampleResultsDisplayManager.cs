using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirtableUnity.PX.Model;
using JoshKery.GenericUI.ContentLoading;

namespace JoshKery.GenericUI.Example
{
    public class ExampleResultsDisplayManager : BaseDisplay
    {
        private Dictionary<string, ExampleRecordDisplay> childDisplaysDict;

        public void InstantiateResultsDisplays(List<BaseRecord<ExampleFields>> records)
        {
            foreach (BaseRecord<ExampleFields> record in records)
            {
                InstantiateDisplay(record);
            }
        }

        protected void InstantiateDisplay(BaseRecord<ExampleFields> record)
        {
            ExampleRecordDisplay recordDisplay = InstantiateDisplay<ExampleRecordDisplay>();
            if (recordDisplay != null)
            {
                recordDisplay.SetContent(record);

                if (childDisplaysDict == null)
                    childDisplaysDict = new Dictionary<string, ExampleRecordDisplay>();

                if (childDisplaysDict != null)
                    childDisplaysDict[record.fields.Name] = recordDisplay;
            }
        }

        public void RefreshResultsDisplays(List<BaseRecord<ExampleFields>> records, ContentLoader.RefreshLimit refreshLimit = ContentLoader.RefreshLimit.AllContent)
        {
            foreach (BaseRecord<ExampleFields> record in records)
            {
                RefreshDisplay(record, refreshLimit);
            }
        }

        protected void RefreshDisplay(BaseRecord<ExampleFields> record, ContentLoader.RefreshLimit refreshLimit)
        {
            if (childDisplaysDict != null && childDisplaysDict.ContainsKey(record.fields.Name))
            {
                ExampleRecordDisplay recordDisplay = childDisplaysDict[record.fields.Name];
                recordDisplay.RefreshContent(record, refreshLimit);
            }
        }
    }
}


