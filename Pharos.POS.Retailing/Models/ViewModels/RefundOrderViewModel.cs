﻿using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.ChildWin.Pay;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.Printer;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class RefundOrderViewModel : BaseViewModel
    {
        public RefundOrderViewModel()
        {
            OrderList = new List<BillDetails>();
            IsOperatEnabled = true;
            this.OnPropertyChanged(o => o.IsOperatEnabled);
            Task.Factory.StartNew(() =>
            {
                #region 退换货理由
                //取退换货理由
                //ReasonParams _params = new ReasonParams()
                //{
                //    StoreId = _machinesInfo.StoreId,
                //    MachineSn = _machinesInfo.MachineSn,
                //    CID = _machinesInfo.CompanyId,
                //    Type = (int)ChangeStatus.Refund
                //};
                //var result = ApiManager.Post<ReasonParams, ApiRetrunResult<IEnumerable<ApiReasonResult>>>(@"api/GetReason", _params);
                //if (result.Code == "200")
                //{
                //    CurrentWindow.Dispatcher.Invoke(new Action(() =>
                //    {
                //        ReasonItem = result.Result;
                //        var first = ReasonItem.FirstOrDefault();
                //        if (first != null)
                //            Reason = first.DicSN;
                //    }));
                //}
                //else
                //{
                //    Toast.ShowMessage(result.Message, CurrentWindow);
                //} 
                #endregion
                //取当前流水号
                BaseApiParams baseParams = new BaseApiParams() { StoreId = _machinesInfo.StoreId, MachineSn = _machinesInfo.MachineSn, CID = _machinesInfo.CompanyId };
                var currentCustomSnInfo = ApiManager.Post<BaseApiParams, ApiRetrunResult<string>>(@"api/GetRefundAllOrderSn", baseParams);
                if (currentCustomSnInfo.Code == "200")
                {
                    CurrentWindow.Dispatcher.Invoke(new Action(() =>
                    {
                        CurrentReturnOrderSn = currentCustomSnInfo.Result;
                    }));
                }
                else
                {
                    CurrentWindow.Dispatcher.Invoke(new Action(() =>
                    {
                        Toast.ShowMessage(currentCustomSnInfo.Message, CurrentWindow);
                    }));
                }
            });
        }

        MachineInformations _machinesInfo = Global.MachineSettings.MachineInformations;

        string password = string.Empty;
        public string Password { get { return password; } set { password = value; this.OnPropertyChanged(o => o.Password); } }
        /// <summary>
        /// 退单新流水号
        /// </summary>
        public string CurrentReturnOrderSn { get; set; }

        /// <summary>
        /// 导购员
        /// </summary>
        public string SaleMan { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        private decimal preferentialAmount;

        public decimal PreferentialAmount
        {
            get { return preferentialAmount; }
            set { preferentialAmount = value; }
        }
        /// <summary>
        /// 订单时间
        /// </summary>
        private DateTime orderTime;

        public DateTime OrderTime
        {
            get { return orderTime; }
            set { orderTime = value; }
        }

        private string oldSaleMan;
        /// <summary>
        /// 原导购员
        /// </summary>
        public string OldSaleMan
        {
            get { return oldSaleMan; }
            set { oldSaleMan = value; this.OnPropertyChanged(o => o.OldSaleMan); }
        }
        /// <summary>
        /// 支付流水号
        /// </summary>
        private string paySn;

        public string PaySn
        {
            get { return paySn; }
            set
            {
                QueryModel.Current.IsQuery = true;
                if (value == null)
                    value = "";
                //条码变动时加载数据
                paySn = value.Trim().ToUpper();
                if (!string.IsNullOrEmpty(paySn))
                {
                    Task.Factory.StartNew(() =>
                    {

                        FindBillHistoryParams _params = new FindBillHistoryParams()
                        {
                            StoreId = _machinesInfo.StoreId,
                            MachineSn = _machinesInfo.MachineSn,
                            CID = _machinesInfo.CompanyId,
                            PaySn = paySn
                        };
                        var result = ApiManager.Post<FindBillHistoryParams, ApiRetrunResult<BillHistoryInfo>>(@"api/FindBillHistory", _params);
                        if (result.Code == "200")
                        {
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                if (result.Result.OrderType == 0)
                                {
                                    if (!result.Result.IsRefundOrder)
                                    {
                                        SaleMan = result.Result.SaleManUserCode + result.Result.SaleManName;
                                        OrderList = result.Result.Details;
                                        Difference = -result.Result.WipeZeroAfterTotalAmount;
                                        paySn = result.Result.PaySn;
                                        PreferentialAmount = result.Result.PreferentialAmount;
                                        OrderTime = result.Result.OrderTime;
                                        this.OnPropertyChanged(o => o.PaySn);
                                    }
                                    else
                                    {
                                        Toast.ShowMessage("该流水号已发生退换操作历史！", CurrentWindow);
                                        var page = CurrentWindow as TuiHuanHuo;
                                        if (page != null)
                                            page.SetInputFocus(3, 0);
                                    }
                                }
                                else
                                {
                                    Toast.ShowMessage("无法退单，该流水号为非销售单 或不存在！", CurrentWindow);
                                    var page = CurrentWindow as TuiHuanHuo;
                                    if (page != null)
                                        page.SetInputFocus(3, 0);
                                }
                                QueryModel.Current.IsQuery = false;

                            }));

                        }
                        else
                        {
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                Toast.ShowMessage(result.Message, CurrentWindow);
                                paySn = string.Empty;
                                var page = CurrentWindow as TuiHuanHuo;
                                if (page != null)
                                    page.SetInputFocus(3, 0);
                                this.OnPropertyChanged(o => o.PaySn);
                            }));

                        }
                    });
                }
                else
                {
                    paySn = string.Empty;
                    this.OnPropertyChanged(o => o.PaySn);
                }
            }
        }
        /// <summary>
        /// 变更列表
        /// </summary>
        private IEnumerable<BillDetails> orderList;

        public IEnumerable<BillDetails> OrderList
        {
            get { return orderList; }
            set
            {
                orderList = value;
                this.OnPropertyChanged(o => o.OrderList);
            }
        }
        /// <summary>
        /// 理由列表
        /// </summary>
        private IEnumerable<ApiReasonResult> reasonItem;

        public IEnumerable<ApiReasonResult> ReasonItem
        {
            get { return reasonItem; }
            set { reasonItem = value; this.OnPropertyChanged(o => o.ReasonItem); }
        }
        int _Reason;
        /// <summary>
        /// 选中的退换货理由
        /// </summary>
        public int Reason
        {
            get { return _Reason; }
            set
            {
                _Reason = value;
                this.OnPropertyChanged(o => o.Reason);
            }
        }

        /// <summary>
        /// 差价
        /// </summary>
        private decimal difference;

        public decimal Difference
        {
            get { return difference; }
            set { difference = value; this.OnPropertyChanged(o => o.Difference); }
        }
        /// <summary>
        /// 确认事件
        /// </summary>
        public GeneralCommand<object> ConfirmCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    IsOperatEnabled = false;
                    this.OnPropertyChanged(o => o.IsOperatEnabled);
                    //调确认的接口
                    if (OrderList.Count() > 0)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            if (string.IsNullOrEmpty(Password))
                            {
                                CurrentWindow.Dispatcher.Invoke(new Action(() =>
                                    {
                                        Toast.ShowMessage("请输入授权密码！", CurrentWindow);
                                        var page = CurrentWindow as TuiHuanHuo;
                                        if (page != null)
                                            page.SetInputFocus(3, 2);
                                    }));
                                return;
                            }
                            AuthorizationParams _paramsAuth = new AuthorizationParams()
                            {
                                StoreId = _machinesInfo.StoreId,
                                MachineSn = _machinesInfo.MachineSn,
                                CID = _machinesInfo.CompanyId,
                                Password = Password
                            };
                            var resultAuth = ApiManager.Post<AuthorizationParams, ApiRetrunResult<object>>(@"api/Authorization", _paramsAuth);
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                if (resultAuth.Code == "200")
                                {
                                    CurrentWindow.Dispatcher.Invoke(new Action(() =>
                                    {
                                        //打开支付界面
                                        MultiPay page = new MultiPay(Difference, PosModels.PayAction.RefundAll);
                                        page.Owner = Application.Current.MainWindow;
                                        CurrentWindow.Hide();
                                        page.ShowDialog();
                                        CurrentWindow.Close();
                                        //ZhiFuFangShi page = new ZhiFuFangShi(Difference, PosModels.PayAction.RefundAll, Reason);
                                        //page.Owner = Application.Current.MainWindow;

                                        //CurrentWindow.Hide();
                                        //page.ShowDialog();
                                        //CurrentWindow.Close();
                                    }));
                                }
                            }));
                        });
                    }
                });
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            OrderList = new List<BillDetails>();
            Difference = 0m;
            Password = string.Empty;
            PaySn = string.Empty;
        }

        public bool IsOperatEnabled { get; set; }
    }
}