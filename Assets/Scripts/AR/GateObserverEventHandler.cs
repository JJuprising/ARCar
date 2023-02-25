/*==============================================================================
Copyright (c) 2021 PTC Inc. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
==============================================================================*/

using System;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class GateObserverEventHandler : MonoBehaviour
{

    public enum TrackingStatusFilter
    {
        Tracked,
        Tracked_ExtendedTracked,
        Tracked_ExtendedTracked_Limited
    }

    /// <summary>
    /// A filter that can be set to either:
    /// - Only consider a target if it's in view (TRACKED)
    /// - Also consider the target if's outside of the view, but the environment is tracked (EXTENDED_TRACKED)
    /// - Even consider the target if tracking is in LIMITED mode, e.g. the environment is just 3dof tracked.
    /// </summary>
    public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked_ExtendedTracked_Limited;

    public bool UsePoseSmoothing = false;
    public AnimationCurve AnimationCurve = AnimationCurve.Linear(0, 0, LERP_DURATION, 1);

    public UnityEvent OnTargetFound;
    public UnityEvent OnTargetLost;


    protected ObserverBehaviour mObserverBehaviour;
    protected TargetStatus mPreviousTargetStatus = TargetStatus.NotObserved;
    protected bool mCallbackReceivedOnce;

    const float LERP_DURATION = 0.3f;

    PoseSmoother mPoseSmoother;

    protected virtual void Start()
    {
        mObserverBehaviour = GetComponent<ObserverBehaviour>();

        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnObserverStatusChanged;
            mObserverBehaviour.OnBehaviourDestroyed += OnObserverDestroyed;

            OnObserverStatusChanged(mObserverBehaviour, mObserverBehaviour.TargetStatus);
            SetupPoseSmoothing();
        }
    }

    protected virtual void OnDestroy()
    {
        if (VuforiaBehaviour.Instance != null)
            VuforiaBehaviour.Instance.World.OnStateUpdated -= OnStateUpdated;

        if (mObserverBehaviour)
            OnObserverDestroyed(mObserverBehaviour);

        mPoseSmoother?.Dispose();
    }

    void OnObserverDestroyed(ObserverBehaviour observer)
    {
        mObserverBehaviour.OnTargetStatusChanged -= OnObserverStatusChanged;
        mObserverBehaviour.OnBehaviourDestroyed -= OnObserverDestroyed;
        mObserverBehaviour = null;
    }

    void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        var name = mObserverBehaviour.TargetName;
        if (mObserverBehaviour is VuMarkBehaviour vuMarkBehaviour && vuMarkBehaviour.InstanceId != null)
        {
            name += " (" + vuMarkBehaviour.InstanceId + ")";
        }

        Debug.Log($"Target status: {name} {targetStatus.Status} -- {targetStatus.StatusInfo}");

        HandleTargetStatusChanged(mPreviousTargetStatus.Status, targetStatus.Status);
        HandleTargetStatusInfoChanged(targetStatus.StatusInfo);

        mPreviousTargetStatus = targetStatus;
    }

    protected virtual void HandleTargetStatusChanged(Status previousStatus, Status newStatus)
    {
        var shouldBeRendererBefore = ShouldBeRendered(previousStatus);
        var shouldBeRendererNow = ShouldBeRendered(newStatus);
        if (shouldBeRendererBefore != shouldBeRendererNow)
        {
            if (shouldBeRendererNow)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }
        else
        {
            if (!mCallbackReceivedOnce && !shouldBeRendererNow)
            {
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
                OnTrackingLost();
            }
        }

        mCallbackReceivedOnce = true;
    }

    protected virtual void HandleTargetStatusInfoChanged(StatusInfo newStatusInfo)
    {
        if (newStatusInfo == StatusInfo.WRONG_SCALE)
        {
            Debug.LogErrorFormat("The target {0} appears to be scaled incorrectly. " +
                                 "This might result in tracking issues. " +
                                 "Please make sure that the target size corresponds to the size of the " +
                                 "physical object in meters and regenerate the target or set the correct " +
                                 "size in the target's inspector.", mObserverBehaviour.TargetName);
        }
    }

    protected bool ShouldBeRendered(Status status)
    {
        if (status == Status.TRACKED)
        {
            // always render the augmentation when status is TRACKED, regardless of filter
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked && status == Status.EXTENDED_TRACKED)
        {
            // also return true if the target is extended tracked
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited &&
            (status == Status.EXTENDED_TRACKED || status == Status.LIMITED))
        {
            // in this mode, render the augmentation even if the target's tracking status is LIMITED.
            // this is mainly recommended for Anchors.
            return true;
        }

        return false;
    }
    public void SummonBoxes() 
    {
        Vector3 GateVec = transform.position;
        //�洢���ߺ��ӵ�����
        var Box1Pos = GateVec;
        var Box2Pos = GateVec;
        var Box3Pos = GateVec;
        //���ɵ��ߺ���
        GameObject GoldBox1 = Instantiate(Resources.Load("GoldBox", typeof(GameObject)), transform) as GameObject;
        GameObject GoldBox2 = Instantiate(Resources.Load("GoldBox", typeof(GameObject)), transform) as GameObject;
        GameObject GoldBox3 = Instantiate(Resources.Load("GoldBox", typeof(GameObject)), transform) as GameObject;
        //���õ��ߺе�λ��
        //1�ź���
        Box1Pos.y = (float)(GateVec.y - 0.6);//����
        Box1Pos.x = (float)(GateVec.x - 0.8);//����
        GoldBox1.transform.position = Box1Pos;
        //2�ź���
        Box2Pos.y = (float)(GateVec.y - 0.6);//����
        GoldBox2.transform.position = Box2Pos;
        //3�ź���
        Box3Pos.y = (float)(GateVec.y - 0.6);//����
        Box3Pos.x = (float)(GateVec.x + 0.8);//����
        GoldBox3.transform.position = Box3Pos;
        //��ӱ�ǩ
        GoldBox1.tag = "GoldBox";
        GoldBox2.tag = "GoldBox";
        GoldBox3.tag = "GoldBox";
        //�����ת�ű�
        GoldBox1.AddComponent<spin>();
        GoldBox2.AddComponent<spin>();
        GoldBox3.AddComponent<spin>();
    }
    public void SummonCoins()
    {
        Vector3 GateVec = transform.position;
        //ʶ��gate3��,���ɽ��
        GameObject Coin = Instantiate(Resources.Load("Coin", typeof(GameObject)), transform) as GameObject;
        var CoinPos = GateVec - Vector3.up * 0.6f;
        Coin.transform.position = CoinPos;
        //���ñ�ǩ
        Coin.tag = "Coin";
        //�����ת�ű�
        Coin.AddComponent<spin>();
    }
    protected virtual void OnTrackingFound()
    {
        if (mObserverBehaviour)
        {
            var rendererComponents = mObserverBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mObserverBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mObserverBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
            {
                component.enabled = true;

            }
            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;
        }
        
        //ʶ�����������
        var name = mObserverBehaviour.TargetName;
        Debug.Log(name);
        //ʶ�����嵱ǰλ��
        //Vector3 GateVec = transform.position;

        switch (name)
        {
            case "gate1":
                Debug.Log("ʶ��1����");
                if (StaticData.GateObserved[0] == 0)//���ɿ�ʼװ�ε�
                {
                    if (TrialMgr.Instance.gate1 != null)
                    {
                        
                    }
                }

                StaticData.GateObserved[0] ++;//NO.0 was marked
                
                
                //���ɿ�ʼ����(����ʱ)��todo
                break;
            case "gate2":
                
                Debug.Log("ʶ��2����");
                //ʶ��gate2��,���ɵ��ߺ���
                if (StaticData.GateObserved[1]==0)
                {
                    SummonBoxes();
                }
                //��¼ʶ��״̬
                StaticData.GateObserved[1]++;//NO.1 was marked
                if (TrialMgr.Instance.gate2 != null)
                {
                    
                }
                break;
            case "gate3":
                if (StaticData.GateObserved[2]==0)
                {
                    SummonCoins();
                }
                //��¼ʶ��״̬
                StaticData.GateObserved[2]++;//NO.2 was marked
                if (TrialMgr.Instance.gate3 != null)
                {
                    
                }
                break;
            case "gate4":
                //��¼ʶ��״̬
                StaticData.GateObserved[3]++;//NO.3 was marked
                //ʶ��gate4��
                if (TrialMgr.Instance.gate4 != null)
                {
                    
                }
                break;
            default:
                break;
        }
       

        OnTargetFound?.Invoke();
    }

    protected virtual void OnTrackingLost()
    {
        //
        if (mObserverBehaviour)
        {
            var rendererComponents = mObserverBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mObserverBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mObserverBehaviour.GetComponentsInChildren<Canvas>(true);

            //// Disable rendering:
            //foreach (var component in rendererComponents)
            //    component.enabled = false;

            //// Disable colliders:
            //foreach (var component in colliderComponents)
            //    component.enabled = false;

            //// Disable canvas':
            //foreach (var component in canvasComponents)
            //    component.enabled = false;
        }

        OnTargetLost?.Invoke();
    }

    protected void SetupPoseSmoothing()
    {
        UsePoseSmoothing &= VuforiaBehaviour.Instance.WorldCenterMode == WorldCenterMode.DEVICE; // pose smoothing only works with the DEVICE world center mode
        mPoseSmoother = new PoseSmoother(mObserverBehaviour, AnimationCurve);

        VuforiaBehaviour.Instance.World.OnStateUpdated += OnStateUpdated;
    }

    void OnStateUpdated()
    {
        if (enabled && UsePoseSmoothing)
            mPoseSmoother.Update();
    }

    class PoseSmoother
    {
        const float e = 0.001f;
        const float MIN_ANGLE = 2f;

        PoseLerp mActivePoseLerp;
        Pose mPreviousPose;

        readonly ObserverBehaviour mTarget;
        readonly AnimationCurve mAnimationCurve;

        TargetStatus mPreviousStatus;

        public PoseSmoother(ObserverBehaviour target, AnimationCurve animationCurve)
        {
            mTarget = target;
            mAnimationCurve = animationCurve;
        }

        public void Update()
        {
            var currentPose = new Pose(mTarget.transform.position, mTarget.transform.rotation);
            var currentStatus = mTarget.TargetStatus;

            UpdatePoseSmoothing(currentPose, currentStatus);

            mPreviousPose = currentPose;
            mPreviousStatus = currentStatus;
        }

        void UpdatePoseSmoothing(Pose currentPose, TargetStatus currentTargetStatus)
        {
            if (mActivePoseLerp == null && ShouldSmooth(currentPose, currentTargetStatus))
            {
                mActivePoseLerp = new PoseLerp(mPreviousPose, currentPose, mAnimationCurve);
            }

            if (mActivePoseLerp != null)
            {
                var pose = mActivePoseLerp.GetSmoothedPosition(Time.deltaTime);
                mTarget.transform.SetPositionAndRotation(pose.position, pose.rotation);

                if (mActivePoseLerp.Complete)
                {
                    mActivePoseLerp = null;
                }
            }
        }

        /// Smooth pose transition if the pose changed and the target is still being reported as "extended tracked" or it has just returned to
        /// "tracked" from previously being "extended tracked"
        bool ShouldSmooth(Pose currentPose, TargetStatus currentTargetStatus)
        {
            return (currentTargetStatus.Status == Status.EXTENDED_TRACKED || (currentTargetStatus.Status == Status.TRACKED && mPreviousStatus.Status == Status.EXTENDED_TRACKED)) &&
                   (Vector3.SqrMagnitude(currentPose.position - mPreviousPose.position) > e || Quaternion.Angle(currentPose.rotation, mPreviousPose.rotation) > MIN_ANGLE);
        }

        public void Dispose()
        {
            mActivePoseLerp = null;
        }
    }

    class PoseLerp
    {
        readonly AnimationCurve mCurve;
        readonly Pose mStartPose;
        readonly Pose mEndPose;
        readonly float mEndTime;

        float mElapsedTime;

        public bool Complete { get; private set; }

        public PoseLerp(Pose startPose, Pose endPose, AnimationCurve curve)
        {
            mStartPose = startPose;
            mEndPose = endPose;
            mCurve = curve;
            mEndTime = mCurve.keys[mCurve.length - 1].time;
        }

        public Pose GetSmoothedPosition(float deltaTime)
        {
            mElapsedTime += deltaTime;

            if (mElapsedTime >= mEndTime)
            {
                mElapsedTime = 0;
                Complete = true;
                return mEndPose;
            }

            var ratio = mCurve.Evaluate(mElapsedTime);
            var smoothPosition = Vector3.Lerp(mStartPose.position, mEndPose.position, ratio);
            var smoothRotation = Quaternion.Slerp(mStartPose.rotation, mEndPose.rotation, ratio);

            return new Pose(smoothPosition, smoothRotation);
        }
    }
}