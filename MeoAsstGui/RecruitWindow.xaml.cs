﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MeoAsstGui
{
    using AsstMsg = MainWindow.AsstMsg;
    /// <summary>
    /// RecruitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RecruitWindow : Window
    {
        [DllImport("MeoAssistance.dll")] static private extern bool AsstCatchEmulator(IntPtr ptr);
        [DllImport("MeoAssistance.dll")] static private extern bool AsstRunOpenRecruit(IntPtr ptr, int[] required_level, bool set_time);

        private IntPtr p_asst;
        public RecruitWindow(IntPtr ptr)
        {
            InitializeComponent();
            p_asst = ptr;
        }
        public void proc_msg(AsstMsg msg, JObject detail)
        {
            switch (msg)
            {
                case AsstMsg.TextDetected:
                    break;
                case AsstMsg.RecruitTagsDetected:
                    JArray tags = (JArray)detail["tags"];
                    string info_content = "识别结果:    ";
                    foreach (var tag_name in tags)
                    {
                        info_content += tag_name.ToString() + "    ";
                    }
                    info.Content = info_content;
                    break;
                case AsstMsg.OcrResultError:
                    info.Content = "识别错误！";
                    break;
                case AsstMsg.RecruitSpecialTag:
                    MessageBox.Show("检测到特殊Tag:" + detail["tag"].ToString(), "提示");
                    break;
                case AsstMsg.RecruitResult:
                    string result_content = "";
                    JArray result_array = (JArray)detail["result"];
                    foreach (var combs in result_array)
                    {
                        result_content += (int)combs["tag_level"] + "星Tags:  ";
                        foreach (var tag in (JArray)combs["tags"])
                        {
                            result_content += tag.ToString() + "    ";
                        }
                        result_content += "\n\t";
                        foreach (var oper in (JArray)combs["opers"])
                        {
                            result_content += oper["level"].ToString() + " - " + oper["name"].ToString() + "    ";
                        }
                        result_content += "\n\n";
                    }
                    result.Content = result_content;
                    break;
            }
        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            info.Content = "正在识别……";
            result.Content = "";

            if (!AsstCatchEmulator(p_asst))
            {
                return;
            }
            List<int> level_list = new List<int>();

            if (checkBox_level_3.IsChecked == true)
            {
                level_list.Add(3);
            }
            if (checkBox_level_4.IsChecked == true)
            {
                level_list.Add(4);
            }
            if (checkBox_level_5.IsChecked == true)
            {
                level_list.Add(5);
            }
            if (checkBox_level_6.IsChecked == true)
            {
                level_list.Add(6);
            }
            bool set_time = checkBox_time.IsChecked == true ? true : false;

            AsstRunOpenRecruit(p_asst, level_list.ToArray(), set_time);
        }
    }
}
