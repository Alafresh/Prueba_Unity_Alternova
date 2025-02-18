using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TicTacToe : MonoBehaviour
{
    public Button[] buttons;
    private string currentPlayer = "X";
    private string[,] board = new string[3, 3];
    private Queue<int>[] rowQueues = new Queue<int>[3];
    private Queue<int>[] colQueues = new Queue<int>[3];
    public Image[] turn;
    public AudioSource AudioSource;
    public AudioClip[] AudioClip;

    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => MakeMove(index));
        }

        for (int i = 0; i < 3; i++)
        {
            rowQueues[i] = new Queue<int>();
            colQueues[i] = new Queue<int>();
        }
    }

    void MakeMove(int index)
    {
        int row = index / 3;
        int col = index % 3;

        if (string.IsNullOrEmpty(board[row, col]))
        {
            board[row, col] = currentPlayer;
            rowQueues[row].Enqueue(index);
            colQueues[col].Enqueue(index);
            ActivateImage(buttons[index], currentPlayer);

            if (CheckWin())
            {
                Debug.Log(currentPlayer + " Wins!");
                ResetGame();
                return;
            }

            ShiftRowOrColumn(row, col);
            currentPlayer = (currentPlayer == "X") ? "O" : "X";

            if(currentPlayer == "X")
            {
                turn[0].enabled = true;
                turn[1].enabled = false;
                AudioSource.PlayOneShot(AudioClip[0]);
            }
            else
            {
                turn[0].enabled = false;
                turn[1].enabled = true;
                AudioSource.PlayOneShot(AudioClip[1]);
            }

        }
    }

    void ActivateImage(Button button, string player)
    {
        Transform dogImage = button.transform.Find("Dog");
        Transform catImage = button.transform.Find("Cat");

        if (dogImage != null && catImage != null)
        {
            dogImage.gameObject.SetActive(player == "X");
            catImage.gameObject.SetActive(player == "O");
        }
    }

    void Desactivate(Button button)
    {
        Transform dogImage = button.transform.Find("Dog");
        Transform catImage = button.transform.Find("Cat");
        if (dogImage != null && catImage != null)
        {
            dogImage.gameObject.SetActive(false);
            catImage.gameObject.SetActive(false);
        }
    }

    void ShiftRowOrColumn(int row, int col)
    {
        if (rowQueues[row].Count == 3)
        {
            int firstIndex = rowQueues[row].Dequeue();
            board[firstIndex / 3, firstIndex % 3] = null;
            Desactivate(buttons[firstIndex]);
        }

        if (colQueues[col].Count == 3)
        {
            int firstIndex = colQueues[col].Dequeue();
            board[firstIndex / 3, firstIndex % 3] = null;
            Desactivate(buttons[firstIndex]);
        }
    }

    bool CheckWin()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == currentPlayer && board[i, 1] == currentPlayer && board[i, 2] == currentPlayer)
                return true;
            if (board[0, i] == currentPlayer && board[1, i] == currentPlayer && board[2, i] == currentPlayer)
                return true;
        }
        return (board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer) ||
               (board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer);
    }

    void ResetGame()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = null;
                ActivateImage(buttons[i * 3 + j], "");
            }
            rowQueues[i].Clear();
            colQueues[i].Clear();
        }
        currentPlayer = "X";
    }
}
