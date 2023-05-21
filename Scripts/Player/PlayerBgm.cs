using UnityEngine;



/// <summary>
/// ゲーム全体のBgm管理
/// </summary>
public class PlayerBgm : MonoBehaviour
{

    #region 全てで使用
    [SerializeField, Tooltip("BgmのAudioSource")] AudioSource _audioSource;
    [SerializeField, Tooltip("Bgmの音量速度")] private float _bgmChangeSpeed;
    private const int _minVolume = 0;//ボリュームの最低値
    private float _maxVolume = 1;//ボリュームの最大値
    private BgmMotion _bgmMotion = BgmMotion.WAIT;//Bgmの現在の状態
    enum BgmMotion
    {
        WAIT,//再生中
        START,//開始
        STOP,//終了   
    }

    
    //処理部----------------------------------------------------------------------------------------------------
    private void Start()
    {

        //ボリュームの初期化
        _maxVolume = ParamSet.BGM_Volume;/*【他メンバーが制作したため添付してません】*/

        //bgmをループ設定
        _audioSource.loop = true;
    }

    private void FixedUpdate()
    {

        switch (_bgmMotion)
        {

            //再生中
            case BgmMotion.WAIT:
                return;


            //開始
            case BgmMotion.START:

                //オーディオの音量を上げる
                _audioSource.volume += Time.deltaTime * _bgmChangeSpeed;
                if (_audioSource.volume >= _maxVolume) { _bgmMotion = BgmMotion.WAIT; }
                break;


            //終了 
            case BgmMotion.STOP:

                //オーディオの音量を下げる
                _audioSource.volume -= Time.deltaTime * _bgmChangeSpeed;
                if (_audioSource.volume <= _minVolume) { _audioSource.Stop(); }
                break;
        }
    }
    #endregion


    [Header("StageのBgm")]
    [SerializeField, Tooltip("1Stage")] AudioClip _oneBgm;
    /// <summary>
    /// BGM1番
    /// </summary>
    public void OneBgm()
    {

        //オーディオ設定と開始
        _audioSource.clip = _oneBgm;
        _audioSource.Play();

        //再生中enumに変更
        _bgmMotion = BgmMotion.START;
    }

    [SerializeField, Tooltip("2Stage")] AudioClip _secondBgm;
    /// <summary>
    /// BGM2番
    /// </summary>
    public void SecondBgm()
    {

        //オーディオ設定と開始
        _audioSource.clip = _secondBgm;
        _audioSource.Play();

        //再生中enumに変更
        _bgmMotion = BgmMotion.START;
    }

    [SerializeField, Tooltip("3Stage")] AudioClip _thirdBgm;
    /// <summary>
    /// BGM3番
    /// </summary>
    public void ThirdBgm()
    {

        //オーディオ設定と開始
        _audioSource.clip = _thirdBgm;
        _audioSource.Play();

        //再生中enumに変更
        _bgmMotion = BgmMotion.START;
    }

    [SerializeField, Tooltip("クリア")] AudioClip _clearBgm;
    /// <summary>
    /// BGMクリア
    /// </summary>
    public void ClearBgm()
    {

        //前回のBGM終了
        _audioSource.Stop();

        //オーディオ設定と開始
        _audioSource.clip = _clearBgm;
        _audioSource.Play();
    }

    [SerializeField, Tooltip("ゲームオーバー")] AudioClip _gameOverBgm;
    /// <summary>
    /// BGMゲームオーバー
    /// </summary>
    public void GameOverBgm()
    {

        //前回のBGM終了
        _audioSource.Stop();
        _audioSource.loop = false;

        //オーディオ設定と開始
        _audioSource.clip = _gameOverBgm;
        _audioSource.Play();
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void BgmStops() { _bgmMotion = BgmMotion.STOP; }
}
