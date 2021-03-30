using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using MahApps.Metro.Controls.Dialogs;
namespace WpfSMSApp.View.Account
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditAccount : Page
    {
        public EditAccount()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = 
                    LblUserName.Visibility = LblUserEmail.Visibility = 
                    LblUserPassword.Visibility =LblUserAdmin.Visibility =
                    LblUserActivated.Visibility= Visibility.Hidden;
                //콤보박스 초기화
                List<string> comboValues = new List<string>
                {
                    "False",//index 0
                    "True"  //index 1
                    
                };
                CboUserAdmin.ItemsSource = comboValues;
                CboUserActivated.ItemsSource = comboValues;

                var user = Commons.LOGINED_USER;
                TxtUserID.Text = user.UserID.ToString();
                TxtUserIdentityNumber.Text = user.UserIdentityNumber;
                TxtUserSurName.Text = user.UserSurname;
                TxtUserName.Text = user.UserName;
                TxtUserEmail.Text = user.UserEmail;
                //TxtUserPassword.Password = user.UserPassword; 필요없음
                CboUserAdmin.SelectedIndex = user.UserAdmin == false ? 0 : 1;
                CboUserActivated.SelectedIndex = user.UserActivated == false ? 0 : 1;
                //CboUserActivated.Text = user.UserActivated.ToString();

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 MyAccount Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            LblUserIdentityNumber.Visibility = LblUserSurName.Visibility =
                    LblUserName.Visibility = LblUserEmail.Visibility =
                    LblUserPassword.Visibility = LblUserAdmin.Visibility =
                    LblUserActivated.Visibility = Visibility.Hidden;

            var user = Commons.LOGINED_USER;

            if (string.IsNullOrEmpty(TxtUserSurName.Text))
            {
                LblUserSurName.Visibility = Visibility.Visible;
                LblUserSurName.Text = "이름(성)을 입력하세요";
                isValid = false;
            }
            if (string.IsNullOrEmpty(LblUserName.Text))
            {
                LblUserName.Visibility = Visibility.Visible;
                LblUserName.Text = "이름을 입력하세요";
                isValid = false;
            }
            if (string.IsNullOrEmpty(LblUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "메일을 입력하세요";
                isValid = false;
            }
            if (string.IsNullOrEmpty(LblUserPassword.Text))
            {
                LblUserPassword.Visibility = Visibility.Visible;
                LblUserPassword.Text = "비밀번호를을 입력하세요";
                isValid = false;
            }
            if (isValid)
            {
                user.UserSurname = TxtUserSurName.Text;
                user.UserName = TxtUserName.Text;
                user.UserEmail = TxtUserEmail.Text;
                user.UserPassword = TxtUserPassword.Password;
                user.UserAdmin = bool.Parse(CboUserAdmin.SelectedValue.ToString());
                user.UserAdmin = bool.Parse(CboUserAdmin.SelectedValue.ToString());// parse의 경우에는 문자열을 가져다가 내용을 파악해 다른 유형으로 변환하는 것!
                                                                                   // ()의 경우 변수의 값을 변경하는 것이 아님 값은 동일한 유형으로 유지된다.



                user.UserActivated = bool.Parse(CboUserActivated.SelectedValue.ToString());

                try
                {
                    var mdHash = MD5.Create();
                    user.UserPassword = Commons.GetMd5Hash(mdHash, user.UserPassword);

                    var result = Logic.DataAccess.SetUser(user);
                    if (result == 0)
                    {
                        //수정 안됨
                        LblResult.Text = "계정 수정에 문제가 발생했습니다. 관리자에게 문의 바랍니다.";
                        LblResult.Foreground = Brushes.OrangeRed;
                    }
                    else
                    {
                        //정상적으로 수정됨
                        LblResult.Text = "정상적으로 수정했습니다.";
                        LblResult.Foreground = Brushes.DeepSkyBlue;
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 : {ex}");
                }
            }

        }
    }
}
