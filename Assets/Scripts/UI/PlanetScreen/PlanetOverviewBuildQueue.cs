using System.Collections.Generic;
using UnityEngine;

public class PlanetOverviewBuildQueue : MonoBehaviour
{
    public PlanetOverview planetOverview;
    public Transform container;
    public GameObject queueUIPrefab;

    private List<QueueUI> queueUIs = new List<QueueUI>();
    public List<QueueUI> pool = new List<QueueUI>();

    private int maxPoolCount = 0;



    private void Awake()
    {
        maxPoolCount = pool.Count;
        //disable every object in the pool.
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].planetOverview = planetOverview;
            pool[i].gameObject.SetActive(false);

        }


    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void UpdateQueueTimer()
    {
        if (planetOverview.planet.planetColony.buildQueue.Count <= 0)
        {
            return;
        }
        queueUIs[0].UpdateProgress();

    }

    public void UpdateQueueChange(List<Queue> queue)
    {
        /*
        if(!added)
        {
            RemoveQueueUIElement(index);
            if (queueUIs.Count > 0)
            {
               UpdateQueueElement(index);
            }
        }
        else
        {
            AddQueueUIElement();
            UpdateQueueElement(index);
        }
        */

        int length = Mathf.Max(queueUIs.Count, queue.Count);
        for (int i = 0; i < length; i++)
        {
            //if the element is within both arrays update it.
            if (i < queue.Count && i < queueUIs.Count)
            {
                UpdateQueueElement(i);

            }
            else if (i >= queueUIs.Count && i >= queue.Count)
            {
                break;
            }
            //if the element is in the queue, but not in the UI queue, add a UI queue element.
            else if (i >= queueUIs.Count && i < queue.Count)
            {
                // print("add UI");
                AddQueueUIElement();
                UpdateQueueElement(i);
            }
            //if the element is in the UI queue, but not the normal queue, remove a UI queue element.
            else if (i >= queue.Count && i < queueUIs.Count)
            {
                //print("remove UI");
                RemoveQueueUIElement(i);
                i--;
                //i--;
            }
            length = Mathf.Max(queueUIs.Count, planetOverview.planet.planetColony.buildQueue.Count);
        }



        /*
        int iterations = Mathf.Max(queueUIs.Count, queue.Count) - Mathf.Min(queueUIs.Count, queue.Count);
        for (int i = 0; i < iterations; i++)
        {
            if(queueUIs.Count < queue.Count)
            {

                AddQueueUIElement();
                UpdateQueueElement(queueUIs.Count-1);
            }
            else
            {
                RemoveQueueUIElement();
                if (queueUIs.Count > 0)
                {
                    UpdateQueueElement(0);
                }
            }
        }
        */




    }

    //this code was made before I knew events were a thing. TODO: rewrite to use events.
    void UpdateQueue()
    {
        if (queueUIs.Count <= 0 && planetOverview.planet.planetColony.buildQueue.Count <= 0)
        {
            return;
        }
        int length = Mathf.Max(queueUIs.Count, planetOverview.planet.planetColony.buildQueue.Count);
        for (int i = 0; i < length; i++)
        {
            //if the element is within both arrays update it.
            if (i < planetOverview.planet.planetColony.buildQueue.Count && i < queueUIs.Count)
            {
                UpdateQueueElement(i);

            }
            else if (i >= queueUIs.Count && i >= planetOverview.planet.planetColony.buildQueue.Count)
            {
                break;
            }
            //if the element is in the queue, but not in the UI queue, add a UI queue element.
            else if (i >= queueUIs.Count && i < planetOverview.planet.planetColony.buildQueue.Count)
            {
                // print("add UI");
                AddQueueUIElement();
                UpdateQueueElement(i);
            }
            //if the element is in the UI queue, but not the normal queue, remove a UI queue element.
            else if (i >= planetOverview.planet.planetColony.buildQueue.Count && i < queueUIs.Count)
            {
                //print("remove UI");
                RemoveQueueUIElement();
                //i--;
            }
            length = Mathf.Max(queueUIs.Count, planetOverview.planet.planetColony.buildQueue.Count);
        }


    }

    void UpdateQueueElement(int index)
    {
        queueUIs[index].UpdateUI(planetOverview.planet.planetColony.buildQueue[index]);
    }

    QueueUI AddQueueUIElement()
    {
        QueueUI queueUI;
        if (pool.Count > 0)
        {
            pool[0].gameObject.SetActive(true);
            queueUI = pool[0];
            queueUIs.Add(pool[0]);

            pool.RemoveAt(0);

        }
        else
        {
            queueUI = Instantiate(queueUIPrefab, transform).GetComponent<QueueUI>();
            queueUI.planetOverview = planetOverview;
            queueUIs.Add(queueUI);
            queueUIs[queueUIs.Count - 1].itemIndex = queueUIs.Count - 1;
        }
        queueUI.SetProgress(0f);
        queueUI.transform.SetAsLastSibling();

        return queueUI;
    }

    void RemoveQueueUIElement(int index = -1)
    {
        if (index < 0)
        {
            index = queueUIs.Count - 1;
        }

        QueueUI queueUI = queueUIs[index];
        queueUIs.RemoveAt(index);
        queueUI.transform.SetAsLastSibling();

        if (pool.Count < maxPoolCount)
        {
            //queueUI.transform.parent.SetAsLastSibling();
            queueUI.gameObject.SetActive(false);
            pool.Add(queueUI);
        }
        else
        {
            Destroy(queueUI.gameObject);
        }

    }




    void AddQueueUIElement(Queue element)
    {
        QueueUI queueUI = AddQueueUIElement();
        UpdateQueueElement(queueUIs.Count - 1);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateQueueTimer();
    }
}
