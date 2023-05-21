using UnityEngine;



/// <summary>
/// プレイヤーのSE管理スクリプト
/// </summary>
public class PlayerSE : MonoBehaviour
{

    [SerializeField, Tooltip("オーディオソース")] AudioSource audioSource;

    [SerializeField, Tooltip("終了時間")] AudioClip _endTime;
    /// <summary>
    /// 終了時間
    /// </summary>
    public void EndTime() { audioSource.PlayOneShot(_endTime); }

    [SerializeField, Tooltip("ロックオン")] AudioClip rockOn;
    /// <summary>
    /// ロックオン
    /// </summary>
    public void RockOnSe() { audioSource.PlayOneShot(rockOn); }

    [SerializeField,Tooltip("火球")] AudioClip fireBollSe;
    /// <summary>
    /// 火球
    /// </summary>
    public void FireBollSe()
    {

        //重複でならないようにする
        if(!_isFireBollSe)
        {
            audioSource.PlayOneShot(fireBollSe);

            _isFireBollSe = true;
        }
    }

    [SerializeField, Tooltip("爆発(小)")] AudioClip explosionMini;
    /// <summary>
    /// 爆発(小)
    /// </summary>
    public void ExplosionMiniSe()
    {

        //重複でならないようにする
        if (!_isExplosionMini)
        {

            audioSource.PlayOneShot(explosionMini);

            _isExplosionMini = true;
        }
    }

    [SerializeField, Tooltip("爆発(大)")] AudioClip explosionBig;
    /// <summary>
    /// 爆発(大)
    /// </summary>
    public void ExplosionBigSe() { audioSource.PlayOneShot(explosionBig); }

    [SerializeField, Tooltip("水音")] AudioClip mizu;
    /// <summary>
    /// 水音
    /// </summary>
    public void Mizuse() { audioSource.PlayOneShot(mizu); }
    
    [SerializeField, Tooltip("柱")] AudioClip hasira;
    /// <summary>
    /// 柱
    /// </summary>
    public void HashiraSe()
    {

        audioSource.PlayOneShot(hasira);
        _isHasira = true;
    }

    [SerializeField, Tooltip("Crystal")] AudioClip crystal;
    /// <summary>
    /// Crystal
    /// </summary>
    public void CrystalBreakSe() { audioSource.PlayOneShot(crystal); }

    [SerializeField, Tooltip("風キリ音")] AudioClip windBgm;
    /// <summary>
    /// 風キリ音
    /// </summary>
    public void WindBgm()
    {
        audioSource.clip = windBgm;
        audioSource.Play();
    }

    [SerializeField, Tooltip("回復")] AudioClip heal;
    /// <summary>
    /// 回復
    /// </summary>
    public void HealSe() { audioSource.PlayOneShot(heal); }

    [SerializeField, Tooltip("回避")] AudioClip brink;
    /// <summary>
    /// 回避
    /// </summary>
    public void BrinkSe() { audioSource.PlayOneShot(brink); }

    /// <summary>
    /// seの停止
    /// </summary>
    public void StopBgm() { audioSource.Stop(); }


    //各Seが重複してならないようにする-----------------------------------------------------------------
    //爆発
    private bool _isExplosionMini = false;
    private float _exTime = 0.8f;
    private float _exCount = 0;
    //火球
    private bool _isFireBollSe;
    private float _fiTime = 0.5f;
    private float _fiCount = 0;
    //柱
    private bool _isHasira;
    private float _HasiraTime = 2.5f;
    private float _HasiraCount = 0;

    private void FixedUpdate()
    {
        
        if(_isExplosionMini)
        {
            _exCount += Time.deltaTime;
            if(_exCount >= _exTime)
            {
                _isExplosionMini = false;
                _exCount = 0;
            }
        }

        if(_isFireBollSe)
        {
            _fiCount += Time.deltaTime;
            if(_fiCount >= _fiTime)
            {
                _isFireBollSe = false;
                _fiCount = 0;
            }
        }

        if(_isHasira)
        {
            _HasiraCount += Time.deltaTime;
            if(_HasiraCount >= _HasiraTime)
            {
                _isHasira = false;
                _HasiraCount = 0;
            }
        }
    }
}
