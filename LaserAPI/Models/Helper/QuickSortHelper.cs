namespace LaserAPI.Models.Helper
{
    public static class QuickSortHelper
    {
        public static void QuickSort(int[] arr) => QuickSort(arr, 0, arr[^1]);
        public static void QuickSort(double[] arr) => QuickSort(arr, 0, arr.Length - 1);

        private static void QuickSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                {
                    QuickSort(arr, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    QuickSort(arr, pivot + 1, right);
                }
            }

        }

        private static void QuickSort(double[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                {
                    QuickSort(arr, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    QuickSort(arr, pivot + 1, right);
                }
            }

        }

        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot)
                {
                    left++;
                }

                while (arr[right] > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;
                    (arr[left], arr[right]) = (arr[right], arr[left]);
                }
                else
                {
                    return right;
                }
            }
        }

        private static int Partition(double[] arr, int left, int right)
        {
            double pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot)
                {
                    left++;
                }

                while (arr[right] > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;
                    (arr[left], arr[right]) = (arr[right], arr[left]);
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
