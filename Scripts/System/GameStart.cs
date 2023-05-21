using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ゲーム開始演出の管理
/// </summary>
public class GameStart : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("ゲームのイベントやTime.deltaTimeを管理")] GameSystem _gameSystem;
    [SerializeField, Tooltip("ゲームのBGM")] PlayerBgm _gameBgm;
    #endregion


    #region その他
    [Header("その他")]
    [SerializeField, Tooltip("レティクル")] private GameObject _reticle;
    [SerializeField, Tooltip("フェードアウト用画像")] private Image FadeImage;
    [SerializeField, Tooltip("イベント用カメラ")] private GameObject _eventCam;
    [SerializeField, Tooltip("メインカメラ")] private GameObject _mainCam;
    #endregion



    //処理部---------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //イベントカメラアクティブ
        _eventCam.SetActive(true);

        //メインカメラのアクティブ終了
        _mainCam.GetComponent<Camera>().enabled = false;

        //イベントの開始
        _gameSystem.TrueIsEvent();

        //1番のBgm開始
        _gameBgm.OneBgm();

        //レティクル消す
        _reticle.SetActive(false);

        //フェードイン
        StartCoroutine("FadeIn");
    }



    //メソッド部---------------------------------------------------------------------------------------------------------------------------
    public void StartCamEnd()
    {

        //イベントの終了
        _gameSystem.FalseIsEvent();

        //メインカメラのアクティブ
        _mainCam.GetComponent<Camera>().enabled = true;

        //カーソルのアクティブ
        _reticle.SetActive(true);
    }

    //フェードイン
    IEnumerator FadeIn()
    {

        //フェードイン
        FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 1);

        //フェードアウト
        for (float i = 1; i > 0; i -= 0.01f)
        {

            FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, i);
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
}
