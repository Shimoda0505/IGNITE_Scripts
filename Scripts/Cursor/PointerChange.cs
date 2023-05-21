using UnityEngine;



/// <summary>
/// ロックオン数やウルト使用に応じて、カーソルの見た目を変更
/// </summary>
public class PointerChange : MonoBehaviour
{

    [SerializeField,Tooltip("ロックオン管理スクリプト")] PointerLockOn _pointerLockOn;
    [SerializeField,Tooltip("通常レティクル1")] private GameObject _nomalRet;
    [SerializeField,Tooltip("通常レティクル2")] private GameObject _nomalRet2;
    [SerializeField,Tooltip("通常レティクル3")] private GameObject _nomalRet3;
    [SerializeField,Tooltip("ウルトレティクル")] private GameObject _ultRet;
    [SerializeField,Tooltip("ウルトレティクルのアニメーション")] private Animator _ultRetAnim;
    [SerializeField,Tooltip("ウルトレティクルが終了するまでの時間")] private float _time;
    private float _count = 0;//時間計測
    private bool _isUlt = false;//ウルトレティクルがアクティブ中か



    private void FixedUpdate()
    {
        
        //ウルトレティクルがアクティブなら処理
        if(_isUlt)
        {

            //時間計測後に処理
            _count += Time.deltaTime;
            if(_count >= _time)
            {

                //時間初期化
                _count = 0;

                //ウルト終了
                _isUlt = false;

                //通常レティクルをアクティブ
                _nomalRet.SetActive(true);

                //ウルトレティクルを非アクティブ
                _ultRet.SetActive(false);
            }
        }
    }


    /// <summary>
    /// レティクルの変更
    /// </summary>
    public void ChangeRet()
    {

        //ノーマル
        if(_nomalRet.activeSelf)
        {

            //レティクルのアクティブ変更
            _nomalRet.SetActive(false);
            _ultRet.SetActive(true);

            //ロックオン範囲の変更
            _pointerLockOn.ChangePointerClamp("ウルト");

            //ウルトレティクルのアニメーション
            _ultRetAnim.SetTrigger("Move");

        }
        //ウルト
        else if (!_nomalRet.activeSelf)
        {

            //ウルト開始
            _isUlt = true;

            //ウルトレティクルのアニメーション
            _ultRetAnim.SetTrigger("ReMove");
        }
    }


    /// <summary>
    /// 通常レティクル1番のみアクティブ
    /// </summary>
    public void Change1()
    {

        //サイズ変更
        _nomalRet.transform.localScale = new Vector2(1, 1);

        //レティクルのアクティブ変更
        _nomalRet2.SetActive(false);
        _nomalRet3.SetActive(false);
    }

    /// <summary>
    /// 通常レティクル2番のみアクティブ
    /// </summary>
    public void Change2()
    {

        //サイズ変更
        _nomalRet.transform.localScale = new Vector2(1.4f, 1.4f);

        //レティクルのアクティブ変更
        _nomalRet2.SetActive(true);
        _nomalRet3.SetActive(false);
    }

    /// <summary>
    /// 通常レティクル3番のみアクティブ
    /// </summary>
    public void Change3()
    {

        //サイズ変更
        _nomalRet.transform.localScale = new Vector2(1.8f, 1.8f);

        //レティクルのアクティブ変更
        _nomalRet2.SetActive(true);
        _nomalRet3.SetActive(true);
    }

}
