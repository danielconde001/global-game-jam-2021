using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
#region Singleton
    private static PuzzleManager _instance;
    public static PuzzleManager Instance
    {
        get 
        {
             if (_instance == null)
             {
                _instance = GameObject.FindObjectOfType<PuzzleManager>();
             
                if (_instance == null)
                  {
                    GameObject container = new GameObject("Puzzle Manager");
                    _instance = container.AddComponent<PuzzleManager>();
                  }
            }

            return _instance;
        }
    }
#endregion

    [SerializeField] uint MaxNumberOfPieces { get; }
    uint TotalNumberOfPiecesAttained { get { return totalNumberOfPiecesAttained; } }
    uint totalNumberOfPiecesAttained = 0;

    private void Awake() 
    {
        _instance = this;
    }

    public void PieceAttained()
    {
        totalNumberOfPiecesAttained++;
    }

    public bool CheckIfAllPiecesCollected()
    {
        return TotalNumberOfPiecesAttained >= MaxNumberOfPieces;
    }
}
