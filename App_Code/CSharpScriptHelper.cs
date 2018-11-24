using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Microsoft.CSharp;

public class CSharpScriptHelper
{
    public static string RunScriptFile(string csScriptFileName, string fullClassName, string enterMethod)
    {
        var script = File.ReadAllText(csScriptFileName);
        return RunScript(script, fullClassName, enterMethod);
    }
    public static string RunScript(string script, string fullClassName, string enterMethod)
    {
        var objCSharpCodePrivoder = new CSharpCodeProvider();
        var objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();
        var objCompilerParameters = new CompilerParameters();
        objCompilerParameters.ReferencedAssemblies.Add("System.dll");
        objCompilerParameters.GenerateExecutable = false;
        objCompilerParameters.GenerateInMemory = true;

        var cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, script);
        if (cr.Errors.HasErrors)
        {
            var sb = new StringBuilder();
            sb.Append("Compiler Error：");
            foreach (CompilerError err in cr.Errors)
            {
                sb.AppendLine(err.ErrorText);
            }
            throw new Exception(sb.ToString());
        }
        else
        {
            var objAssembly = cr.CompiledAssembly;
            var obj = objAssembly.CreateInstance(fullClassName);
            var mi = obj.GetType().GetMethod(enterMethod);
            mi.Invoke(obj, null);
            return string.Empty;
        }
    }
    public static string EvaluateExpression(string exp)
    {
        var sb = new StringBuilder();
        sb.Append("using System;");
        sb.Append(Environment.NewLine);
        sb.Append("namespace DynamicCode");
        sb.Append(Environment.NewLine);
        sb.Append("{");
        sb.Append(Environment.NewLine);
        sb.Append("    public class Provider");
        sb.Append(Environment.NewLine);
        sb.Append("    {");
        sb.Append(Environment.NewLine);
        sb.Append("        public string Compute()");
        sb.Append(Environment.NewLine);
        sb.Append("        {");
        sb.Append(Environment.NewLine);
        sb.Append(string.Format("            return ({0}).ToString();", exp));
        sb.Append(Environment.NewLine);
        sb.Append("        }");
        sb.Append(Environment.NewLine);
        sb.Append("    }");
        sb.Append(Environment.NewLine);
        sb.Append("}");

        var code = sb.ToString();

        // 1.CSharpCodePrivoder  
        var objCSharpCodePrivoder = new CSharpCodeProvider();

        // 2.ICodeComplier  
        var objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

        // 3.CompilerParameters  
        var objCompilerParameters = new CompilerParameters();
        objCompilerParameters.ReferencedAssemblies.Add("System.dll");
        objCompilerParameters.GenerateExecutable = false;
        objCompilerParameters.GenerateInMemory = true;

        var cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, code);
        if (cr.Errors.HasErrors)
        {
            var sbe = new StringBuilder();
            sbe.Append("Compiler Error：");
            foreach (CompilerError err in cr.Errors)
            {
                sbe.AppendLine(err.ErrorText);
            }
            throw new Exception(sbe.ToString());
        }
        else
        {
            // 通过反射，调用方法
            var objAssembly = cr.CompiledAssembly;
            var obj = objAssembly.CreateInstance("DynamicCode.Provider");
            var mi = obj.GetType().GetMethod("Compute");
            var s = mi.Invoke(obj, null);
            return s.ToString();
        }
    }
}

