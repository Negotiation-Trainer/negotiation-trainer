using Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public User Cpu1 { get; private set; }
    public User Cpu2 { get; private set; }
    public User Player { get; private set; }

    private void Awake()
    {
        Instance = this;
        
        Cpu1 = new User();
        Cpu2 = new User();
        Player = new User();
    }
}
