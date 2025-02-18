using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ricimi;
using System.Collections;

public class TicTacToeLimited : MonoBehaviour
{
    public PopupOpener popupOpener;
    public LineRenderer lineRenderer;
    public Button[] buttons;
    private string currentPlayer = "X";
    private string[,] board = new string[3, 3];
    private Dictionary<string, Queue<int>> playerMoves = new Dictionary<string, Queue<int>>
    {
        { "X", new Queue<int>() },
        { "O", new Queue<int>() }
    };
    public Image[] turn;
    public AudioSource audioSource;
    public AudioClip[] audioClip;

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

        if (board[row, col] == null || playerMoves[currentPlayer].Contains(index))
        {
            if (playerMoves[currentPlayer].Count == 3)
            {
                int oldIndex = playerMoves[currentPlayer].Dequeue();
                int oldRow = oldIndex / 3;
                int oldCol = oldIndex % 3;
                board[oldRow, oldCol] = null;
                ActivateImage(buttons[oldIndex], "");
            }

            board[row, col] = currentPlayer;
            playerMoves[currentPlayer].Enqueue(index);
            ActivateImage(buttons[index], currentPlayer);

            if (CheckWin())
            {
                StartCoroutine(Wait());
                Debug.Log(currentPlayer + " Wins!");
                return;
            }

            currentPlayer = (currentPlayer == "X") ? "O" : "X";

            if (currentPlayer == "X")
            {
                turn[0].enabled = true;
                turn[1].enabled = false;
                audioSource.PlayOneShot(audioClip[1]);
            }
            else
            {
                turn[0].enabled = false;
                turn[1].enabled = true;
                audioSource.PlayOneShot(audioClip[0]);
            }
            Handheld.Vibrate();
        }
    }

    public IEnumerator Wait()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        yield return new WaitForSeconds(3);
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
        popupOpener.OpenPopup();
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

    bool CheckWin()
    {
        
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == currentPlayer && board[i, 1] == currentPlayer && board[i, 2] == currentPlayer)
            {
                switch (i)
                {
                    case 0:
                        {
                            lineRenderer.SetPosition(0, new Vector3(-5, 2.5f, 0));
                            lineRenderer.SetPosition(1, new Vector3(5, 2.5f, 0));
                            break;
                        }
                    case 1:
                        {
                            lineRenderer.SetPosition(0, new Vector3(-5, 0, 0));
                            lineRenderer.SetPosition(1, new Vector3(5, 0, 0));
                            break;
                        }
                    case 2:
                        {
                            lineRenderer.SetPosition(0, new Vector3(-5, -2.5f, 0));
                            lineRenderer.SetPosition(1, new Vector3(5, -2.5f, 0));
                            break;
                        }
                }
                return true;
            }
            if (board[0, i] == currentPlayer && board[1, i] == currentPlayer && board[2, i] == currentPlayer)
            {
                switch (i)
                {
                    case 0:
                        {
                            lineRenderer.SetPosition(0, new Vector3(-3, 4, 0));
                            lineRenderer.SetPosition(1, new Vector3(-3, -4, 0));
                            break;
                        }
                    case 1:
                        {
                            lineRenderer.SetPosition(0, new Vector3(0, 4, 0));
                            lineRenderer.SetPosition(1, new Vector3(0, -4, 0));
                            break;
                        }
                    case 2:
                        {
                            lineRenderer.SetPosition(0, new Vector3(3, 4, 0));
                            lineRenderer.SetPosition(1, new Vector3(3, -4, 0));
                            break;
                        }
                }
                return true;
            }
                
        }
        if (board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer)
        {
            lineRenderer.SetPosition(0, new Vector3(-5, 5, 0));
            lineRenderer.SetPosition(1, new Vector3(5, -5, 0));
            return true;
        }
        if (board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer)
        {
            lineRenderer.SetPosition(0, new Vector3(5, 5, 0));
            lineRenderer.SetPosition(1, new Vector3(-5, -5, 0));
            return true;
        }
        return false;
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
        playerMoves["X"].Clear();
        playerMoves["O"].Clear();
        currentPlayer = "X";
    }
}
