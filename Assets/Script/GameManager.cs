using System;
using System.Collections;
using System.Collections.Generic;
using Script.Mover;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PathManager> listPathManagers;
    [SerializeField] private SphereItem spheraPrefab;
    
    private int amountColor;
   
    
     // private List<PathData> listPathPoints;
    private int drop = 1; // коефіцієнт поділу , впливає на точність розрахунків (чим більша цифра тим більше буде точок для переміщення сфер)

    private int amountPathResult;
    private int amountSpheres;

   // private Dictionary<COLORTYPE, int> dictionaryColorAmount = new Dictionary<COLORTYPE, int>();
   private List<ColorType> colorList = new List<ColorType>();
       
    
    void Start()
    {
        amountColor = listPathManagers.Count;

        foreach (PathManager pathManager in listPathManagers)
        {
            pathManager.IsPath += OnIsPath;
            
            pathManager.SetSphera(spheraPrefab);
            pathManager.SetDrop(drop);
            pathManager.StartCalculatePath();
            
            
        }
        
        
    }

    private void OnIsPath(int  pointsInPath, ColorType color)
    {
        amountPathResult++;
        amountSpheres += pointsInPath;
        for (int i = 0; i < pointsInPath; i++)
        {
           colorList.Add(color);
        }
       
        if (amountPathResult == listPathManagers.Count)
        {
            CreateSpheras();
        }
       
    }

    private void CreateSpheras()
    {
        int currentSpheraAmount =0;
        for (int j = 0; j < listPathManagers.Count; j++)
        {
            PathManager pathManager = listPathManagers[j];
            List<ColorType> randomColorsForPathManager = new List<ColorType>();
            if (j ==listPathManagers.Count - 1)
            {
                if (pathManager.GetAmountPointsInPath() != colorList.Count)
                {
                    Debug.LogError("The number of colors does not correspond to the number of points in the paths");
                    return;
                }
                randomColorsForPathManager = new List<ColorType>(colorList);
            }
            else
            {
                
                Random rand = new Random();
               
                do
                {
                    int randomIndex= rand.Next(0, colorList.Count);
                    randomColorsForPathManager.Add(colorList[randomIndex]);
                    colorList.RemoveAt(randomIndex);
                   

                } while (randomColorsForPathManager.Count!=pathManager.GetAmountPointsInPath());
            }

            pathManager.SetRandomColor(randomColorsForPathManager);
        }
       
    }


    
}
