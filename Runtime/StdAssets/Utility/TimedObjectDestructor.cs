using System;
using UnityEngine;
using static UnityEngine.Random;
namespace UnityStandardAssets.Utility
{
    public class TimedObjectDestructor : MonoBehaviour
    {
        [SerializeField] private float m_TimeOut = 1.0f;
        [SerializeField] private float m_TimeOutRandomisationTime = 1.0f;
        [SerializeField] private bool m_DetachChildren = false;


        private void Awake()
        {
            Invoke("DestroyNow", m_TimeOut + RandomRange(0,m_TimeOutRandomisationTime));
        }


        private void DestroyNow()
        {
            if (m_DetachChildren)
            {
                transform.DetachChildren();
            }
            Destroy(gameObject);
        }
    }
}
