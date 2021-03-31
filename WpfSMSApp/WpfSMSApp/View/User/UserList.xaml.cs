﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace WpfSMSApp.View.User
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserList : Page
    {
        public UserList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RdoAll.IsChecked = true;

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 myaccount loaded : {ex}");
                throw ex;
            }
        }

        private void BtnEditAccount_Click(object sender, RoutedEventArgs e)
        {
           //NavigationService.Navigate(new EditAccount());
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddUser());

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnAddUser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new EditUser());

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnEidtUser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnDeactivateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new DeactiveUser());

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnDeactivateUser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF File(*pdf)|*.pdf";
            saveDialog.FileName = "";
            if (saveDialog.ShowDialog() == true)
            {
                //PDF 변환
                try
                {
                    iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                    string pdfFilePath = saveDialog.FileName;

                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4);
                    //PDF 객체 생성
                    PdfPTable pdfTable = new PdfPTable(GrdData.Columns.Count);

                    //PDF 내용 만들기
                    string naumttf = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), @"D2Coding.ttf");
                    BaseFont nanumBase = BaseFont.CreateFont(naumttf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var nanumFont = new iTextSharp.text.Font(nanumBase, 16f);

                    Paragraph title = new Paragraph($"부경대 Stock Management System : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                    //PDF 파일생성
                    using (FileStream stream = new FileStream(pdfFilePath,FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(title);
                        // 2번에서 만들 내용 추가
                        pdfDoc.Close();
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 BtnExportPdf_Click : {ex}");
                }
            }
            

        }

        private void RdoAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<WpfSMSApp.Model.User> users = new List<Model.User>();

                if (RdoAll.IsChecked ==true)
                {
                    users = Logic.DataAccess.GetUsers();
                }
                this.DataContext = users;
                
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
            }
        }

        private void RdoActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<WpfSMSApp.Model.User> users = new List<Model.User>();

                if (RdoActive.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers().Where(u => u.UserActivated == true).ToList();
                }
                this.DataContext = users;

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
            }
        }

        private void RdoDeactive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<WpfSMSApp.Model.User> users = new List<Model.User>();

                if (RdoDeactive.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers().Where(u => u.UserActivated ==false).ToList();
                }
                this.DataContext = users;

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
            }
        }
    }
}
