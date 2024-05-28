namespace NamespaceGPT.Common.Modules.Sorting.Module
{
    public static class SortingModule<T>
        where T : IComparable<T>
    {
        public static void BubbleSort(List<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (list[j].CompareTo(list[j + 1]) > 0)
                    {
                        T elem = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = elem;
                    }
                }
            }
        }
        public static void MergeSort(List<T> list)
        {
            MergeSortRecursive(list, 0, list.Count - 1);
        }

        private static void MergeSortRecursive(List<T> list, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                MergeSortRecursive(list, left, middle);
                MergeSortRecursive(list, middle + 1, right);
                Merge(list, left, middle, right);
            }
        }

        private static void Merge(List<T> list, int left, int middle, int right)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;
            List<T> leftList = new List<T>(list.GetRange(left, n1));
            List<T> rightList = new List<T>(list.GetRange(middle + 1, n2));
            int i = 0, j = 0, k = left;
            while (i < n1 && j < n2)
            {
                if (leftList[i].CompareTo(rightList[j]) <= 0)
                {
                    list[k++] = leftList[i++];
                }
                else
                {
                    list[k++] = rightList[j++];
                }
            }
            while (i < n1)
            {
                list[k++] = leftList[i++];
            }
            while (j < n2)
            {
                list[k++] = rightList[j++];
            }
        }
        public static void GnomeSort(List<T> list)
        {
            int index = 0;
            while (index < list.Count)
            {
                if (index == 0)
                {
                    index++;
                }
                if (list[index].CompareTo(list[index - 1]) >= 0)
                {
                    index++;
                }
                else
                {
                    T elem = list[index];
                    list[index] = list[index - 1];
                    list[index - 1] = elem;
                    index--;
                }
            }
        }
    }
}
