using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PathManager> listPathManagers;
    [SerializeField] private BallItem ballPrefab;
    [SerializeField] private List<MovingPart> listMovingParts;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private ScriptableObject_ColorData colorData;
    [SerializeField] private SoundManager soundManager;
    
    
    
    private int drop = 1; //default =1// коефіцієнт поділу , впливає на точність розрахунків (чим більша цифра тим більше буде точок для переміщення сфер)

    private int amountPathResult;
    // private Dictionary<COLORTYPE, int> dictionaryColorAmount = new Dictionary<COLORTYPE, int>();
   private List<ColorType> colorList = new List<ColorType>();
   //словник готовності сортування
   private Dictionary<int, bool> readySortDictionary = new Dictionary<int, bool>();

   private void Awake()
   {
       QualitySettings.vSyncCount = 1;
       Application.targetFrameRate = 60;
   }

   void Start()
    {
        winWindow.SetActive(false);
       foreach (PathManager pathManager in listPathManagers)
        {
            pathManager.IsPath += OnIsPath;
            pathManager.OnBallsMove += OnBallMove;
            
            pathManager.SetBallPrefab(ballPrefab);
            pathManager.SetDrop(drop);
            pathManager.SetColorData(colorData);
            pathManager.StartCalculatePath();
        }

        GlobalSignals.OnDisplacementBallIntoNewPath += OnDisplacementBallIntoNewPath;
        foreach (MovingPart movingPart in listMovingParts)
        {
            movingPart.SwitchBlockPath += OnSwitchBlockPath;
            movingPart.OnCheckSortedBalls += OnCheckSortedBalls;
            movingPart.OnClick += OnMovingPartClick;

        }

        CompletingDictionaryForSortData();
        

    }

   


   private void CompletingDictionaryForSortData()
    {
        foreach (PathManager pathManager in listPathManagers)
        {
           bool isReadyBallsSort= pathManager.IsReadyBallsSort();
           readySortDictionary[pathManager.Id] = isReadyBallsSort;
        }
        FinalCheckSortedBalls();
    }
   


    private void OnIsPath(int  pointsInPath, ColorType color)
    {
        amountPathResult++;
        for (int i = 0; i < pointsInPath; i++)
        {
           colorList.Add(color);
        }
       
        if (amountPathResult == listPathManagers.Count)
        {
            CreateBalls();
        }
       
    }
    
    private void OnBallMove()
    {
        soundManager.PlayBallMovingPlay();
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
    
    private void OnDisplacementBallIntoNewPath(BallItem ball, int pathid, int pathindex)
    {
        PathManager pathManager = listPathManagers.Find(path => path.Id == pathid);
        if (pathManager == null)
        {
            Debug.LogError("Didn`t find path manager wit id "+pathid);
            return;
        }
        
        pathManager.SetBallByPathIndex(ball, pathindex);
    }
    
    private void OnSwitchBlockPath(bool b, List<IPathID> listPathId)
    {
        foreach (IPathID path in listPathId)
        {
            PathManager pathManager = listPathManagers.Find(pm => pm.Id == path.Id);
            if (pathManager == null)
            {
                Debug.LogError($"Path Manager id = {path.Id} doesn`t find in listPathManagers");
                return;
            }

            pathManager.SetBlock(b);
        }
    }
    
    private void OnCheckSortedBalls(List<IPathID> listPathId)
    {
        foreach (IPathID pathID in listPathId)
        {
            PathManager pathManager = listPathManagers.Find(pm => pm.Id == pathID.Id);
            if (pathManager == null)
            {
                Debug.LogError("Didn`t find path manager with id = "+pathID.Id);
                return;
            }

            bool isReadyBallsSort = pathManager.IsReadyBallsSort();
            readySortDictionary[pathManager.Id] = isReadyBallsSort;
        }

        FinalCheckSortedBalls();
    }

    private void FinalCheckSortedBalls()
    {
        bool isAllReady = readySortDictionary.Values.All(isready => isready);
        if (isAllReady)
        {
            winWindow.SetActive(true);
            soundManager.PlayWinSound();
        }
    }
    
    private void OnMovingPartClick()
    {
        soundManager.PlayClickMovingPartSound();
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
