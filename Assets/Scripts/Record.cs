using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record
{
    public int player1Wins = 0;
    public int player2Wins = 0;

    public Record(int p1Wins, int p2Wins)
    {
        player1Wins = p1Wins;
        player2Wins = p2Wins;
    }

    public Record()
    {

    }
}
