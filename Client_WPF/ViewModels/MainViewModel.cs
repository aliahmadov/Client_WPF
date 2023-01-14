﻿using Client_WPF.Commands;
using Client_WPF.Helpers;
using Client_WPF.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client_WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {


        public RelayCommand DragDropCommand { get; set; }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; OnPropertyChanged(); }
        }


        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        public byte[] ImageBytes { get; set; }
        public RelayCommand ChooseImageCommand { get; set; }

        public RelayCommand ConnectCommand { get; set; }

        public RelayCommand SendToServerCommand { get; set; }

        public Item ClientItem { get; set; }
        public bool IsConnected { get; set; }

        public Socket Socket { get; set; }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return null;
        }

        public MainViewModel()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAddress = IPAddress.Parse(GetLocalIPAddress());
            var port = 80;
            var endPoint = new IPEndPoint(ipAddress, port);

            DragDropCommand = new RelayCommand(c =>
            {
                var d = c as DragEventArgs;

                if (d.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])d.Data.GetData(DataFormats.FileDrop);
                    string fileName = Path.GetFileName(files[0]);
                    ImagePath = files[0];
                    ImageBytes = File.ReadAllBytes(ImagePath);
                }

            });

            ChooseImageCommand = new RelayCommand(c =>
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
            });


            ConnectCommand = new RelayCommand(c =>
            {
                try
                {
                    Socket.Connect(endPoint);
                    IsConnected = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Not possible to connect");
                    throw;
                }
            });



            SendToServerCommand = new RelayCommand(c =>
            {
                ClientItem = new Item { ImageBytes = ImageBytes, Title = Title };
                try
                {
                    if (Socket.Connected)
                    {
                        MessageBox.Show("Connected to Server");
                        while (true)
                        {
                            var jsonString=FileHelper<Item>.Serialize(ClientItem);
                            var bytes = Encoding.UTF8.GetBytes(jsonString);
                            Socket.Send(bytes);
                        }
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Not possible to connect to the server");
                    throw;
                }
            }, (a) =>
            {
                if (IsConnected && Title != null) return true;
                return false;
            });


        }



    }
}
