using System;
using System.Collections;
using System.Collections.Generic;
using Script.Mover;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PathManager> listPathManagers;
    [SerializeField] private BallItem ballPrefab;
    
    private int amountColor;
   
    
     // private List<PathData> listPathPoints;
    private int drop = 1; // коефіцієнт поділу , впливає на точність розрахунків (чим більша цифра тим більше буде точок для переміщення сфер)

    private int amountPathResult;
    private int amountBalls;

   // private Dictionary<COLORTYPE, int> dictionaryColorAmount = new Dictionary<COLORTYPE, int>();
   private List<ColorType> colorList = new List<ColorType>();
       
    
    void Start()
    {
        amountColor = listPathManagers.Count;

        foreach (PathManager pathManager in listPathManagers)
        {
            pathManager.IsPath += OnIsPath;
            
            pathManager.SetBall(ballPrefab);
            pathManager.SetDrop(drop);
            pathManager.StartCalculatePath();
            
            
        }
        
        
    }

    private void OnIsPath(int  pointsInPath, ColorType color)
    {
        amountPathResult++;
        amountBalls += pointsInPath;
        for (int i = 0; i < pointsInPath; i++)
        {
           colorList.Add(color);
        }
       
        if (amountPathResult == listPathManagers.Count)
        {
            CreateBalls();
        }
       
    }

    private void CreateBalls()
    {
        int currentSpheraAmount =0;
        int id = 0;
        for (int j = 0; j < listPathManagers.Count; j++)
        {
            PathManager pathManager = listPathManagers[j];
            List<BallData> randomColorsForPathManager = new List<BallData>();
            
                Random rand = new Random();
               
                do
                {
                    int randomIndex= rand.Next(0, colorList.Count);
                    randomColorsForPathManager.Add(new BallData(colorList[randomIndex], id));
                    colorList.RemoveAt(randomIndex);
                    id++;


                } while (randomColorsForPathManager.Count!=pathManager.GetAmountPointsInPath());
            

            pathManager.SetBallsDataAndCreateBalls(randomColorsForPathManager);
        }
       
    }


    
}

public class BallData
{
    private ColorType colorType;
    public ColorType ColorType => colorType;

    private int id;
    public int Id => id;

    public BallData(ColorType colorType, int id)
    {
        this.colorType = colorType;
        this.id = id;
    }

}
