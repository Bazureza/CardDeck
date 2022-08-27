using UnityEngine;
using GuraGames.Data;
using System.IO;

namespace TGC.MDS
{
    public static class MDSSaveSystem
    {
        public static string EXTENSION = ".mds";
        public static string APPLICATION_PATH = $"{Application.persistentDataPath}/MetaDataSource/";

        public static void Save(CurrentStateData data, string file_name)
        {
            var json = JsonUtility.ToJson(data);
            var full_path = $"{APPLICATION_PATH}{file_name}{EXTENSION}";

            File.WriteAllText(full_path, json);
        }

        public static CurrentStateData Load(string file_name)
        {
            CurrentStateData data = new CurrentStateData();
            var full_path = $"{APPLICATION_PATH}{file_name}{EXTENSION}";

            if (File.Exists(full_path))
            {
                string dataAsJson = File.ReadAllText(full_path);
                data = JsonUtility.FromJson<CurrentStateData>(dataAsJson);
            }

            return data;
        }
    }
}