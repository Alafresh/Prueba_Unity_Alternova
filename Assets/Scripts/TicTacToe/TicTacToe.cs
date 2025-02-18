using UnityEngine;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    public Button[] buttons;
    private string currentPlayer = "X";
    private string[,] board = new string[3, 3];
    public Image[] turn;

    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => MakeMove(index));
        }
    }

    void MakeMove(int index)
    {
        int row = index / 3;
        int col = index % 3;

        if (string.IsNullOrEmpty(board[row, col]))
        {
            board[row, col] = currentPlayer;
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
            }
            else
            {
                turn[0].enabled = false;
                turn[1].enabled = true;
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

    void ShiftRowOrColumn(int row, int col)
    {
        if (IsRowFull(row))
        {
            for (int i = 0; i < 2; i++)
            {
                board[row, i] = board[row, i + 1];
                ActivateImage(buttons[row * 3 + i], board[row, i] ?? "");
            }
            board[row, 2] = null;
            ActivateImage(buttons[row * 3 + 2], "");
        }

        if (IsColumnFull(col))
        {
            for (int i = 0; i < 2; i++)
            {
                board[i, col] = board[i + 1, col];
                ActivateImage(buttons[i * 3 + col], board[i, col] ?? "");
            }
            board[2, col] = null;
            ActivateImage(buttons[2 * 3 + col], "");
        }
    }

    bool IsRowFull(int row)
    {
        return !string.IsNullOrEmpty(board[row, 0]) && !string.IsNullOrEmpty(board[row, 1]) && !string.IsNullOrEmpty(board[row, 2]);
    }

    bool IsColumnFull(int col)
    {
        return !string.IsNullOrEmpty(board[0, col]) && !string.IsNullOrEmpty(board[1, col]) && !string.IsNullOrEmpty(board[2, col]);
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
        }
        currentPlayer = "X";
    }
}
