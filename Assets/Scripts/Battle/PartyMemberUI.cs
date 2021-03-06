using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Pokemon Pokemon;

    public void Init(Pokemon pokemon)
    {
        Pokemon = pokemon;
        UpdateData();

        Pokemon.OnHPChanged += UpdateData;
        
    }

    void UpdateData()
    {
        nameText.text = Pokemon.Base.Name;
        levelText.text = "Lvl " + Pokemon.Level;
        hpBar.SetHP((float)Pokemon.HP / Pokemon.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = GlobalSettings.i.HighlightedColor;
        else
            nameText.color = Color.black;
    }
}
