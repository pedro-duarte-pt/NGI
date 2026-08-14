using System; using System.Collections.Generic; using System.Globalization; using System.Text;
public static class AddonJson {
 public static object Parse(string json)=>new Parser(json).Value();
 public static Dictionary<string,object> Obj(object v)=>v as Dictionary<string,object>;
 public static List<object> Arr(object v)=>v as List<object>;
 public static string Str(Dictionary<string,object> o,string k,string f="")=>o!=null&&o.TryGetValue(k,out var v)&&v!=null?Convert.ToString(v,CultureInfo.InvariantCulture):f;
 public static int Int(Dictionary<string,object> o,string k,int f=0){try{return o!=null&&o.TryGetValue(k,out var v)?Convert.ToInt32(v,CultureInfo.InvariantCulture):f;}catch{return f;}}
 sealed class Parser {
  readonly string s; int i; public Parser(string s){this.s=s??"";}
  public object Value(){Skip();if(i>=s.Length)return null;char c=s[i];if(c=='{')return Object();if(c=='[')return Array();if(c=='"')return String();if(M("true")){i+=4;return true;}if(M("false")){i+=5;return false;}if(M("null")){i+=4;return null;}return Number();}
  Dictionary<string,object> Object(){var d=new Dictionary<string,object>(StringComparer.OrdinalIgnoreCase);i++;Skip();if(P('}')){i++;return d;}while(true){Skip();string k=String();Skip();R(':');i++;d[k]=Value();Skip();if(P('}')){i++;break;}R(',');i++;}return d;}
  List<object> Array(){var a=new List<object>();i++;Skip();if(P(']')){i++;return a;}while(true){a.Add(Value());Skip();if(P(']')){i++;break;}R(',');i++;}return a;}
  string String(){R('"');i++;var b=new StringBuilder();while(i<s.Length){char c=s[i++];if(c=='"')break;if(c!='\\'){b.Append(c);continue;}if(i>=s.Length)break;c=s[i++];switch(c){case'"':b.Append('"');break;case'\\':b.Append('\\');break;case'/':b.Append('/');break;case'b':b.Append('\b');break;case'f':b.Append('\f');break;case'n':b.Append('\n');break;case'r':b.Append('\r');break;case't':b.Append('\t');break;case'u':if(i+4<=s.Length){b.Append((char)int.Parse(s.Substring(i,4),NumberStyles.HexNumber,CultureInfo.InvariantCulture));i+=4;}break;}}return b.ToString();}
  object Number(){int a=i;while(i<s.Length&&"-+0123456789.eE".IndexOf(s[i])>=0)i++;string n=s.Substring(a,i-a);if(n.Length==0)throw new FormatException("Invalid JSON at "+i);if(n.IndexOfAny(new[]{'.','e','E'})>=0)return double.Parse(n,CultureInfo.InvariantCulture);return long.Parse(n,CultureInfo.InvariantCulture);}
  void Skip(){while(i<s.Length&&char.IsWhiteSpace(s[i]))i++;}bool P(char c)=>i<s.Length&&s[i]==c;bool M(string t)=>i+t.Length<=s.Length&&string.Compare(s,i,t,0,t.Length,StringComparison.Ordinal)==0;void R(char c){if(!P(c))throw new FormatException("Expected '"+c+"' at "+i);}
 }}
