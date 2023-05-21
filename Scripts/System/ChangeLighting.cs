using UnityEngine;



/// <summary>
/// �X�e�[�W��Lighting�ύX
/// </summary>
public class ChangeLighting : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField,Tooltip("�v���C���[�̃��[�g�C�x���g�Ǘ��X�N���v�g")] RootNav _rootNav;
    AnyUseMethod _anyUseMethod;
    #endregion


    #region �����ݒ�
    [Header("�����ݒ�")]
    [SerializeField, Tooltip("�����̐F")] private Color _changeColor;
    [SerializeField, Tooltip("�F�̕ς�鑬�x")] private float _colorSpeed;
    private Color _defColor = default;//�����̊����̐F
    private Color _color = default;//�����̕ۊ�
    #endregion


    #region �t�H�O
    [Header("�t�H�O")]
    [SerializeField, Tooltip("�t�H�O�̋���")] private float _changeFogDensity;
    [SerializeField,Tooltip("�t�H�O�̑��x")] private float _fogSpeed;
    private float _defFogDensity = default;//�����̃t�H�O�̋���
    private float _fog = default;//�t�H�O�̕ۊ�
    #endregion


    #region ����
    [Header("����")]
    [SerializeField, Tooltip("����")] private float _time;
    private float _count = 0;//���ԃJ�E���g
    #endregion


    #region ���[�g
    [Header("���[�g�֘A")]
    [SerializeField, Tooltip("�ύX�|�C���g")] private GameObject[] _changePoints;
    private int _number = 0;//�ύX�ԍ�
    #endregion


    Motion _motion = Motion.WAIT;//�ύXenum
    enum Motion
    {
        
        WAIT,
        CHANGE
    }



    //������------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�����̊����̐F
        _defColor = RenderSettings.ambientSkyColor;

        //�����̃t�H�O�̋���
        _defFogDensity = RenderSettings.fogDensity;
    }

    private void FixedUpdate()
    {


        switch (_motion)
        {

            case Motion.WAIT:

                //�ԍ��I�u�W�F�N�g�Ȃ珈��
                if (_rootNav.NowPoint() == _changePoints[_number])
                {

                    //�ύX
                    if (_number == 0) { Change(_changeColor, _changeFogDensity); }

                    //������
                    else if (_number == 1) { Change(_defColor, _defFogDensity);}

                    //���[�g�ԍ��̍X�V
                    _number++;
                }
                break;


            case Motion.CHANGE:

                //���̕ύX
                RenderSettings.ambientSkyColor = _anyUseMethod.MoveToWardsColorVector3(RenderSettings.ambientSkyColor, _color, _colorSpeed);

                //�t�H�O�̕ύX
                RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, _fog, _fogSpeed);

                //���Ԍo�ߌ�ɏ���
                _count += Time.deltaTime;
                if (_count >= _time)
                {

                    //�����̕ύX
                    if(_number > _changePoints.Length - 1) { this.gameObject.GetComponent<ChangeLighting>().enabled = false; }

                    //������
                    _count = 0;
                    _motion = Motion.WAIT;
                }
                break;
        }
    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// �ύX(�F / ����)
    /// </summary>
    private void Change(Color color,float density) { _color = color; _fog = density; _motion = Motion.CHANGE; }

}
