using System;
using System.Collections.Generic;
public sealed class AddonStyleResolver
{
    readonly Dictionary<string,AddonStyleRuleDefinition> rules=new(StringComparer.OrdinalIgnoreCase);
    readonly Dictionary<string,string> tokens=new(StringComparer.OrdinalIgnoreCase);
    public AddonStyleResolver(AddonStyleSheetDefinition sheet) {
        if(sheet==null)return;
        if(sheet.tokens!=null) foreach(var t in sheet.tokens) if(t!=null&&!string.IsNullOrWhiteSpace(t.name)) tokens[t.name]=t.value;
        if(sheet.styles!=null) foreach(var r in sheet.styles) if(r!=null&&!string.IsNullOrWhiteSpace(r.name)) rules[r.name]=r;
    }
    public AddonStyleProperties Resolve(IEnumerable<string> names,IEnumerable<string> activeStates) {
        var result=new AddonStyleProperties(); if(names==null)return result;
        var states=activeStates==null?new HashSet<string>(StringComparer.OrdinalIgnoreCase):new HashSet<string>(activeStates,StringComparer.OrdinalIgnoreCase);
        foreach(var n in names) if(rules.TryGetValue(n,out var r)) {
            Merge(result,r.style);
            if(r.states!=null) foreach(var s in r.states) if(s!=null&&!string.IsNullOrWhiteSpace(s.name)&&states.Contains(s.name)) Merge(result,s.style);
        }
        result.backgroundColor=Token(result.backgroundColor); result.color=Token(result.color); result.borderColor=Token(result.borderColor);
        return result;
    }
    string Token(string v){ if(string.IsNullOrWhiteSpace(v)||v[0]!='$')return v; return tokens.TryGetValue(v.Substring(1),out var x)?x:v; }
    static void C(OptionalFloat s,ref OptionalFloat t){if(s!=null&&s.HasValue)t=OptionalFloat.Of(s.value);}
    static void Merge(AddonStyleProperties t,AddonStyleProperties s){
        if(s==null)return;
        if(!string.IsNullOrWhiteSpace(s.backgroundColor))t.backgroundColor=s.backgroundColor;
        if(!string.IsNullOrWhiteSpace(s.color))t.color=s.color; C(s.fontSize,ref t.fontSize);
        if(!string.IsNullOrWhiteSpace(s.fontWeight))t.fontWeight=s.fontWeight;
        if(!string.IsNullOrWhiteSpace(s.textAlign))t.textAlign=s.textAlign;
        C(s.width,ref t.width);C(s.height,ref t.height);C(s.minWidth,ref t.minWidth);C(s.minHeight,ref t.minHeight);
        C(s.paddingLeft,ref t.paddingLeft);C(s.paddingRight,ref t.paddingRight);C(s.paddingTop,ref t.paddingTop);C(s.paddingBottom,ref t.paddingBottom);
        C(s.marginLeft,ref t.marginLeft);C(s.marginRight,ref t.marginRight);C(s.marginTop,ref t.marginTop);C(s.marginBottom,ref t.marginBottom);
        if(!string.IsNullOrWhiteSpace(s.borderColor))t.borderColor=s.borderColor; C(s.borderWidth,ref t.borderWidth);C(s.borderRadius,ref t.borderRadius);
        if(!string.IsNullOrWhiteSpace(s.layoutDirection))t.layoutDirection=s.layoutDirection;
        if(!string.IsNullOrWhiteSpace(s.alignItems))t.alignItems=s.alignItems;
        if(!string.IsNullOrWhiteSpace(s.justifyContent))t.justifyContent=s.justifyContent;
        C(s.flexGrow,ref t.flexGrow);C(s.opacity,ref t.opacity);
    }
}
