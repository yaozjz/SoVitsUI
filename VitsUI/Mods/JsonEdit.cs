using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using YamlDotNet.RepresentationModel;

namespace VitsUI.Mods
{
    internal class JsonEdit
    {
        public static JObject LoadJsonData(string jsonPath)
        {
            string json = File.ReadAllText(jsonPath);
            JObject objs = JObject.Parse(json);

            return objs;
        }

        // 修改JSON文件的按钮点击事件处理程序  
        public static void WirteJson(string jsonPath, JObject jobject)
        {
            string convertString = Convert.ToString(jobject);//将json装换为string
            File.WriteAllText(jsonPath, convertString);
        }

        public static YamlMappingNode LoadYaml(string yamlPath)
        {
            var yaml = new YamlStream();
            yaml.Load(new StringReader(File.ReadAllText(yamlPath)));
            // 获取YAML文件中的数据  
            var data = yaml.Documents[0].RootNode as YamlMappingNode;
            return data;
        }

        public static void WriteYaml(string yamlPath, YamlMappingNode yaml)
        {
            var yamlStream = new YamlStream();
            using (StreamWriter writer = new StreamWriter(yamlPath))
            {
                yamlStream.Add(new YamlDocument(yaml));
                yamlStream.Save(writer, false);
            }
            var yaml_text = File.ReadAllText(yamlPath);
            yaml_text = Regex.Replace(yaml_text, @"\.\.\.(\r?\n)", "");
            Debug.WriteLine(yaml_text);
            File.WriteAllText(yamlPath, yaml_text);
        }

    }
}
