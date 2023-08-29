using System.Collections.Generic;

namespace JoshKery.USGA.LockerCapstones
{
    public enum ProfileModuleType
    {
        None = -1,
        Biography = 0,
        Accomplishments = 1,
        MediaGallery = 2
    }

    public static class ProfileModulesOrder
    {
        public static ProfileModuleType[] order =
            new ProfileModuleType[]
            {
                ProfileModuleType.Biography,
                ProfileModuleType.Accomplishments,
                ProfileModuleType.MediaGallery
            };

        public static Dictionary<ProfileModuleType, string> moduleInfo =
            new Dictionary<ProfileModuleType, string>()
            {
                { ProfileModuleType.Biography, "Biography" },
                { ProfileModuleType.Accomplishments, "Accomplishments" },
                { ProfileModuleType.MediaGallery, "Media Gallery" }
            };
    }
}


