using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catalog : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private List<Text> _buttonsText;

    private List<CatalogItem> _catalog = new List<CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        HandleCatalog(result.Catalog);
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item);
        }

        CatalogFormation();
    }

    private void CatalogFormation()
    {
        for(int i = 0; i < (_catalog.Count < 10 ? _catalog.Count : 10); i++)
        {
            _buttonsText[i].text = _catalog[i].DisplayName;
            var cat = _catalog[i];
            _buttons[i].interactable = true;
            _buttons[i].onClick.AddListener(delegate { ClickButtons((CatalogItem)cat); });
        }
    }

    private void ClickButtons(CatalogItem cat)
    {
        Debug.Log($"{cat.DisplayName} - {cat.VirtualCurrencyPrices["GD"]} gold.");
    }
}
