using UnityEngine;
using GuraGames.Data;
using System.IO;

namespace TGC.MDS
{
    public static class MDSSaveSystem
    {
        public static string EXTENSION = ".mds";
        public static string APPLICATION_PATH = $"{Application.persistentDataPath}/MetaDataSource";

        public static void Save(CurrentStateData data, string file_name)
        {
            var json = JsonUtility.ToJson(data);
            var exist = IsSaveDataAvailable(file_name, out string full_path);

            if (exist)
            {
                File.WriteAllText(full_path, json);
            } else
            {
                Directory.CreateDirectory(APPLICATION_PATH);
                File.WriteAllText(full_path, json);
            }
        }

        public static CurrentStateData Load(string file_name, out bool result)
        {
            result = false;
            CurrentStateData data = new CurrentStateData();

            if (IsSaveDataAvailable(file_name, out string full_path))
            {
                result = true;
                string dataAsJson = File.ReadAllText(full_path);
                data = JsonUtility.FromJson<CurrentStateData>(dataAsJson);
            }

            return data;
        }

        public static bool IsSaveDataAvailable(string file_name, out string full_path)
        {
            full_path = $"{APPLICATION_PATH}/{file_name}{EXTENSION}";
            return File.Exists(full_path);
        }

        public static void DeleteAllSaveData(string file_name)
        {
            if (IsSaveDataAvailable(file_name, out string full_path))
            {
                File.Delete(full_path);
            }
        }
    }
}