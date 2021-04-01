using iTextSharp.text;
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
                    //0. PDF 사용 폰트 설정
                    string nanumPath = Path.Combine(Environment.CurrentDirectory, @"NanumGothic.ttf"); //폰트 경로
                    BaseFont nanumBase = BaseFont.CreateFont(nanumPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var nanumTitle = new iTextSharp.text.Font(nanumBase, 20f); // 20 타이틀용 나눔 폰트;
                    var nanumContent = new iTextSharp.text.Font(nanumBase, 12f); // 12 내용에 사용하는 폰트;


                   // iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                    string pdfFilePath = saveDialog.FileName;

                    // 1.PDF 객체 생성
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4);


                    // 2.PDF 내용 만들기
                    Paragraph title1 = new Paragraph("부경대 재고관리시스템(SMS)\n",nanumTitle);
                    Paragraph subtitle = new Paragraph($"사용자 리스트 exported : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n\n",nanumContent);

                    PdfPTable pdfTable = new PdfPTable(GrdData.Columns.Count);
                    pdfTable.WidthPercentage = 100; //전체 사이즈 다 쓰는 것

                    //그리드 헤더 작업
                    foreach (DataGridColumn column in GrdData.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString(),nanumContent));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        
                        pdfTable.AddCell(cell);
                    }
                    float[] columnsWidth = new float[] { 5f, 15f, 10f, 10f, 30f, 15f, 10f, };
                    pdfTable.SetWidths(columnsWidth);

                    //그리드 Row 작업
                    foreach (var item in GrdData.Items)
                    {
                        if (item is Model.User)
                        {
                            var temp = item as Model.User;
                            //UserID 
                            PdfPCell cell = new PdfPCell(new Phrase(temp.UserID.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserIdentityNumber.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserSurname.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserName.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserEmail.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserAdmin.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserActivated.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);
                        }
                    }

                    // 3.PDF 파일생성
                    using (FileStream stream = new FileStream(pdfFilePath,FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(title1);
                        pdfDoc.Add(subtitle);
                        pdfDoc.Add(pdfTable);
                        // 2번에서 만들 내용 추가
                        pdfDoc.Close();
                        stream.Close();
                    }
                    Commons.ShowMessageAsync("PDF변환","PDF 익스포트 성공했습니다.");
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
