using UnityEngine;



/// <summary>
/// Cursor�̍��W���ړ�������
/// </summary>
public class CursorController : MonoBehaviour
{

    public GameObject _target = default;//�Ǐ]����^�[�Q�b�g
    [SerializeField,Tooltip("�J�[�\���̉摜�Ǘ��X�N���v�g")] PointerImage _pointerImage;
    [SerializeField,Tooltip("�Q�[���V�X�e���X�N���v�g")] GameSystem _gameSystem;
    [SerializeField,Tooltip("CanvasTransform")] private RectTransform _rect;
    private Vector2 _rectPos = default;//�X�N���[�����W
    private Vector2 _upRect = new Vector2(0, 20);



    //����-------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {

        //�^�[�Q�b�g��Bull�ɂȂ�����
        if(_target ==null)
        {

            //�J�[�\���̃J���[�ύX
            _pointerImage.DefaultColorChange();

            //�J�[�\���̃A�N�e�B�u������
            this.gameObject.SetActive(false);

            return;
        }

        //�C�x���g���͏������Ȃ�
        if (_gameSystem.IsEvent()) { return; }

        //�^�[�Q�b�g�̃X�N���[�����W�ɕϊ�
        Vector2 enemyPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);

        //�X�N���[�����W��RectTransfirm�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, enemyPos, Camera.main, out _rectPos);

        //�^�[�Q�b�g�̍��W�Ɉړ�
        this.gameObject.GetComponent<RectTransform>().localPosition = _rectPos + _upRect;
    }
}
