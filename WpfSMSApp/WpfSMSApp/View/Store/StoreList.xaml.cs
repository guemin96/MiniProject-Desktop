﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StoreList : Page
    {
        public StoreList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Store 테이블 데이터 읽어와야 함
                List<Model.Store> stores = new List<Model.Store>();
                List<Model.StockStore> stockStores = new List<Model.StockStore>();
                List<Model.Stock> stocks = new List<Model.Stock>();
                stores = Logic.DataAccess.GetStores();
                stocks = Logic.DataAccess.GetStocks();

                //store 데이터를 stockStores로 옮김
                foreach (Model.Store item in stores)
                {
                    stockStores.Add(new Model.StockStore()
                    {
                        StoreID = item.StoreID,
                        StoreName =item.StoreName,
                        StoreLocation=item.StoreLocation,
                        ItemStatus=item.ItemStatus,
                        TagID=item.TagID,
                        BarcodeID = item.BarcodeID,
                        StockQuantity = 0

                    }) ;
                }


                this.DataContext = stores;

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 StoreList loaded : {ex}");
                throw ex;
            }
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddStore());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnAddUser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}