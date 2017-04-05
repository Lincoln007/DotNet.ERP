﻿using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Pharos.POS.Retailing.Models.ApiReturnResults;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// TuiHuanHuo.xaml 的交互逻辑
    /// </summary>
    public partial class TuiHuanHuo : DialogWindow02, IBarcodeControl, IPosDataGrid
    {
        public TuiHuanHuo()
            : this(0)
        {
        }
        /// <summary>
        /// 换货=0，退货=1，退单=2
        /// </summary>
        /// <param name="index"></param>
        public TuiHuanHuo(int index)
        {
            InitializeComponent();
            this.InitDefualtSettings();
            CurrentModel = new RefundChangeViewModel();
            CurrentModel.Change.CurrentWindow = this;
            CurrentModel.Refund.CurrentWindow = this;
            CurrentModel.RefundOrder.CurrentWindow = this;
            this.ApplyBindings(this, CurrentModel);
            this.Closing += TuiHuanHuo_Closing;
            this.PreviewKeyDown += TuiHuanHuo_PreviewKeyDown;
            SetTabItemShow(index);
            this.Loaded += TuiHuanHuo_Loaded;
            this.Activated += TuiHuanHuo_Activated;
        }

        void TuiHuanHuo_Activated(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0 || tabControl.SelectedIndex == 1)
            {
                if (tabControl.SelectedIndex == 0)
                    Keyboard.Focus(txtChangeBarcode);
                if (tabControl.SelectedIndex == 1)
                    Keyboard.Focus(txtRefundBarcode);

            }
        }
        private void TuiHuanHuo_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           // this.ThreadFilterMessage();
            Keyboard.Focus(CurrentIInputElement);

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(300);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Keyboard.Focus(CurrentIInputElement);
                }));
            });

        }
        public void SetInputFocus(int tabItem, int inputIndex)
        {
            switch (tabItem)
            {
                default:
                case 0:
                    switch (inputIndex)
                    {
                        case 0:
                        default:
                            Keyboard.Focus(ChangeOldOrderSn);
                            break;
                        case 1:
                            //Keyboard.Focus(ChangeSaleman);
                            break;
                        case 2:
                            Keyboard.Focus(txtChangeBarcode);
                            break;
                        case 3:
                            Keyboard.Focus(ChangePwd);
                            break;
                    }
                    break;
                case 1:
                    switch (inputIndex)
                    {
                        case 0:
                        default:
                            Keyboard.Focus(RefundOldOrderSn);
                            break;
                        case 1:
                            //Keyboard.Focus(RefundSaleman);
                            break;
                        case 2:
                            Keyboard.Focus(txtRefundBarcode);
                            break;
                        case 3:
                            Keyboard.Focus(RefundPwd);
                            break;
                    }
                    break;
                case 3:
                    switch (inputIndex)
                    {
                        case 0:
                        default:
                            Keyboard.Focus(txtRefundAllBarcode);
                            break;
                        case 2:
                            Keyboard.Focus(RefundAllPwd);
                            break;
                    }
                    break;

            }
        }
        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    //   HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
        //    HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);

        //    if (source != null)
        //    {
        //        source.AddHook(this.WndProc);
        //    }

        //    base.OnSourceInitialized(e);
        //}
        void TuiHuanHuo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (tabControl.SelectedIndex == 0 || tabControl.SelectedIndex == 1)
            {
                switch (e.SystemKey)
                {
                    case Key.F10:
                        switch (tabControl.SelectedIndex)
                        {
                            case 0:
                            default:
                                switch (CurrentModel.Change.Mode)
                                {
                                    case Models.PosModels.ChangeInputMode.New:
                                        CurrentModel.Change.Mode = Models.PosModels.ChangeInputMode.Old;
                                        txtChangeBarcode.Focus();
                                        break;
                                    case Models.PosModels.ChangeInputMode.Old:
                                        CurrentModel.Change.Mode = Models.PosModels.ChangeInputMode.New;
                                        txtChangeBarcode.Focus();
                                        break;
                                }
                                break;
                            case 1:
                                txtRefundBarcode.Focus();
                                break;
                        }
                        e.Handled = true;
                        break;
                    case Key.Escape:
                        this.Close();
                        break;
                }
                if (e.Key == Key.Delete)
                {
                    if (CurrentGrid.SelectedItem != null)
                    {
                        var product = (ChangingList)CurrentGrid.SelectedItem;
                        product.RemoveCommand.Execute(null);
                        e.Handled = true;
                    }
                }
            }

        }
        public DataGrid CurrentGrid { get; set; }

        public System.Windows.IInputElement CurrentIInputElement { get; set; }
        RefundChangeViewModel CurrentModel { get; set; }


        internal void SetTabItemShow(int index)
        {
            tabControl.SelectedIndex = index;
            switch (index)
            {
                case 0:
                    CurrentIInputElement = ChangeOldOrderSn;
                    CurrentGrid = HUANHUOGRID;
                    break;
                case 1:
                    CurrentIInputElement = RefundOldOrderSn;
                    CurrentGrid = TUIHUOGRID;
                    break;
                case 2:
                    CurrentIInputElement = txtRefundAllBarcode;
                    CurrentGrid = null;
                    break;
            }
            if (CurrentIInputElement != null)//处理快捷键因界面刷新失效，延时处理焦点
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        Keyboard.Focus(CurrentIInputElement);
                    }));
                });
            }
        }


        void TuiHuanHuo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                CurrentModel.Change.Clear();
                CurrentModel.Refund.Clear();
            });

        }




        private void txtMode_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (CurrentModel.Change.Mode)
            {
                case Models.PosModels.ChangeInputMode.New:
                    CurrentModel.Change.Mode = Models.PosModels.ChangeInputMode.Old;
                    break;
                case Models.PosModels.ChangeInputMode.Old:
                    CurrentModel.Change.Mode = Models.PosModels.ChangeInputMode.New;
                    break;
            }
        }

        private void IconTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var ctrl = sender as TextBox;
            if (e.Key == Key.Enter && string.IsNullOrEmpty(ctrl.Text.Trim()) && !ctrl.IsReadOnly)
            {
                ctrl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                ctrl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                Keyboard.Focus(ctrl);
            }


        }

        private void TabItemHeader_TuiHuo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentIInputElement != txtRefundBarcode)
                SetTabItemShow(1);
        }

        private void TabItemHeader_HuanHuo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentIInputElement != txtRefundBarcode)
                SetTabItemShow(0);
        }
        private void TabItemHeader_TuiDan_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentIInputElement != txtRefundBarcode)
                SetTabItemShow(2);
        }


        //private void refund_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (CurrentIInputElement != txtRefundBarcode)
        //        SetTabItemShow(1);
        //}

        //private void refunAll_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (CurrentIInputElement != txtRefundAllBarcode)
        //        SetTabItemShow(2);
        //}

        //private void change_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (CurrentIInputElement != txtChangeBarcode)
        //        SetTabItemShow(0);
        //}




    }
}