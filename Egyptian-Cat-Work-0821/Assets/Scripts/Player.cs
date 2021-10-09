using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    #region ���
    [Header("���ʳt��"), Range(0, 1000)]
    public float speed = 500.0f;
    [Header("���D����"), Range(0, 3000)]
    public int jump = 500;
    [Header("��q"), Range(0, 200)]
    public float hp = 100;
    [Header("�O�_�b�a�O�W"), Tooltip("�Ψ��x�s����O�_�b�a�O�W����T�A�b�a�O�W true�A���b�a�O�W false")]
    public bool isGround;
    [Header("���O"), Range(0.01f, 1)]
    public float gravity = 0.1f;
    [Header("�����N�o"), Range(0, 5)]
    public float cd = 0.5f;

    /// <summary>
    /// �����p�ɾ�
    /// </summary>
    private float timer;
    /// <summary>
    /// �O�_����
    /// </summary>
    private bool isAttack;

    [Header("�����O"), Range(0, 1000)]
    public float attack = 20;
    [Header("���`�ƥ�")]
    public UnityEvent onDead;
    [Header("���İϰ�")]
    public AudioClip soundJump;
    public AudioClip soundAttack;

    // �p�H��줣���
    // �}���ݩʭ��O�����Ҧ� Debug �i�H�ݨ�p�H���
    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;
    /// <summary>
    /// ���a������J��
    /// </summary>
    private float hValue;
    #endregion

    #region �ƥ�
    /// <summary>
    /// ��r��q
    /// </summary>
    private Text textHp;
    /// <summary>
    /// ���
    /// </summary>
    private Image imgHp;
    /// <summary>
    /// ��q�̤j��
    /// </summary>
    private float hpMax;

    [Header("�����ϰ쪺�첾�P�j�p")]
    public Vector2 checkAttackOffset;
    public Vector3 checkAttackSize;

    private void Start()
    {
        // GetComponent<����>() �x����k�A�i�H���w��������
        // �@�ΡG���o������ 2D ���餸��
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        hpMax = hp;
        textHp = GameObject.Find("��r��q").GetComponent<Text>();
        imgHp = GameObject.Find("���").GetComponent<Image>();
    }

    // �@�������� 60 ��
    private void Update()
    {
        GetPlayerInputHorizontal();
        TurnDirection();
        Jump();
        Attack();
        Fall();
    }

    // �T�w��s�ƥ�
    // �@���T�w���� 50 ���A�x���ĳ���ϥΨ쪫�z API �n�b���ƥ󤺰���
    private void FixedUpdate()
    {
        Move(hValue);
    }

    [Header("�ˬd�a�O�ϰ�G�첾�P�b�|")]
    public Vector3 groundOffset;
    [Range(0, 2)]
    public float groundRadius = 0.1f;

    // ø�s�ϥܡG���U�}�o�̥ΡA�ȷ|��ܦb�s�边 Unity ��
    private void OnDrawGizmos()
    {
        // ���M�w�C��Aø�s�ϥ�
        Gizmos.color = new Color(1, 0, 0, 0.3f);    // �b�z������
        // ø�s�y��(�����I, �b�|)
        Gizmos.DrawSphere(transform.position + groundOffset, groundRadius);

        Gizmos.color = new Color(0.2f, 0.3f, 0.1f, 0.3f);
        Gizmos.DrawCube(transform.position +
            transform.right * checkAttackOffset.x +
            transform.up * checkAttackOffset.y,
            checkAttackSize);
    }

    #endregion

    #region ��k

    /// <summary>
    /// ���o���a��J�����b�V�ȡGA�BD�B���B�k
    /// </summary>
    private void GetPlayerInputHorizontal()
    {
        // ������ = ��J.���o�b�V(�b�V�W��)
        // �@�ΡG���o���a���U�������䪺�ȡA���k�� 1 �B������ -1 �B�S���� 0
        hValue = Input.GetAxis("Horizontal");
        // print("���a�����ȡG" + hValue);
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Move(float horizontal)
    {
        /** �Ĥ@�ز��ʤ覡�G�ۭq���O...
        // �ϰ��ܼơG�b��k�������A���ϰ�ʡA�ȭ��󦹤�k���s��
        // transform ������ Transform �ܧΤ���
        // posMove = ������e�y�� + ���a��J��������
        // Time.fixedDeltaTime �� 1/50 ��
        Vector2 posMove = transform.position + new Vector3(horizontal, -gravity, 0) * speed * Time.fixedDeltaTime;
        // ����.���ʮy��(�n�e�����y��)
        rig.MovePosition(posMove);
        */

        /** �ĤG�ز��ʤ覡�G�ϥαM�פ������O - ���w�C */
        rig.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rig.velocity.y);

        ani.SetBool("�����}��", horizontal != 0);
    }

    /// <summary>
    /// �����V�G�B�z���⭱�V���D�A���k���� 0 �A�������� 180
    /// </summary>
    private void TurnDirection()
    {
        // print("���a���U�k�G" + Input.GetKeyDown(KeyCode.D));
        // �p�G ���a�� D �N�N���׳]�� 0, 0, 0
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.eulerAngles = Vector3.zero;
        }
        // �p�G ���a�� A �N�N���׳]�� 0, 180, 0
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }

    /// <summary>
    /// ���D
    /// </summary>
    private void Jump()
    {
        // Vector2 �Ѽƥi�H�ϥ� Vector3 �N�J�A�{���|�۰ʧ� Z �b����
        // << �첾�B��l
        // ���w�ϼh�y�k�G1 << �ϼh�s��
        Collider2D hit = Physics2D.OverlapCircle(transform.position + groundOffset, groundRadius, 1 << 6);

        // �p�G �I�쪫��s�b �N�N���b�a���W �_�h �N�N�����A�a���W
        // �P�_���p�G�u�� �@�ӵ����Ÿ��F �i�H�ٲ��j�A��
        if (hit)
        {
            isGround = true;
            // print("�I�쪺����G" + hit.name);
        }
        else
        {
            isGround = false;
        }

        ani.SetBool("���DĲ�o", !isGround);

        // �p�G ���a ���U �ť��� ����N���W���D
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rig.AddForce(new Vector2(0, jump));
            aud.PlayOneShot(soundJump, Random.Range(0.7f, 1.1f));
        }
    }

    private void Attack()
    {
        // �p�G ���U ���� �Ұ�Ĳ�o�Ѽ�
        // �p�G ���O������ �åB ���U ���� �~�i�H���� �Ұ�Ĳ�o�Ѽ�
        if (!isAttack && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttack = true;
            ani.SetTrigger("����Ĳ�o");
            aud.PlayOneShot(soundAttack, Random.Range(0.7f, 1.1f));

            // �P�w�����ϰ�O�_������ 8 ���ĤH�ϼh����
            Collider2D hit = Physics2D.OverlapBox(transform.position +
                transform.right * checkAttackOffset.x +
                transform.up * checkAttackOffset.y,
                checkAttackSize, 0, 1 << 8);
            if (hit)
            {
                hit.GetComponent<BaseEnemy>().Hurt(attack);
                // StartCoroutine(cameraControl.ShakeEffect());
            }
        }
        // �p�G ���U����������N�}�l�֥[�ɶ�
        if (isAttack)
        {
            if (timer < cd)
            {
                timer += Time.deltaTime;
                // print("������֥[�ɶ��G" + timer);
            }
            else
            {
                timer = 0;
                isAttack = false;
            }
        }
    }

    private void Fall()
    {
        if (transform.position.y < -15)
        {
            hp = 0;
            textHp.text = "Hp" + hp;
            imgHp.fillAmount = hp / hpMax;
            Dead();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="damage"></param>
    public void Hurt(float damage)
    {
        hp -= damage;           // ��q�����ˮ`��

        if (hp <= 0) Dead();    // �p�G ��q <= 0 �N ���`

        textHp.text = "Hp" + hp;
        imgHp.fillAmount = hp / hpMax;
    }

    /// <summary>
    /// ���`
    /// </summary>
    private void Dead()
    {
        hp = 0;                         // ��q�k�s
        ani.SetBool("���`�}��", true);   // ���`�ʵe
        onDead.Invoke();                // �I�s���`�ƥ�
        enabled = false;                // ���}���}��
    }

    /// <summary>
    /// �Y�D��
    /// </summary>
    /// <param name="propName"></param>
    private void EatProp(string propName)
    {
        switch (propName)
        {
            case "�t�Q�r���p":
                Destroy(goPropHit);                 // �R��(����,����ɶ�)
                hp += 10;
                hp = Mathf.Clamp(hp, 0, hpMax);     // ��s����
                textHp.text = "Hp" + hp;
                imgHp.fillAmount = hp / hpMax;
                break;
            case "�t�Q�r����":
                Destroy(goPropHit);
                hp += 20;
                hp = Mathf.Clamp(hp, 0, hpMax);
                textHp.text = "Hp" + hp;
                imgHp.fillAmount = hp / hpMax;
                break;
            case "�t�Q�r���j":
                Destroy(goPropHit);
                hp += 30;
                hp = Mathf.Clamp(hp, 0, hpMax);
                textHp.text = "Hp" + hp;
                imgHp.fillAmount = hp / hpMax;
                break;
            case "�t���ά��j":
                Destroy(goPropHit);
                break;
            case "�t���ά���":
                Destroy(goPropHit);
                break;
            case "�t���ά��p":
                Destroy(goPropHit);
                break;
            default:
                break;
        }
    }
    #endregion

    private GameObject goPropHit;

    // �I���ƥ�G
    // 1. ��ӸI�����󳣭n�� collider
    // 2. �åB�䤤�@�ӭn�� Rigidbody
    // 3. ��ӳ��S���Ŀ� Is Trigger
    // Enter �ƥ�G�I���}�l�ɰ���@��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        goPropHit = collision.gameObject;
        EatProp(collision.gameObject.name);
    }
}