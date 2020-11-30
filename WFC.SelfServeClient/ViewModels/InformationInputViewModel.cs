﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using WFC.SelfServeClient.Helper;
using WFC.SelfServeClient.Models;
using WFC.ServerClient;
using WFC.ServerClient.HttpModels;
using WFC.ServerClient.Utilities;

namespace WFC.SelfServeClient.ViewModels
{
    public class InformationInputViewModel : Screen
    {
        public event System.Action OnGotoFinishClick;
        private string serverUrl = ConfigurationManager.AppSettings["WebApiServiceUrl"];
        public event System.Action OnGotoWelcomeClick;
        DispatcherTimer gotoWelcomeTimer;
        public BindableCollection<DisplayItem> VisitorArea { get; set; }
        public BindableCollection<DisplayItem> VisitorFloor { get; set; }

        public HendersonVisitor hendersonVisitor { get; set; } = new HendersonVisitor();

        public InformationInputViewModel(HendersonVisitor hendersonVisitor)
        {
            VisitorArea = new BindableCollection<DisplayItem>();
            VisitorArea.Add(new DisplayItem { Id = "wfc.east.tower", Name = "东塔" });
            VisitorArea.Add(new DisplayItem { Id = "wfc.west.tower", Name = "西塔" });
            VisitorFloor = new BindableCollection<DisplayItem>();
            VisitorFloor.Add(new DisplayItem { Id = "-1", Name = "B1M" });
            for (int i = 0; i <= 23; i++)
            {
                VisitorFloor.Add(new DisplayItem { Id = i.ToString(), Name = $"{i + 1}楼" });
            }
            gotoWelcomeTimer = new DispatcherTimer();
            gotoWelcomeTimer.Interval = TimeSpan.FromSeconds(60);
            gotoWelcomeTimer.Tick += Snapshot_Tick;
            gotoWelcomeTimer.Start();
            this.hendersonVisitor = hendersonVisitor;

        }

        public void BtnOK()
        {
            gotoWelcomeTimer.Stop();
            //if (string.IsNullOrWhiteSpace(this.hendersonVisitor.Name))
            //{
            //    MessageBox.Show("请输入访客姓名！");
            //    return;
            //}

            if (string.IsNullOrEmpty(hendersonVisitor.AreaCode))
            {
                MessageBox.Show("请输入手机号码国家代码！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.hendersonVisitor.Phone))
            {
                MessageBox.Show("请输入手机号码！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.hendersonVisitor.IdCardNo))
            {
                MessageBox.Show("请输入身份证号！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.hendersonVisitor.Buildings))
            {
                MessageBox.Show("请选择到访区域！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.hendersonVisitor.Floors))
            {
                MessageBox.Show("请选择到访楼层！");
                return;
            }
            if (string.IsNullOrEmpty(hendersonVisitor.HendersonTenantPersonName))
            {
                MessageBox.Show("请输入受访人！");
                return;
            }
            if (string.IsNullOrEmpty(hendersonVisitor.TenantAreaCode))
            {
                MessageBox.Show("请输入受访人电话国家代码！");
                return;
            }
            if (string.IsNullOrEmpty(hendersonVisitor.HendersonTenantPersonPhone))
            {
                MessageBox.Show("请输入受访人电话！");
                return;
            }
            //if (this.StartTime > this.EndTime)
            //{
            //    MessageBox.Show("卡有效开始日期必须小于结束日期！");
            //    return;
            //}

            try
            {
                string url = serverUrl + "api/v1/hendersonvisitor/visitor";
                Dictionary<string, string> postForm = new Dictionary<string, string>();
                // For Henderson
                postForm.Add("VisitorName", hendersonVisitor.Name);
                postForm.Add("PhoneNumber", hendersonVisitor.Phone);
                postForm.Add("StartTime", new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds().ToString());
                postForm.Add("EndTime", new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59)).ToUnixTimeMilliseconds().ToString());
                postForm.Add("NumberOfAccess", "10");
                postForm.Add("IdCardNumber", hendersonVisitor.IdCardNo);
                postForm.Add("EastFloors", hendersonVisitor.Floors);
                postForm.Add("HendersonTenantPersonPhone", hendersonVisitor.HendersonTenantPersonPhone);
                postForm.Add("Buildings", hendersonVisitor.Buildings);

                // For WFC
                postForm.Add("HendersonTenantId", "");
                postForm.Add("HendersonTenantName", "");
                postForm.Add("Sex", hendersonVisitor.Gender);
                postForm.Add("Nation", hendersonVisitor.Nation);
                postForm.Add("Address", hendersonVisitor.Address);
                postForm.Add("CredentialId", hendersonVisitor.CredentialId);
                postForm.Add("HendersonTenantPersonName", hendersonVisitor.HendersonTenantPersonName);
                postForm.Add("AuthCode", WebApiClientHelper.AccessToken);
                postForm.Add("VisitorType", "SelfHelp");

                Dictionary<string, string> postFile = new Dictionary<string, string>();
                postFile.Add("VisitorPhoto", this.hendersonVisitor.VisitorPhoto);

                var postResult = HttpClientHelper.RestPostFile<AddVisitorResponse>(url, WebApiClientHelper.Jwt, postForm, postFile);
                if (postResult.StatusCode == "SUCCESS")
                {
                    OnGotoFinishClick?.Invoke();
                }
                else
                {
                    gotoWelcomeTimer.Start();
                    MessageBox.Show(postResult.Message);
                }
            }
            catch (Exception ex)
            {
                gotoWelcomeTimer.Start();
                MessageBox.Show(ex.HandleException());
                Logger.Error(ex.HandleException());
            }
        }

        private void Snapshot_Tick(object sender, EventArgs e)
        {
            gotoWelcomeTimer.Stop();
            OnGotoWelcomeClick?.Invoke();
        }
    }
}
