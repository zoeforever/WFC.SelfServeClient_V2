using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using WFC.SelfServeClient.Helper;
using WFC.SelfServeClient.Models;
using WFC.ServerClient;
using WFC.ServerClient.HttpModels;
using WFC.ServerClient.Utilities;

namespace WFC.SelfServeClient.ViewModels
{
    public class InformationInputViewModel : Screen
    {
        private string location = ConfigurationManager.AppSettings["Location"];
        private string serverUrl = ConfigurationManager.AppSettings["WebApiServiceUrl"];
        private HendersonTenant hendersonTenant;
        private DisplayItem selectedHendersonTenantPerson;

        public BindableCollection<HendersonTenant> VisitorTenant { get; set; } = new BindableCollection<HendersonTenant>();
        public BindableCollection<DisplayItem> HendersonTenantPerson { get; set; } = new BindableCollection<DisplayItem>();
        public HendersonTenant SelectedVisitorTenant
        {
            get
            {
                return hendersonTenant;
            }
            set
            {
                hendersonTenant = value;
                if (hendersonTenant != null)
                {
                    hendersonVisitor.Floors = string.Join(",", hendersonTenant.Locations.Select(f => f.Floor.Display_name));
                    HendersonTenantPerson = new BindableCollection<DisplayItem>(hendersonTenant.Contacts.Select(t => new DisplayItem { Id = t.Contact_number, Name = t.Display_name }));
                }
            }
        }

        public DisplayItem SelectedHendersonTenantPerson
        {
            get
            {
                return selectedHendersonTenantPerson;
            }
            set
            {
                selectedHendersonTenantPerson = value;
                if (selectedHendersonTenantPerson != null)
                {
                    hendersonVisitor.HendersonTenantPersonPhone = selectedHendersonTenantPerson.Id;
                }
            }
        }
        public HendersonVisitor hendersonVisitor { get; set; } = new HendersonVisitor();

        public InformationInputViewModel(HendersonVisitor hendersonVisitor)
        {
            this.hendersonVisitor = hendersonVisitor;

            GetTenants();

            void GetTenants()
            {
                try
                {
                    var client = WebApiClient.HttpApi.Resolve<IHendersonVisitorApi>();
                    VisitorTenant = new BindableCollection<HendersonTenant>(client.GetTenantsByBuildingIdAsync(location).GetAwaiter().GetResult().Result);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                    VisitorTenant = new BindableCollection<HendersonTenant>();
                }
            }
        }

        public void BtnOK()
        {
            if (string.IsNullOrWhiteSpace(this.hendersonVisitor.Name))
            {
                MessageBox.Show("请输入访客姓名！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.hendersonVisitor.Phone))
            {
                MessageBox.Show("请输入电话！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.hendersonVisitor.IdCardNo))
            {
                MessageBox.Show("请输入身份证号！");
                return;
            }
            //if (Visitor.VisitorPhoto == EmptyPhotoFilePath)
            //{
            //    MessageBox.Show("请拍照或刷身份证！");
            //    return;
            //}
            var hendersonTenant = VisitorTenant.FirstOrDefault(x => x.Id == hendersonVisitor.HendersonTenantId);
            if (hendersonTenant == null)
            {
                MessageBox.Show("请选择到访公司！");
                return;
            }
            var hendersonTenantPerson = HendersonTenantPerson.FirstOrDefault(x => x.Name == hendersonVisitor.HendersonTenantPersonName);
            if (hendersonTenantPerson == null)
            {
                MessageBox.Show("请选择访问人！");
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
                postForm.Add("IdCardNumber", hendersonVisitor.IdCardNo);
                postForm.Add("NumberOfAccess", "10");
                postForm.Add("StartTime", new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds().ToString());
                postForm.Add("EndTime", new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeMilliseconds().ToString());
                postForm.Add("HendersonTenantId", hendersonTenant.Id);
                postForm.Add("HendersonTenantName", hendersonTenant.Display_name);

                // For WFC
                postForm.Add("Sex", hendersonVisitor.Gender);
                postForm.Add("Nation", hendersonVisitor.Nation);
                postForm.Add("Address", hendersonVisitor.Address);
                postForm.Add("CredentialId", hendersonVisitor.CredentialId);
                postForm.Add("HendersonTenantPersonName", hendersonVisitor.HendersonTenantPersonName);
                postForm.Add("HendersonTenantPersonPhone", hendersonVisitor.HendersonTenantPersonPhone);
                postForm.Add("Floors", hendersonVisitor.Floors);

                Dictionary<string, string> postFile = new Dictionary<string, string>();
                postFile.Add("VisitorPhoto", this.hendersonVisitor.VisitorPhoto);

                //var postResult = HttpClientHelper.RestPostFile<VST.BaseEntity.WFC.AddVisitorResponse>(url, WebApiClientHelper.Jwt, postForm, postFile);
                //if (postResult.StatusCode == "SUCCESS")
                //{
                //}
                //else
                //{
                //    MessageBox.Show(postResult.Message);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.HandleException());
                Logger.Error(ex.HandleException());
            }
        }
    }
}
