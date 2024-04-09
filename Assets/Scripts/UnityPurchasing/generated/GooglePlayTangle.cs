// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("JZL8mJXbA3kmog1OGvs597TkOslF+8VJBmXpUfO+gZ8nxo5fe8aJ4NE3oBmAzuneN2ZgWXTv7yLBkOFgZNZVdmRZUl1+0hzSo1lVVVVRVFenrF00Lyo4CPd9CMlS3oaS+uLf4mBLiSZqq1YGIhLc/K8laEVGUEug5XMy1Wecdq6knZsxQ1DRTd9efKPWVVtUZNZVXlbWVVVUx5CyGvz609ZcQAp38luqqXrpZ5AScJrTahrNHxLvAGdycPFmX6A61RTAwNt5oYXgEuG7N/Gks047S8XSssyN0udMLxT/aNH52jlZRlHtqsxgQQ6nke4Dt+Z3zDkRJxTKA0nGhC9hvR65TDCGtDHixADc2roXvuk/Itq+wdn3Gf7ynBBO9AGit1ZXVVRV");
        private static int[] order = new int[] { 12,4,11,12,5,6,8,8,11,12,13,12,13,13,14 };
        private static int key = 84;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
