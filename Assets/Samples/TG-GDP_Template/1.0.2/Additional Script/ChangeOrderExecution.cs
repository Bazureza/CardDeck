using TomGustin.GameDesignPattern;
using UnityEditor;
using UnityEngine;

namespace TomGustin
{
    [InitializeOnLoad]
    public class ChangeOrderExecution
    {
        private static int EXEC_ORDER_SERVICE = -55;
        static ChangeOrderExecution()
        {
            ServiceRegisterChangeOrderExecution();
        }

        static void ServiceRegisterChangeOrderExecution()
        {
            var temp = new GameObject();

            var reader = temp.AddComponent<ServiceRegister>();
            MonoScript readerScript = MonoScript.FromMonoBehaviour(reader);
            if (MonoImporter.GetExecutionOrder(readerScript) != EXEC_ORDER_SERVICE)
            {
                MonoImporter.SetExecutionOrder(readerScript, EXEC_ORDER_SERVICE);
            }
            Object.DestroyImmediate(temp);
        }
    }
}