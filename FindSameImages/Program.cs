using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace FindSameImages
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> images = new List<string>();
            Dictionary<string, string> imageHashes = new Dictionary<string, string>();

            GetImages(images);

            HashImages(images, imageHashes);

            ContainsSameImages(imageHashes);

            Console.WriteLine("\n");
        }

        static void GetImages(List<string> images)
        {
            List<string> folders = new List<string>();

            string path = "";

            while (!Directory.Exists(path))
            {
                Console.WriteLine("Введите путь до корневой папки");
                path = Console.ReadLine();

            }

            folders.Add(path);
            folders = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();

            for (int i = 0; i < folders.Count; i++)
            {
                foreach (var file in Directory.GetFiles(folders[i]))
                {
                    images.Add(file);
                }
            }

        }

        static void HashImages(List<string> images, Dictionary<string, string> imageHashes)
        {
            foreach (string image in images)
            {
                using var md5 = MD5.Create();

                using var stream = File.OpenRead(image);

                imageHashes.Add(image, BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant());
            }
        }

        static void ContainsSameImages(Dictionary<string, string> imageHashes)
        {
            imageHashes.GroupBy(x => x.Value).Where(x => x.Count() > 1);
            var lookup = imageHashes.ToLookup(x => x.Value, x => x.Key).Where(x => x.Count() > 1);

            foreach (var item in lookup)
            {
                var keys = item.Aggregate("", (s, v) => s + "\n" + v);
                var message = $"Одинаковые картинки: {keys}";
                Console.WriteLine(message);
            }
        }
    }
}
