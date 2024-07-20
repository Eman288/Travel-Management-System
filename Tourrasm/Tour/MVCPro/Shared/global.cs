using MVCPro.Models;

namespace MVCPro.Shared
{
    public static class global
    {
        public static string currentUser { get; set; } = null;
        public static string currentType { get; set; } = null;
        public static int currentTrip { get; set; } = 0;
        public static int i { get; set; } = 0;
        public static string currentTouristAttraction { get; set; } = null;
        public static Staff s { get; set; } = null;
        public static User u { get; set; } = null;

        public static bool checkIndex = true;

        public static bool isIndex = true;

        public static bool ispic = true;

    }
}
