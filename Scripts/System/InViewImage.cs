using UnityEngine;


/// <summary>
/// �G�����߂ĉ�ʂɉf���������Cursor�Œm�点��ۂ́ACursor�����Ǘ�
/// </summary>
public class InViewImage : MonoBehaviour
{

    public GameObject _target = default;//�Ǐ]����^�[�Q�b�g


    [SerializeField,Tooltip("�Q�[���C�x���g�Ǘ��X�N���v�g")] GameSystem _gameSystem;
    [SerializeField,Tooltip("�J�[�\����RectTransform")] private RectTransform _rect;

    private Vector2 _rectPos = default;//�J�[�\���̉�ʍ��W
    private Vector2 _upRect = new Vector2(0, 20);//�X�V���W

    private float _time = 1f;//����
    private float _count = 0;//�J�E���g



    //������-------------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //�C�x���g���͏������Ȃ�
        if (_gameSystem.IsEvent()) { return; }

        //���Ԍv����ɒ�~
        _count += Time.deltaTime;
        if(_count >= _time) { _count = 0; this.gameObject.SetActive(false); }

        //�^�[�Q�b�g�̃X�N���[�����W�ɕϊ�
        Vector2 enemyPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);

        //�X�N���[�����W��RectTransfirm�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, enemyPos, Camera.main, out _rectPos);

        //�^�[�Q�b�g�̍��W�Ɉړ�
        this.gameObject.GetComponent<RectTransform>().localPosition = _rectPos + _upRect;
    }
}
