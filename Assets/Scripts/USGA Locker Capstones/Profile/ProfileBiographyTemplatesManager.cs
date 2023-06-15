using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileBiographyTemplatesManager : MonoBehaviour
    {
        [SerializeField]
        List<ProfileBiographyFieldsManager> templates;

        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                int chosenTemplate = 0;

                if (templates != null)
                {
                    chosenTemplate = System.Math.Clamp(chosenTemplate, 0, templates.Count - 1);

                    for (int i = 0; i < templates.Count; i++)
                    {
                        ProfileBiographyFieldsManager template = templates[i];
                        template.gameObject.SetActive(i == chosenTemplate);
                        if (i == chosenTemplate)
                        {
                            template.SetContent(lockerProfile);
                        }
                    }
                }
            }

            
        }
    }
}

