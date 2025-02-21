using System.Collections;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    Board m_board;

    bool m_isMooving = false;

    public InterpType interpolation = InterpType.SmootherStep;

    public enum InterpType
    { 
    Linear, 
    EaseOut,
    EaseIn,
    SmoothStep,
    SmootherStep
    }

    public MatchValue matchValue;
    public enum MatchValue
    {
        Yellow,
        Blue,
        Magenta,
        Indigo,
        Green,
        Teal,
        Red,
        Cyan,
        Wild
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move((int)transform.position.x + 1, (int)transform.position.y, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move((int)transform.position.x - 1, (int)transform.position.y, 0.5f);
        }
        */
    }

    public void Init(Board board)
    {

        m_board = board;
    }

    public void SetCoord(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void Move(int destX, int destY, float timeToMove)
    {
        if(!m_isMooving)
        {
            StartCoroutine(MoveRoutine(new Vector3(destX,destY,0),timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
    {
        Vector3 startPosition = transform.position;
        bool reachedDestination = false;
        float ellapsedTime = 0f;

        m_isMooving = true;

        while(!reachedDestination)
        {
            //if we are close enough to destination
            if(Vector3.Distance(transform.position, destination)< 0.01f)
            {
                reachedDestination = true;

                //transform.position = destination;
                if (m_board != null)
                {
                    m_board.PlaceGamePiece(this, (int)destination.x, (int)destination.y);
                }
                //SetCoord((int)destination.x,(int)destination.y);

                break;
            }

            ellapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(ellapsedTime / timeToMove,0f,1f);


            switch (interpolation)
            {
                case InterpType.Linear:
                    //t = t;
                    break;
                case InterpType.EaseIn:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.EaseOut:
                    t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.SmoothStep:
                    t = t * t * (t * (3-2*t));

                    break;
                case InterpType.SmootherStep:
                    t = t * t * t * t * (t * (t * 6 - 15) + 10);

                    break;
            }

            transform.position = Vector3.Lerp(startPosition, destination, t);

            //wait until next frame
            yield return null;
        }

        m_isMooving = false;
    }
}
