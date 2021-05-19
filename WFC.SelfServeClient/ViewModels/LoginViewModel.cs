using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WFC.SelfServeClient.Helper;
using WFC.ServerClient;

namespace WFC.SelfServeClient.ViewModels
{
    [Export(typeof(LoginViewModel))]
    [ImplementPropertyChanged]
    public class LoginViewModel : Conductor<Screen>
    {
        private static string OrganizationIdentifier = ConfigurationManager.AppSettings["OrganizationIdentifier"];
        public string Phone { get; set; }
        public string Code { get; set; }
        public string AreaCode { get; set; } = "86";

        public event System.Action OnValidateSuccess;

        IAccountsApi client;
        public LoginViewModel()
        {
            client = WebApiClient.HttpApi.Resolve<IAccountsApi>();
        }
        public async void GetCode()
        {
            if (string.IsNullOrEmpty(AreaCode))
            {
                MessageBox.Show("请输入国家区号");
                return;
            }
            if (string.IsNullOrEmpty(Phone))
            {
                MessageBox.Show("请输入手机号");
                return;
            }

            try
            {
                // 获取验证码
#if !TEST
                await client.AccountsAsync(new HendersonLoginRequest { AreaCode = AreaCode, PhoneNumber = Phone, OrganizationIdentifier = OrganizationIdentifier });
#endif
                MessageBox.Show("验证码发送成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取验证码失败");
                Logger.Error(ex.ToString());
            }
        }

        public async void Validate()
        {
            if (string.IsNullOrEmpty(AreaCode))
            {
                MessageBox.Show("请输入国家区号");
                return;
            }
            if (string.IsNullOrEmpty(Phone))
            {
                MessageBox.Show("请输入手机号");
                return;
            }
            if (string.IsNullOrEmpty(Code))
            {
                MessageBox.Show("请输入验证码");
                return;
            }
            try
            {
#if TEST
                OnValidateSuccess?.Invoke();
#else
                // 校验验证码
               
                  var verifyCodeResponse = await client.VerifyCodeAsync(new VerifyCodeRequest { AreaCode = AreaCode, PhoneNumber = Phone, VerifyCode = Code });
                  //var verifyCodeResult = await verifyCodeResponse..ReadAsStringAsync();
                  //var token = JsonConvert.DeserializeObject<VerifyCodeResponse>(verifyCodeResult);
                if (verifyCodeResponse.StatusCode == "SUCCESS")
                  {
                      if (verifyCodeResponse.Result == null || verifyCodeResponse.Result.Count == 0 || string.IsNullOrEmpty(verifyCodeResponse.Result[0].access_token))
                      {
                          MessageBox.Show("验证失败");
                      }
                      else
                      {

                          WebApiClientHelper.AccessToken = verifyCodeResponse.Result[0].access_token;
                          WebApiClientHelper.RefreshToken = verifyCodeResponse.Result[0].refresh_token;
                          OnValidateSuccess?.Invoke();
                      }
                  }
                  else
                  {
                      MessageBox.Show("验证失败");
                  }
#endif
            }
            catch (Exception ex)
              {
                  MessageBox.Show("验证失败!");
                  Logger.Error(ex.ToString());
              
            }
            }
    }
}
