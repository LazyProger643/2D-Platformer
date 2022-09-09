#if UNITY_EDITOR
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace LazyProger.ScenesConstantNames
{
    public static class ScenesGenerator
    {
        public static string Generate(List<string> sceneNameList)
        {
            var targetClass = new CodeTypeDeclaration(ScenesSettings.ClassName)
            {
                TypeAttributes = System.Reflection.TypeAttributes.Class | System.Reflection.TypeAttributes.Public,
                Attributes = MemberAttributes.Static
            };

            foreach (var sceneName in sceneNameList)
            {
                CodeMemberField field = new CodeMemberField(new CodeTypeReference(typeof(string)), sceneName)
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    InitExpression = new CodePrimitiveExpression(sceneName)
                };

                targetClass.Members.Add(field);
            }

            var targetNamespace = new CodeNamespace(ScenesSettings.Namespace);
            targetNamespace.Types.Add(targetClass);

            var targetUnit = new CodeCompileUnit();
            targetUnit.Namespaces.Add(targetNamespace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions { BracingStyle = "C" };

            var code = new StringWriter();
            provider.GenerateCodeFromCompileUnit(targetUnit, code, options);

            return code.ToString();
        }
    }
}
#endif
