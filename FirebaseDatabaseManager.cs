using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseDatabaseManager : MonoBehaviour
{
    [SerializeField] string databaseUrl;
    DatabaseReference databaseRef;

    void Start()
    {
        if (string.IsNullOrEmpty(databaseUrl))
        {
            Debug.LogError("You forgot to add you database URL in the inspector! Please check it!");
        }
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseUrl);
        // Get the root reference location of the database.
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    #region POST (add new) levels
    /// <summary> Add new the level with a generated key </summary>
    public async Task UploadLevelAsync(Level level, Action OnSuccess, Action<AggregateException> OnError)
    {
        string json = JsonConvert.SerializeObject(level);
        AggregateException exception = null;
        //Push creates a new unique key automatically for our level
        await databaseRef.Child("levels").Push().SetRawJsonValueAsync(json).ContinueWith(
            task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    exception = task.Exception;
                }
            }
            );
        if (exception != null)
        {
            OnError(exception);
        }
        else
        {
            OnSuccess();
        }
    }
    #endregion


    #region GET all contents of a child node
    /// <summary> GET all the levels from the database </summary>
    public async void GetLevelsAsync(Action<List<Level>> OnSuccess, Action<AggregateException> OnError)
    {
        AggregateException exception = null;
        var levels = new List<Level>();
        await databaseRef.Child("levels")
         .GetValueAsync().ContinueWith(task =>
         {
             if (task.IsFaulted || task.IsCanceled)
             {
                 exception = task.Exception;
             }
             else if (task.IsCompleted)
             {
                 DataSnapshot snapshot = task.Result;
                 foreach (var item in snapshot.Children)
                 {
                     Level tempLevel = JsonConvert.DeserializeObject<Level>(item.GetRawJsonValue());
                     tempLevel.DatabaseKey = item.Key;
                     levels.Add(tempLevel);
                 }
             }
         });

        if (exception != null)
        {
            OnError(exception);
        }
        else
        {
            OnSuccess(levels);
        }
    }
    #endregion
          
}
