using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class InfoUser : MonoBehaviour
{
    [SerializeField] private Text _titleLabel;
    [SerializeField] private GameObject _objLoadingIcon;

    public void Run()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
    }

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        _objLoadingIcon.SetActive(false);
        _titleLabel.text = $"Welcome back, {result.AccountInfo.Username}. The last time you logged in: {result.AccountInfo.TitleInfo.LastLogin}";
    }

    private void OnFailure(PlayFabError error)
    {
        _objLoadingIcon.SetActive(false);
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}
