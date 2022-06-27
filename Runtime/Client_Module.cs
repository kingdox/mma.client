#region Access
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
namespace MMA.Client
{
    public static class Key
    {
        // public const string _   = KeyData._;
        public static string AddAction   = "Client_AddAction";
        public static string RunAction   = "Client_RunAction";
        public static string OnCompleted   = "Client_OnCompleted";
        public static string OnFail   = "Client_OnFail";
        public static string OnProgress   = "Client_OnProgress";
    }
    public static class Import
    {
        //public const string _ = _;
    }
    public sealed partial class Client_Module : Module
    {
        #region References
        //[Header("Applications")]
        //[SerializeField] public ApplicationBase interface_Client;
        private readonly SortedDictionary<int, (string id, IEnumerator action)> dic_sorted = new SortedDictionary<int, (string id, IEnumerator action)>();
        #endregion
        #region Reactions ( On___ )
        // Contenedor de toda las reacciones del Client
        protected override void OnSubscription(bool condition)
        {
            // AddAction
            Middleware<(string id, int priority, IEnumerator action)>.Subscribe_Publish(condition, Key.AddAction, AddAction);

            // RunAction
            Middleware.Subscribe_IEnumerator(condition, Key.RunAction, RunActions);
        }
        #endregion
        #region Methods
        // Contenedor de toda la logica del Client
        private void AddAction((string id, int priority, IEnumerator action) value)
        {
            //Añade
            //list_clientActions.Add(value);
            dic_sorted.Add(value.priority, (value.id, value.action));
            Debug.Log($"Added: {value.id} | Client Actions: {dic_sorted.Count}");
        }

        private void OnProgress((string id, int current, int lenght) value){
            Middleware<(string id, int current, int lenght)>.Invoke_Publish(Key.OnProgress, value);
        }
        #endregion
        #region Request ( Coroutines )
        // Contenedor de toda la Esperas de corutinas del Client
        private IEnumerator RunActions()
        {
            int intCount = 0;
            foreach (KeyValuePair<int, (string id, IEnumerator action)> pair in dic_sorted)
            {
                Debug.Log($"[{pair.Key}]: Start Action => {pair.Value.id}");
                OnProgress((pair.Value.id, intCount, dic_sorted.Count));
                yield return pair.Value.action;
            }
        }
        #endregion
        #region Task ( async )
        // Contenedor de toda la Esperas asincronas del Client
        #endregion
    }


}
