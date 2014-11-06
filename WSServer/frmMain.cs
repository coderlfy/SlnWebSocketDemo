using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSSocket;

namespace WSServer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            TcpServerEx tcpserver = new TcpServerEx();
            tcpserver._Receiver = new FlashAuthSwitcher();
            tcpserver.StartListen(843);
        }
    }
}
