using UnityEngine;



/// <summary>
/// ボスのse管理
/// </summary>
public class RaizoSe : MonoBehaviour
{

    [SerializeField,Tooltip("オーディオ")] AudioSource _audioSource;


    //メソッド部----------------------------------------------------------------------------------------------------

    [SerializeField, Tooltip("咆哮")] private AudioClip _roar;
    /// <summary>
    /// 咆哮
    /// </summary>
    public void RoarSe() { _audioSource.PlayOneShot(_roar); }

    [SerializeField, Tooltip("大咆哮")] private AudioClip _bigRoar;
    /// <summary>
    /// 大咆哮
    /// </summary>
    public void BigRoarSe() { _audioSource.PlayOneShot(_bigRoar); }

    [SerializeField, Tooltip("タックル")] private AudioClip _tackle;
    /// <summary>
    /// タックル
    /// </summary>
    public void TackleSe() { _audioSource.PlayOneShot(_tackle); }

    [SerializeField, Tooltip("風圧")] private AudioClip _wing;
    /// <summary>
    /// 咆哮
    /// </summary>
    public void WingSe() { _audioSource.PlayOneShot(_wing); }

    [SerializeField, Tooltip("雷弾")] private AudioClip _bullet;
    /// <summary>
    /// 雷弾
    /// </summary>
    public void BulletSe() { _audioSource.PlayOneShot(_bullet); }

    [SerializeField, Tooltip("大雷弾")] private AudioClip _bigBullet;
    /// <summary>
    /// 大雷弾
    /// </summary>
    public void BigBulletSe() { _audioSource.PlayOneShot(_bigBullet); }

    [SerializeField, Tooltip("雷弾ヒット")] private AudioClip _bulletHit;
    /// <summary>
    /// 雷弾
    /// </summary>
    public void BulletHitSe() { _audioSource.PlayOneShot(_bulletHit); }

    [SerializeField, Tooltip("大雷弾ヒット")] private AudioClip _bigBulletHit;
    /// <summary>
    /// 大雷弾
    /// </summary>
    public void BigBulletHitSe() { _audioSource.PlayOneShot(_bigBulletHit); }

    [SerializeField, Tooltip("落雷")] private AudioClip _bolt;
    /// <summary>
    /// 落雷
    /// </summary>
    public void BoltSe() { _audioSource.PlayOneShot(_bolt); }
}
