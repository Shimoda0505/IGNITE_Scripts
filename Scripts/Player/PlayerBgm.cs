using UnityEngine;



/// <summary>
/// �Q�[���S�̂�Bgm�Ǘ�
/// </summary>
public class PlayerBgm : MonoBehaviour
{

    #region �S�ĂŎg�p
    [SerializeField, Tooltip("Bgm��AudioSource")] AudioSource _audioSource;
    [SerializeField, Tooltip("Bgm�̉��ʑ��x")] private float _bgmChangeSpeed;
    private const int _minVolume = 0;//�{�����[���̍Œ�l
    private float _maxVolume = 1;//�{�����[���̍ő�l
    private BgmMotion _bgmMotion = BgmMotion.WAIT;//Bgm�̌��݂̏��
    enum BgmMotion
    {
        WAIT,//�Đ���
        START,//�J�n
        STOP,//�I��   
    }

    
    //������----------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�{�����[���̏�����
        _maxVolume = ParamSet.BGM_Volume;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

        //bgm�����[�v�ݒ�
        _audioSource.loop = true;
    }

    private void FixedUpdate()
    {

        switch (_bgmMotion)
        {

            //�Đ���
            case BgmMotion.WAIT:
                return;


            //�J�n
            case BgmMotion.START:

                //�I�[�f�B�I�̉��ʂ��グ��
                _audioSource.volume += Time.deltaTime * _bgmChangeSpeed;
                if (_audioSource.volume >= _maxVolume) { _bgmMotion = BgmMotion.WAIT; }
                break;


            //�I�� 
            case BgmMotion.STOP:

                //�I�[�f�B�I�̉��ʂ�������
                _audioSource.volume -= Time.deltaTime * _bgmChangeSpeed;
                if (_audioSource.volume <= _minVolume) { _audioSource.Stop(); }
                break;
        }
    }
    #endregion


    [Header("Stage��Bgm")]
    [SerializeField, Tooltip("1Stage")] AudioClip _oneBgm;
    /// <summary>
    /// BGM1��
    /// </summary>
    public void OneBgm()
    {

        //�I�[�f�B�I�ݒ�ƊJ�n
        _audioSource.clip = _oneBgm;
        _audioSource.Play();

        //�Đ���enum�ɕύX
        _bgmMotion = BgmMotion.START;
    }

    [SerializeField, Tooltip("2Stage")] AudioClip _secondBgm;
    /// <summary>
    /// BGM2��
    /// </summary>
    public void SecondBgm()
    {

        //�I�[�f�B�I�ݒ�ƊJ�n
        _audioSource.clip = _secondBgm;
        _audioSource.Play();

        //�Đ���enum�ɕύX
        _bgmMotion = BgmMotion.START;
    }

    [SerializeField, Tooltip("3Stage")] AudioClip _thirdBgm;
    /// <summary>
    /// BGM3��
    /// </summary>
    public void ThirdBgm()
    {

        //�I�[�f�B�I�ݒ�ƊJ�n
        _audioSource.clip = _thirdBgm;
        _audioSource.Play();

        //�Đ���enum�ɕύX
        _bgmMotion = BgmMotion.START;
    }

    [SerializeField, Tooltip("�N���A")] AudioClip _clearBgm;
    /// <summary>
    /// BGM�N���A
    /// </summary>
    public void ClearBgm()
    {

        //�O���BGM�I��
        _audioSource.Stop();

        //�I�[�f�B�I�ݒ�ƊJ�n
        _audioSource.clip = _clearBgm;
        _audioSource.Play();
    }

    [SerializeField, Tooltip("�Q�[���I�[�o�[")] AudioClip _gameOverBgm;
    /// <summary>
    /// BGM�Q�[���I�[�o�[
    /// </summary>
    public void GameOverBgm()
    {

        //�O���BGM�I��
        _audioSource.Stop();
        _audioSource.loop = false;

        //�I�[�f�B�I�ݒ�ƊJ�n
        _audioSource.clip = _gameOverBgm;
        _audioSource.Play();
    }

    /// <summary>
    /// BGM��~
    /// </summary>
    public void BgmStops() { _bgmMotion = BgmMotion.STOP; }
}
