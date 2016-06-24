using UnityEngine;
using System.Text;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

public static class DebugExtensions
{   
    // in: "some.text.with.dots"
    // out: "dots"
    public static string GetStringExt<T>(T stringable)
    {
        var asStr = stringable.ToString();
        return asStr.Substring(asStr.LastIndexOf('.')+1);    
    } 
    
    public static string GetTypeName(this System.Object o, bool shortName= true)
    {
        string fullname = o.GetType().ToString();
        if(!shortName)
            return fullname;
        return GetStringExt(fullname);
    }
    
    // Todo: itterate and use stringbuilder instead of recursion.
    public static string GetParentName(this Transform transform)
    {
        if(transform.parent != null)
            return transform.parent.GetName() + "/";
        return transform.name;
    }

    public static string GetName(this Transform transform)
    {
        if(transform.parent != null)
            return transform.parent.GetName() + "/" + transform.name;
        return transform.name;
    }
 
    public static string GetName(this UnityEngine.GameObject go)
    {
        return go.transform.GetName();
    }
    
    public static string GetName(this UnityEngine.MonoBehaviour behaviour)
    {
        return behaviour.transform.GetName();
    }
    
    public static string GetParentName(this UnityEngine.GameObject go)
    {
        return go.transform.GetParentName();
    }
    
    public static string GetParentName(this UnityEngine.MonoBehaviour behaviour)
    {
        return behaviour.transform.GetParentName();
    }
}