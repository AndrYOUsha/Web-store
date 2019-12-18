namespace WebStore.AdditionalLogic
{
    public static class Images
    {
        public static string[] GetImages(string pathToRoot, string pathToPicture)
        {
            string[] array = System.IO.Directory.GetFiles(pathToRoot + pathToPicture);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array[i].TrimStart(pathToRoot.ToCharArray());
            }

            return array;
        }
    }
}
