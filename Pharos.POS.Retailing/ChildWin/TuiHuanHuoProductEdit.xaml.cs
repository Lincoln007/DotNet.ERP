﻿using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pharos.Wpf.Extensions;
using Pharos.Wpf.HotKeyHelper;
using Pharos.Wpf.Controls;
using Pharos.POS.Retailing.Models.ApiReturnResults;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// TuiHuanHuoProductEdit.xaml 的交互逻辑
    /// </summary>
    public partial class TuiHuanHuoProductEdit : DialogWindow02
    {
        ChangingList item;
        Window tempMainwindow;
        public TuiHuanHuoProductEdit(ChangingList model, int mode)
        {
            item = model;
            tempMainwindow = item.CurrentWindow;
            model.tempNumber = model.ChangeNumber;
            InitializeComponent();
            //  this.InitDefualtSettings();
            this.ApplyBindings(this, model);
            this.ApplyHotKeyBindings();
            CurrentModel = model;
            if (mode == 0)
            {
                FocusControl = txtprice;
            }
            else
            {
                FocusControl = txtnum;

            }
            this.PreviewKeyDown += _this_PreviewKeyDown;
            this.Loaded += _this_Loaded;
            this.Closed += TuiHuanHuoProductEdit_Closed;
        }

        void TuiHuanHuoProductEdit_Closed(object sender, EventArgs e)
        {
            item.CurrentWindow = tempMainwindow;
        }
        void _this_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(FocusControl);
        }

        void _this_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var win = sender as Window;
            switch (e.Key)
            {
                case Key.Escape:
                    win.Close();
                    break;
                case Key.Y:
                    txtprice.Focus();
                    txtnum.Focus();
                    CurrentModel.Confirm.Execute(null);
                    e.Handled = true;
                    break;
                case Key.Add:
                case Key.Up:
                case Key.PageUp:
                case Key.VolumeUp:
                    CurrentModel.NumAdd.Execute(null);
                    e.Handled = true;
                    break;
                case Key.Down:
                case Key.PageDown:
                case Key.Subtract:
                case Key.VolumeDown:
                    CurrentModel.NumDec.Execute(null);
                    e.Handled = true;
                    break;
            }
        }
        ChangingList CurrentModel { get; set; }

        internal IInputElement FocusControl { get; private set; }

    }
}