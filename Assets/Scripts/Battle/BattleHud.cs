using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
  [SerializeField] Text nameText;
  [SerializeField] Text levelText;
  [SerializeField] Text statusText;
  [SerializeField] HPBar hpBar;
  [SerializeField] GameObject expBar;

  [SerializeField] Color psnColor;
  [SerializeField] Color brnColor;
  [SerializeField] Color slpColor;
  [SerializeField] Color parColor;
  [SerializeField] Color frzColor;


    Pokemon Pokemon;
    Dictionary<ConditionID, Color> statusColors;

  public void SetData(Pokemon pokemon)
  {
        if (Pokemon != null)
        {
            Pokemon.OnStatusChanged -= SetStatusText;
            Pokemon.OnHPChanged -= UpdateHP;
        }
        Pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        SetLevel();
        hpBar.SetHP((float) pokemon.HP / pokemon.MaxHp);
        SetExp();

        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn, psnColor },
            {ConditionID.brn, brnColor },
            {ConditionID.slp, slpColor },
            {ConditionID.par, parColor },
            {ConditionID.frz, frzColor },
        };

        SetStatusText();
        Pokemon.OnStatusChanged += SetStatusText;
        Pokemon.OnHPChanged += UpdateHP;
  }

    void SetStatusText()
    {
        if (Pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = Pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[Pokemon.Status.Id];
        }
    }

    public void SetLevel()
    {
        levelText.text = "Lvl " + Pokemon.Level;

    }

    public void SetExp()
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }
    public IEnumerator SetExpSmooth(bool reset=false)
    {
        // need to use yield break for coroutine like Ienumerator instead of return****
        if (expBar == null) yield break;

        if (reset)
            expBar.transform.localScale = new Vector3(0, 1, 1);

        float normalizedExp = GetNormalizedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }

    float GetNormalizedExp()
    {
        int currLevelExp = Pokemon.Base.GetExpForLevel(Pokemon.Level);
        int nextLevelExp = Pokemon.Base.GetExpForLevel(Pokemon.Level + 1);

        float normalizedExp = (float) (Pokemon.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    public void UpdateHP()
    {
        StartCoroutine(UpdateHPAsync());
    }

    public IEnumerator UpdateHPAsync()
    {
       yield return hpBar.SetHPSmooth((float)Pokemon.HP / Pokemon.MaxHp); 
    }

    public IEnumerator WaitForHPUpdate()
    {
        yield return new WaitUntil(() => hpBar.IsUpdating == false);
    }

    public void ClearData()
    {
        if (Pokemon != null)
        {
            Pokemon.OnStatusChanged -= SetStatusText;
            Pokemon.OnHPChanged -= UpdateHP;
        }
    }
}
