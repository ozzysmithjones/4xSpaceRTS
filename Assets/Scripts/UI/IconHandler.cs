using System.Collections.Generic;
using UnityEngine;

public class IconHandler : MonoBehaviour
{

    public Star star;

    public GameObject IconPrefab;

    //the first four icons are in object pools to improve performance.
    //Icons come in two layers: civilian ships and military ships.(uusing the settings, the player can hide a layer so as to not be
    //overwhelmed.
    private int poolSize = 4;
    public List<IconData> iconData = new List<IconData>();

    private List<IconData> pool = new List<IconData>();

    public Transform military;
    public Transform civilian;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            IconData icon = Instantiate(IconPrefab, transform).GetComponent<IconData>();
            //iconData.Add(icon);
            pool.Add(icon);

            icon.gameObject.SetActive(false);


        }
    }

    public void AddIcon(Navigator navigator, Sprite sprite, bool military = true, int faction = -1)
    {

        int id = FirstAvalibleID();
        navigator.iconHandlerID = id;

        //if possible, re-use an icon from the pool
        if (pool.Count > 0)
        {

            pool[0].gameObject.SetActive(true);
            pool[0].transform.SetParent(military ? this.military : this.civilian);
            pool[0].ID = id;
            pool[0].SetImage(sprite, military, faction);

            iconData.Add(pool[0]);
            pool.RemoveAt(0);
        }
        else
        {
            IconData icon = Instantiate(IconPrefab, transform).GetComponent<IconData>();
            icon.ID = id;
            icon.SetImage(sprite, military, faction);
            icon.transform.SetParent(military ? this.military : this.civilian);
            iconData.Add(icon);
        }
    }

    int FirstAvalibleID()
    {

        int iterations = 0;
        int id = iconData.Count;
        bool first = true;

        while (iterations < iconData.Count)
        {
            if (iconData[iterations].ID != id)
            {
                iterations++;
            }
            else
            {

                iterations = 0;
                if (!first)
                {
                    id++;
                }
                else
                {
                    first = false;
                    id = 0;
                }
            }

        }

        return id;
    }

    public void RemoveIcon(int id)
    {
        if (iconData.Count <= 0 || id < 0)
        {
            return;
        }

        bool delete = iconData.Count > poolSize ? true : false;
        int toRemove = -1;

        for (int i = 0; i < iconData.Count; i++)
        {
            if (iconData[i].ID == id)
            {
                toRemove = i;
                break;
            }

            if (i >= iconData.Count - 1)
            {
                return;
            }
        }
        if (toRemove < 0)
        {
            return;
        }


        if (delete)
        {

            Destroy(iconData[toRemove].gameObject);
            iconData.RemoveAt(toRemove);
        }
        else
        {
            iconData[toRemove].gameObject.SetActive(false);
            iconData[toRemove].ID = -1;
            iconData[toRemove].SetImage(null);

            pool.Add(iconData[toRemove]);
            iconData.RemoveAt(toRemove);
        }

    }

    public Navigator FindFleet(int id)
    {
        StarShipManager starShipManager = star.starShipManager;

        for (int i = 0; i < starShipManager.fleets.Count; i++)
        {
            if (starShipManager.fleets[i].iconHandlerID == id)
            {
                return starShipManager.fleets[i];
            }
        }

        return null;
    }

    public IconData FindIcon(int id)
    {
        for (int i = 0; i < iconData.Count; i++)
        {
            if (iconData[i].ID == id)
            {
                return iconData[i];
            }
        }

        return null;
    }
}
