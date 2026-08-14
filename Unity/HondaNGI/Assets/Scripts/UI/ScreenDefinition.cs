using System; using System.Collections.Generic; using System.Globalization;
public sealed class ScreenDefinition {
 public string Id,Title,Style; public IReadOnlyList<WidgetDefinition> Widgets;
 public static ScreenDefinition FromJson(string json){var r=AddonJson.Obj(AddonJson.Parse(json));if(r==null)return null;return new ScreenDefinition{Id=AddonJson.Str(r,"id"),Title=AddonJson.Str(r,"title"),Style=AddonJson.Str(r,"style"),Widgets=WidgetDefinition.List(r.TryGetValue("widgets",out var w)?w:null)};}
}
public sealed class WidgetDefinition {
 public string Type,Id,StyleClass; public WidgetLayout Layout; public IReadOnlyDictionary<string,object> Props; public IReadOnlyList<WidgetDefinition> Children;
 public static WidgetDefinition From(Dictionary<string,object> o){if(o==null)return null;return new WidgetDefinition{Type=AddonJson.Str(o,"type"),Id=AddonJson.Str(o,"id"),StyleClass=AddonJson.Str(o,"styleClass"),Layout=WidgetLayout.From(o.TryGetValue("layout",out var l)?AddonJson.Obj(l):null),Props=o.TryGetValue("props",out var p)?AddonJson.Obj(p)??new Dictionary<string,object>():new Dictionary<string,object>(),Children=List(o.TryGetValue("children",out var c)?c:null)};}
 public static IReadOnlyList<WidgetDefinition> List(object raw){var a=AddonJson.Arr(raw);var r=new List<WidgetDefinition>();if(a!=null)foreach(var x in a){var n=From(AddonJson.Obj(x));if(n!=null)r.Add(n);}return r;}
 public string PropString(string k,string f="")=>Props!=null&&Props.TryGetValue(k,out var v)&&v!=null?Convert.ToString(v,CultureInfo.InvariantCulture):f;
 public float PropFloat(string k,float f=0){try{return Props!=null&&Props.TryGetValue(k,out var v)?Convert.ToSingle(v,CultureInfo.InvariantCulture):f;}catch{return f;}}
 public int PropInt(string k,int f=0){try{return Props!=null&&Props.TryGetValue(k,out var v)?Convert.ToInt32(v,CultureInfo.InvariantCulture):f;}catch{return f;}}
 public string[] PropStringArray(string k){if(Props==null||!Props.TryGetValue(k,out var v))return null;var a=AddonJson.Arr(v);if(a==null)return null;var r=new string[a.Count];for(int i=0;i<a.Count;i++)r[i]=Convert.ToString(a[i],CultureInfo.InvariantCulture);return r;}
}
public sealed class WidgetLayout { public int X,Y,Width=1,Height=1; public static WidgetLayout From(Dictionary<string,object> o)=>o==null?null:new WidgetLayout{X=AddonJson.Int(o,"x"),Y=AddonJson.Int(o,"y"),Width=AddonJson.Int(o,"width",1),Height=AddonJson.Int(o,"height",1)}; }
