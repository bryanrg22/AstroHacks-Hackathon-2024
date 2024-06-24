using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownForSearchButton : MonoBehaviour
{
    static List<string> names = new List<string>();
    public TMP_InputField field;
    void Start()
    {
        for (int i = 0; i < PlanetManager.planets; i++)
        {
            names.Add(PlanetManager.planetNames[i]);
        }

        UpdateDropdown("");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateDropdown(string dropdownContents)
    {
        GetComponent<TMP_Dropdown>().ClearOptions();
        string name = dropdownContents.ToLower();
        for (int i = 0; i < names.Count; i++)
        {
            if (name.Equals(names[i].ToLower().Substring(0, name.Length))) {
                GetComponent<TMP_Dropdown>().AddOptions(new List<string>() { names[i] });
            }
        }
    }

    public void UpdateTextBox()
    {
        field.text = GetComponent<TMP_Dropdown>().options[GetComponent<TMP_Dropdown>().value].text;
    }
}
