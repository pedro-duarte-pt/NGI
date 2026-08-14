using System;
using UnityEngine;
using UnityEngine.UIElements;
public static class AddonStyleApplier {
 static bool H(OptionalFloat v)=>v!=null&&v.HasValue;
 public static void Apply(VisualElement e,AddonStyleProperties s){
  if(e==null||s==null)return; Reset(e);
  if(ColorUtility.TryParseHtmlString(s.backgroundColor,out var bg))e.style.backgroundColor=bg;
  if(ColorUtility.TryParseHtmlString(s.color,out var fg))e.style.color=fg;
  if(H(s.fontSize))e.style.fontSize=s.fontSize.value;
  if(!string.IsNullOrWhiteSpace(s.fontWeight))e.style.unityFontStyleAndWeight=s.fontWeight.Equals("bold",StringComparison.OrdinalIgnoreCase)?FontStyle.Bold:FontStyle.Normal;
  if(!string.IsNullOrWhiteSpace(s.textAlign))e.style.unityTextAlign=TA(s.textAlign);
  if(H(s.width))e.style.width=s.width.value;if(H(s.height))e.style.height=s.height.value;if(H(s.minWidth))e.style.minWidth=s.minWidth.value;if(H(s.minHeight))e.style.minHeight=s.minHeight.value;
  if(H(s.paddingLeft))e.style.paddingLeft=s.paddingLeft.value;if(H(s.paddingRight))e.style.paddingRight=s.paddingRight.value;if(H(s.paddingTop))e.style.paddingTop=s.paddingTop.value;if(H(s.paddingBottom))e.style.paddingBottom=s.paddingBottom.value;
  if(H(s.marginLeft))e.style.marginLeft=s.marginLeft.value;if(H(s.marginRight))e.style.marginRight=s.marginRight.value;if(H(s.marginTop))e.style.marginTop=s.marginTop.value;if(H(s.marginBottom))e.style.marginBottom=s.marginBottom.value;
  if(ColorUtility.TryParseHtmlString(s.borderColor,out var bc)){e.style.borderLeftColor=bc;e.style.borderRightColor=bc;e.style.borderTopColor=bc;e.style.borderBottomColor=bc;}
  if(H(s.borderWidth)){float v=s.borderWidth.value;e.style.borderLeftWidth=v;e.style.borderRightWidth=v;e.style.borderTopWidth=v;e.style.borderBottomWidth=v;}
  if(H(s.borderRadius)){float v=s.borderRadius.value;e.style.borderTopLeftRadius=v;e.style.borderTopRightRadius=v;e.style.borderBottomLeftRadius=v;e.style.borderBottomRightRadius=v;}
  if(!string.IsNullOrWhiteSpace(s.layoutDirection))e.style.flexDirection=s.layoutDirection.Equals("row",StringComparison.OrdinalIgnoreCase)?FlexDirection.Row:FlexDirection.Column;
  if(!string.IsNullOrWhiteSpace(s.alignItems))e.style.alignItems=A(s.alignItems);if(!string.IsNullOrWhiteSpace(s.justifyContent))e.style.justifyContent=J(s.justifyContent);
  if(H(s.flexGrow))e.style.flexGrow=s.flexGrow.value;if(H(s.opacity))e.style.opacity=s.opacity.value;
 }
 static void Reset(VisualElement e){
  e.style.backgroundColor=StyleKeyword.Null;e.style.color=StyleKeyword.Null;e.style.fontSize=StyleKeyword.Null;e.style.unityFontStyleAndWeight=StyleKeyword.Null;e.style.unityTextAlign=StyleKeyword.Null;
  e.style.paddingLeft=StyleKeyword.Null;e.style.paddingRight=StyleKeyword.Null;e.style.paddingTop=StyleKeyword.Null;e.style.paddingBottom=StyleKeyword.Null;
  e.style.marginLeft=StyleKeyword.Null;e.style.marginRight=StyleKeyword.Null;e.style.marginTop=StyleKeyword.Null;e.style.marginBottom=StyleKeyword.Null;
  e.style.borderLeftColor=StyleKeyword.Null;e.style.borderRightColor=StyleKeyword.Null;e.style.borderTopColor=StyleKeyword.Null;e.style.borderBottomColor=StyleKeyword.Null;
  e.style.borderLeftWidth=StyleKeyword.Null;e.style.borderRightWidth=StyleKeyword.Null;e.style.borderTopWidth=StyleKeyword.Null;e.style.borderBottomWidth=StyleKeyword.Null;
  e.style.borderTopLeftRadius=StyleKeyword.Null;e.style.borderTopRightRadius=StyleKeyword.Null;e.style.borderBottomLeftRadius=StyleKeyword.Null;e.style.borderBottomRightRadius=StyleKeyword.Null;
  e.style.flexDirection=StyleKeyword.Null;e.style.alignItems=StyleKeyword.Null;e.style.justifyContent=StyleKeyword.Null;e.style.opacity=StyleKeyword.Null;
  // Intentionally preserve width/height/minWidth/minHeight/flexGrow: current layout/runtime ownership.
 }
 static Align A(string v)=>v=="center"?Align.Center:v=="flex-end"?Align.FlexEnd:v=="stretch"?Align.Stretch:Align.FlexStart;
 static Justify J(string v)=>v=="center"?Justify.Center:v=="flex-end"?Justify.FlexEnd:v=="space-between"?Justify.SpaceBetween:Justify.FlexStart;
 static TextAnchor TA(string v){switch(v){case"upper-center":return TextAnchor.UpperCenter;case"upper-right":return TextAnchor.UpperRight;case"middle-left":return TextAnchor.MiddleLeft;case"middle-center":return TextAnchor.MiddleCenter;case"middle-right":return TextAnchor.MiddleRight;case"lower-left":return TextAnchor.LowerLeft;case"lower-center":return TextAnchor.LowerCenter;case"lower-right":return TextAnchor.LowerRight;default:return TextAnchor.UpperLeft;}}
}
