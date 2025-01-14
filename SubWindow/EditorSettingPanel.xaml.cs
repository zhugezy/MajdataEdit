﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using WPFLocalizeExtension.Engine;

namespace MajdataEdit
{
    /// <summary>
    /// BPMtap.xaml 的交互逻辑
    /// </summary>
    public partial class EditorSettingPanel : Window
    {
        private readonly string[] langList = new string[3] { "zh-CN", "en-US", "ja" }; // 语言列表
        private bool saveFlag = false;
        private bool dialogMode;

        public EditorSettingPanel(bool _dialogMode = false)
        {
            this.dialogMode = _dialogMode;
            InitializeComponent();

            if (dialogMode)
            {
                Cancel_Button.IsEnabled = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string curLang = ((MainWindow)Owner).editorSetting.Language;
            int boxIndex = -1;
            for (int i = 0; i < langList.Length; i++)
            {
                if (curLang == langList[i])
                {
                    boxIndex = i;
                    break;
                }
            }

            if (boxIndex == -1)
            {
                // 如果没有语言设置 或者语言未知 就自动切换到English
                boxIndex = 1;
            }
            LanguageComboBox.SelectedIndex = boxIndex;
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //LanguageComboBox.SelectedIndex
            LocalizeDictionary.Instance.Culture = new CultureInfo(langList[LanguageComboBox.SelectedIndex]);
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            saveFlag = true;
            ((MainWindow)Owner).editorSetting.Language = langList[LanguageComboBox.SelectedIndex];
            ((MainWindow)Owner).SaveEditorSetting();
            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!saveFlag)
            {
                // 取消或直接关闭窗口
                if (dialogMode) {
                    // 模态窗口状态下 则阻止关闭
                    e.Cancel = true;
                    MessageBox.Show(MainWindow.GetLocalizedString("NoEditorSetting"), MainWindow.GetLocalizedString("Error"));
                }
                else
                {
                    LocalizeDictionary.Instance.Culture = new CultureInfo(((MainWindow)Owner).editorSetting.Language);
                }
            }
            else
            {
                if (dialogMode)
                {
                    this.DialogResult = true;
                }
            }
        }
    }
}
