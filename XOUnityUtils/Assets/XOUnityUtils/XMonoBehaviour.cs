using UnityEngine;
using System.Collections;

public abstract class XMonoBehaviour : MonoBehaviour {
    [System.Flags]
    protected enum XUpdateKind {
        Update = 1,
        FixedUpdate = 2,
        LateUpdate = 4
    }
    
    [System.Flags]
    protected enum XSetupKind {
        Reset = 1,
        Awake = 2
    }
    
    [System.Flags]
    protected enum XReadyKind {
        OnEnable = 1,
        Start = 2
    }
    
    protected XUpdateKind m_UpdateKind = XUpdateKind.Update;
    protected XSetupKind m_SetupKind = XSetupKind.Reset | XSetupKind.Awake;
    protected XReadyKind m_ReadyKind = XReadyKind.Start;

    protected virtual void XSetup(XSetupKind kind) {}
    protected virtual void XReady(XReadyKind kind) {}
    
    protected virtual void XEnable() {}
    protected virtual void XDisable() {}
    protected virtual void XDestroy() {}
    protected virtual void XUpdate(XUpdateKind kind) {}
    protected virtual void XExit() {}
    
    #if UNITY_EDITOR
    protected virtual void XDrawGizmos(){}
    #endif

    // lifecycle
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Reset() {
        if((m_SetupKind & XSetupKind.Reset) == XSetupKind.Reset)
            XSetup(XSetupKind.Reset);
    }
    
    private void Awake() {
        if((m_SetupKind & XSetupKind.Awake) == XSetupKind.Awake)
            XSetup(XSetupKind.Reset);
    }
    
    private void OnEnable() {
        if((m_ReadyKind & XReadyKind.OnEnable) == XReadyKind.OnEnable)
            XReady(XReadyKind.OnEnable);
        XEnable();
    }
    
    private void Start() {
        if((m_ReadyKind & XReadyKind.Start) == XReadyKind.Start)
            XReady(XReadyKind.Start);
    }
    
    private void OnDisable() {
        XDisable();
    }
    
    private void OnApplicationExit() {
        XExit();
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        XDrawGizmos();
        // reset gizmos
        Gizmos.color = Color.white;
        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
    
    private void FixedUpdate() {
        if((m_UpdateKind & XUpdateKind.FixedUpdate) == XUpdateKind.FixedUpdate)
            XUpdate(XUpdateKind.FixedUpdate);
    }
    
    private void Update() {
        if((m_UpdateKind & XUpdateKind.Update) == XUpdateKind.Update)
            XUpdate(XUpdateKind.Update);
    }
    
    private void LateUpdate() {
        if((m_UpdateKind & XUpdateKind.LateUpdate) == XUpdateKind.LateUpdate)
            XUpdate(XUpdateKind.LateUpdate);
    }
    
    private void OnDestroy() {
        XDestroy();
    }
    
    // Component Lifecycle
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [System.Flags]
    protected enum FindComponentMethod {
        OnSelf,
        OnChild,
        OnParent,
        OnSelfOrChild = OnSelf | OnChild,
        OnSelfOrParent = OnSelf | OnParent,
        OnAny = OnSelf | OnChild | OnParent,
    }
    
    protected enum FindNamedMethod {
        Tag,
        Name
    }
    
    protected T Find<T>(string namedRef, FindNamedMethod namedMethod = FindNamedMethod.Tag) where T:UnityEngine.Component {
        if(namedMethod == FindNamedMethod.Tag) {
            return GameObject.FindWithTag(namedRef).GetComponent<T>();   
        }
        else {
            return GameObject.Find(namedRef).GetComponent<T>();
        }
    }
    
    protected T Find<T>(FindComponentMethod findMethod = FindComponentMethod.OnSelf, bool includeInactive = true) where T:UnityEngine.Component {
        T component = null;
        if((findMethod & FindComponentMethod.OnSelf) == FindComponentMethod.OnSelf) {
            component = this.GetComponent<T>();    
        }
        if(component == null && (findMethod & FindComponentMethod.OnChild) == FindComponentMethod.OnChild) {
            component = this.GetComponentInChildren<T>(includeInactive);
        }
        if(component == null && (findMethod & FindComponentMethod.OnParent) == FindComponentMethod.OnParent) {
            Transform t = transform.parent;
            while(t && component == null) {
                component = t.GetComponent<T>();
                t = t.parent;
            }
        }
        return component; 
    }
    
    protected T Require<T>(FindComponentMethod findMethod = FindComponentMethod.OnSelf, bool includeInactive = true) where T:UnityEngine.Component {
        T component = Find<T>(findMethod, includeInactive);
#if UNITY_EDITOR
        ErrorIfNull(component);
        return component;
#else
        return component;
#endif
    }
    
    protected T Require<T>(string namedRef, FindNamedMethod namedMethod = FindNamedMethod.Tag) where T:UnityEngine.Component {
        T component = Find<T>(namedRef, namedMethod);
#if UNITY_EDITOR
        ErrorIfNull(component);
        return component;
#else
        return component;
#endif
    }
    
    protected void Require<T>(ref T existingRef, FindComponentMethod findMethod = FindComponentMethod.OnSelf, bool includeInactive = true) where T:UnityEngine.Component {
        if(existingRef == null) {
            existingRef = Find<T>(findMethod, includeInactive);
            ErrorIfNull(existingRef);        
        }
    }
    
    protected void Require<T>(ref T existingRef, string namedRef, FindNamedMethod namedMethod = FindNamedMethod.Tag) where T:UnityEngine.Component {
        if(existingRef == null) {
            existingRef = Find<T>(namedRef, namedMethod);
            ErrorIfNull(existingRef);
        }
    }
    
    protected void ErrorIfNull<T>(T component) where T:UnityEngine.Object {
#if UNITY_EDITOR
        if(component == null) {
            Debug.LogErrorFormat("[{0}<b>{1}<<color=blue>{2}</color>></b>] requires an object of type <<b><color=blue>{3}</color></b>>", this.GetParentName(), this.name, this.GetTypeName(), DebugExtensions.GetStringExt(typeof(T)));
        }
#endif
    }
    
    protected void ErrorIfNotNull<T>(T component)
    {
#if UNITY_EDITOR
        if(component != null) {
            Debug.LogErrorFormat("[{0}<b>{1}<<color=blue>{2}</color>></b>] requires component of type <<b><color=blue>{3}</color></b>> to be null.", this.GetParentName(), this.name, this.GetTypeName(), DebugExtensions.GetStringExt(typeof(T)));
        }
#endif   
    }
    
    protected void SafeDestroy<T>(ref T component) where T:UnityEngine.Object {
        if(component != null) {
            Destroy(component);
            component = null;
        }
    }
}
