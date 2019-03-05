using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtl_Type_01 : GameController
{
    private void Start()
    {
        AudioSourceSystem.Instance.PlayBackgroundMusic(AudioSourceType.AT_BG_Map01);
    }
    #region Gizmos 路点 辅助线

    public PathNode BluePathNode;
    public PathNode RedpathNode;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (BluePathNode != null)
        {
            var go = BluePathNode;
            while (true)
            {
                if (go.BuleNextNode != null)
                {
                    Gizmos.DrawLine(go.transform.position, go.BuleNextNode.transform.position);
                    go = go.BuleNextNode;
                }
                else
                {
                    break;
                }
            }
        }

        Gizmos.color = Color.blue;
        if (RedpathNode != null)
        {
            var go = RedpathNode;
            while (true)
            {
                if (go.RedNextNode != null)
                {
                    Gizmos.DrawLine(go.transform.position, go.RedNextNode.transform.position);
                    go = go.RedNextNode;
                }
                else
                {
                    break;
                }
            }
        }


    }

    #endregion
}
