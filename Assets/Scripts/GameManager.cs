using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private User _cpu1;
    private User _cpu2;
    
    private User _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _cpu1 = new User();
        _cpu2 = new User();
        _player = new User();
    }

    public void ProposeTrade()
    {
        
    }
}
