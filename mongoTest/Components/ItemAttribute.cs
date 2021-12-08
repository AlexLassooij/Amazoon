using System;
namespace mongoTest.Components
{
    public class ItemAttribute
    {
        public static int[] peanutbutter = new int[2] { 200, 5 };
        public static int[] shirt = new int[2] { 100, 3 };
        public static int[] pot = new int[2] { 300, 30 };
        public static int[] salad = new int[2] { 30, 5 };
        public static int[] phonecase = new int[2] { 30, 2 };
        public static int[] bottle = new int[2] { 150, 10 };
        public static int[] speaker = new int[2] { 250, 8 };
        


        public static int[] GetItemAttributes(string itemName)
        {
            switch (itemName)
            {
                /* these are dynamic based on the the feed */
                case "peanutbutter": return peanutbutter;
                case "shirt": return shirt;
                case "pot": return pot;
                case "salad": return salad;
                case "phonecase": return phonecase;
                case "bottle": return bottle;
                case "speaker": return speaker;

                /* fixed default */
                default: throw new ArgumentException("propertyName");
            }
        }
    }  
}
