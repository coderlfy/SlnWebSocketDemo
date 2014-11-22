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
        private static TcpServerEx _tcpFlashAuthServer = null;
        private static TcpServerEx _tcpMsgServer = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            if (!_tcpFlashAuthServer._IsSuccessStarted)
                _tcpFlashAuthServer.StartListen(843);
            if (!_tcpMsgServer._IsSuccessStarted)
                _tcpMsgServer.StartListen(1818);
            this.btnStartService.Enabled = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (_tcpFlashAuthServer == null)
            {
                _tcpFlashAuthServer = new TcpServerEx();
                _tcpFlashAuthServer._Receiver = new FlashAuthSwitcher();
            }
            if (_tcpMsgServer == null)
            {
                _tcpMsgServer = new TcpServerEx();
                _tcpMsgServer._Receiver = new WSServerSwitcher();
            }

        }
    }
}
