using UnityEngine;



/// <summary>
/// �v���C���[��SE�Ǘ��X�N���v�g
/// </summary>
public class PlayerSE : MonoBehaviour
{

    [SerializeField, Tooltip("�I�[�f�B�I�\�[�X")] AudioSource audioSource;

    [SerializeField, Tooltip("�I������")] AudioClip _endTime;
    /// <summary>
    /// �I������
    /// </summary>
    public void EndTime() { audioSource.PlayOneShot(_endTime); }

    [SerializeField, Tooltip("���b�N�I��")] AudioClip rockOn;
    /// <summary>
    /// ���b�N�I��
    /// </summary>
    public void RockOnSe() { audioSource.PlayOneShot(rockOn); }

    [SerializeField,Tooltip("�΋�")] AudioClip fireBollSe;
    /// <summary>
    /// �΋�
    /// </summary>
    public void FireBollSe()
    {

        //�d���łȂ�Ȃ��悤�ɂ���
        if(!_isFireBollSe)
        {
            audioSource.PlayOneShot(fireBollSe);

            _isFireBollSe = true;
        }
    }

    [SerializeField, Tooltip("����(��)")] AudioClip explosionMini;
    /// <summary>
    /// ����(��)
    /// </summary>
    public void ExplosionMiniSe()
    {

        //�d���łȂ�Ȃ��悤�ɂ���
        if (!_isExplosionMini)
        {

            audioSource.PlayOneShot(explosionMini);

            _isExplosionMini = true;
        }
    }

    [SerializeField, Tooltip("����(��)")] AudioClip explosionBig;
    /// <summary>
    /// ����(��)
    /// </summary>
    public void ExplosionBigSe() { audioSource.PlayOneShot(explosionBig); }

    [SerializeField, Tooltip("����")] AudioClip mizu;
    /// <summary>
    /// ����
    /// </summary>
    public void Mizuse() { audioSource.PlayOneShot(mizu); }
    
    [SerializeField, Tooltip("��")] AudioClip hasira;
    /// <summary>
    /// ��
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

    [SerializeField, Tooltip("���L����")] AudioClip windBgm;
    /// <summary>
    /// ���L����
    /// </summary>
    public void WindBgm()
    {
        audioSource.clip = windBgm;
        audioSource.Play();
    }

    [SerializeField, Tooltip("��")] AudioClip heal;
    /// <summary>
    /// ��
    /// </summary>
    public void HealSe() { audioSource.PlayOneShot(heal); }

    [SerializeField, Tooltip("���")] AudioClip brink;
    /// <summary>
    /// ���
    /// </summary>
    public void BrinkSe() { audioSource.PlayOneShot(brink); }

    /// <summary>
    /// se�̒�~
    /// </summary>
    public void StopBgm() { audioSource.Stop(); }


    //�eSe���d�����ĂȂ�Ȃ��悤�ɂ���-----------------------------------------------------------------
    //����
    private bool _isExplosionMini = false;
    private float _exTime = 0.8f;
    private float _exCount = 0;
    //�΋�
    private bool _isFireBollSe;
    private float _fiTime = 0.5f;
    private float _fiCount = 0;
    //��
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
