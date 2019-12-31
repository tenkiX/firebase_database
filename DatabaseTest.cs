using System;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseTest : MonoBehaviour
{
    [SerializeField] FirebaseDatabaseManager databaseManager;

    #region POST (add new to database)
    public void UploadLevelButtonClicked()
    {
        Level level = new Level("name", new int[] { 0, 1, 2 }, "1.0", UnityEngine.Random.Range(0, 100), 5);
        databaseManager.UploadLevelAsync(level, OnUploadLevelSuccess, OnUploadLevelFailed);
    }

    void OnUploadLevelSuccess()
    {
        Debug.Log("Upload success");
    }

    void OnUploadLevelFailed(AggregateException exception)
    {
        Debug.LogError(exception.ToString());
    }
    #endregion


    #region GET (read from database)
    /// <summary> GET all levels </summary>
    public void OnGetLevelButtonClicked()
    {
        databaseManager.GetLevelsAsync(OnGetLevelSuccess, OnGetLevelFailed);
    }

    void OnGetLevelSuccess(List<Level> result)
    {
        if (result.Count == 0)
        {
            Debug.Log("Call succeed, but no results found in the database!");
        }
        else
        {
            foreach (var level in result)
            {
                Debug.Log("Successfully retrieved level (dbKey): " + level.DatabaseKey);
            }
        }
    }

    void OnGetLevelFailed(AggregateException exception)
    {
        Debug.LogError(exception.ToString());
    }
    #endregion

 }
