using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    private LineRenderer lr;                                                        //����Ⱦ��.

    private Transform collidersParent;                                              //��ײ��ĸ�����.

    public Transform startPos;                                                     //�����.
    public Transform endPos;                                                       //���յ�.
    private Vector3 controlPos;                                                     //���Ƶ�.

    private Vector3 tempStartPos;
    private Vector3 tempEndPos;

    [SerializeField]
    private float offset = 1.0f;                                                    //���Ƶ�ƫ����.
    private int segment = 20;                                                       //�ߵĶ���.
    private float lineWidth = 0.2f;                                                 //�ߵĿ��.

    private List<GameObject> colliders = new List<GameObject>();                     //��ײ�����弯��.
    private void Awake()
    {

        lr = gameObject.GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
        }

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        collidersParent = GameObject.Find("Colliders").transform;

        startPos = GameObject.Find("Gate1_ImageTarget").transform;
        endPos = GameObject.Find("Gate2_ImageTarget").transform;
    }

    private void Update()
    {
        //ֻҪλ�øı�����»�������.
        if (tempStartPos != startPos.position || tempEndPos != endPos.position)
        {

            //�������ߵ�.
            this.controlPos = CalcControlPos(startPos.position, endPos.position, offset);

            //��������.
            Vector3[] poses = DrawWire(this.controlPos, this.segment);

            //�����ײ��.
            AttackCollider(poses, collidersParent, lineWidth);

            tempStartPos = startPos.position;
            tempEndPos = endPos.position;
        }

    }

    private void OnDrawGizmos()
    {
        if (endPos == null)
        {
            Awake();
        }
        //����(����ʼ��ָ���յ�)
        Vector3 dir = endPos.position - startPos.position;
        //ȡ����һ������. ����ȡ����.
        Vector3 otherDir = Vector3.up;

        //��ƽ�淨��.  ע��otherDir��dir���ܵ���λ��,ƽ��ķ������з����,(����λ�ûᵼ�·��߷����෴)
        //ps: ��������ϵʹ�����ֶ��� ��������ϵʹ�����ֶ��� (����ʲô������������ϵ���ﲻϸ˵��Google)
        //unity����������ʹ�õ�����������ϵ,���Է��ߵķ���Ӧ�������ֶ����ж�.
        Vector3 planeNormal = Vector3.Cross(otherDir, dir);

        //����startPos��endPos�Ĵ���. ��ʵ��������һ�β��.
        Vector3 vertical = Vector3.Cross(dir, planeNormal).normalized;
        //�е�.
        Vector3 centerPos = (startPos.position + endPos.position) / 2f;
        //���Ƶ�.
        Vector3 controlPos = centerPos + vertical * offset;

        //�߶�.
        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPos.position, endPos.position);

        //ƽ�淨��.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(centerPos, controlPos + planeNormal.normalized * 5);

        //��Ϊȷ���ķ���.
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPos.position, startPos.position + otherDir.normalized * 5);

        //�д���.
        Gizmos.color = Color.green;
        Gizmos.DrawLine(centerPos, centerPos + vertical.normalized * 5);
    }

    /// <summary>
    /// ��ȡ���Ƶ�.
    /// </summary>
    /// <param name="startPos">���.</param>
    /// <param name="endPos">�յ�.</param>
    /// <param name="offset">ƫ����.</param>
    private Vector3 CalcControlPos(Vector3 startPos, Vector3 endPos, float offset)
    {
        //����(����ʼ��ָ���յ�)
        Vector3 dir = endPos - startPos;
        //ȡ����һ������. ����ȡ����. ͻ�߳���forward�� back�� up/left�� down/right�� 
        Vector3 otherDir = Vector3.back;

        //��ƽ�淨��.  ע��otherDir��dir���ܵ���λ��,ƽ��ķ������з����,(����λ�ûᵼ�·��߷����෴)
        //ps: ��������ϵʹ�����ֶ��� ��������ϵʹ�����ֶ��� (����ʲô������������ϵ���ﲻϸ˵��Google)
        //unity����������ʹ�õ�����������ϵ,���Է��ߵķ���Ӧ�������ֶ����ж�.
        Vector3 planeNormal = Vector3.Cross(otherDir, dir);

        //����startPos��endPos�Ĵ���. ��ʵ��������һ�β��.
        Vector3 vertical = Vector3.Cross(dir, planeNormal).normalized;
        //�е�.
        Vector3 centerPos = (startPos + endPos) / 2f;
        //���Ƶ�.
        Vector3 controlPos = centerPos + vertical * offset;

        return controlPos;
    }

    /// <summary>
    /// ��������.
    /// </summary>
    private Vector3[] DrawWire(Vector3 controlPos, int segments)
    {
        Vector3[] bezierPoses = BezierUtils.GetBeizerList(startPos.position, controlPos, endPos.position, segments);

        lr.positionCount = bezierPoses.Length;
        for (int i = 0; i <= bezierPoses.Length - 1; i++)
        {
            lr.SetPosition(i, bezierPoses[i]);
        }
        return bezierPoses;
    }

    /// <summary>
    /// ���������ײ��.
    /// </summary>
    /// <param name="poses">�㼯��.</param>
    /// <param name="colls">��ײ���ĸ�����.</param>
    /// <param name="radius">�뾶.</param>
    private void AttackCollider(Vector3[] poses, Transform colls, float radius)
    {
        Vector3 lastPos = poses[0];
        for (int i = 1; i < poses.Length; i++)
        {
            Vector3 nextPos = poses[i];

            GameObject colliderObj = null;
            if (i <= colliders.Count - 1)
            {
                colliderObj = colliders[i - 1];
            }
            else
            {
                colliderObj = new GameObject();
                colliders.Add(colliderObj);
            }

            colliderObj.name = (i - 1).ToString();
            colliderObj.transform.parent = colls;
            colliderObj.transform.forward = (nextPos - lastPos).normalized;

            CapsuleCollider coll = colliderObj.GetComponent<CapsuleCollider>();
            if (coll == null)
            {
                coll = colliderObj.AddComponent<CapsuleCollider>();
            }
            Vector3 center = (lastPos + nextPos) / 2f;

            //���ý��������.
            colliderObj.transform.position = center;
            coll.center = Vector3.zero;
            coll.radius = radius;
            coll.height = Vector3.Distance(lastPos, nextPos);
            coll.direction = 2;                         //0-X 1-Y 2-Z
            coll.tag = "Wire";

            lastPos = nextPos;
        }
    }
}